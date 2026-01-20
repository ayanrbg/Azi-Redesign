using UnityEngine;

public class ChatManager : MonoBehaviour
{
    [Header("Chat Root")]
    [SerializeField] private Transform chatContent;

    [Header("Prefabs")]
    [SerializeField] private PlayerEnterPrefab playerEnterPrefab;
    [SerializeField] private PlayerMessage playerMessagePrefab;
    [SerializeField] private KilledPlayerPrefab killedPlayerPrefab;
    [SerializeField] private KilledPlayerPrefab healedPlayerPrefab;
    [SerializeField] private KilledPlayerPrefab blockedPlayerPrefab;
    [SerializeField] private DayEndPrefab dayEndPrefab;
    [SerializeField] private ChatSeparator dayChatPrefab;
    [SerializeField] private ChatSeparator nightChatPrefab;

    [Header("Avatars")]
    [SerializeField] private Sprite[] avatarSprites;
    [SerializeField] private Sprite[] smallAvatarSprites;

    private ObjectPool<PlayerEnterPrefab> enterPool;
    private ObjectPool<PlayerMessage> messagePool;
    private ObjectPool<KilledPlayerPrefab> killedPool;
    private ObjectPool<KilledPlayerPrefab> healedPool;
    private ObjectPool<KilledPlayerPrefab> blockedPool;
    private ObjectPool<DayEndPrefab> dayEndPool;
    private ObjectPool<PlayerEnterPrefab> systemMessagePool;
    private ObjectPool<ChatSeparator> daySeparatorPool;
    private ObjectPool<ChatSeparator> nightSeparatorPool;


    // ===================== LIFECYCLE =====================

    private void Awake()
    {
        enterPool   = new ObjectPool<PlayerEnterPrefab>(playerEnterPrefab, chatContent, 5);
        messagePool = new ObjectPool<PlayerMessage>(playerMessagePrefab, chatContent, 10);
        killedPool  = new ObjectPool<KilledPlayerPrefab>(killedPlayerPrefab, chatContent, 5);
        healedPool = new ObjectPool<KilledPlayerPrefab>(healedPlayerPrefab, chatContent, 5);
        blockedPool = new ObjectPool<KilledPlayerPrefab>(blockedPlayerPrefab, chatContent, 5);
        dayEndPool  = new ObjectPool<DayEndPrefab>(dayEndPrefab, chatContent, 5);
        daySeparatorPool   = new ObjectPool<ChatSeparator>(dayChatPrefab, chatContent, 3);
        nightSeparatorPool = new ObjectPool<ChatSeparator>(nightChatPrefab, chatContent, 3);
    }

    // ===================== PUBLIC API =====================

    public void OnPlayerEnter(string username)
    {
        if(string.IsNullOrEmpty(username)) return;

        var item = enterPool.Get();

        if (username == GameState.Instance.PlayerProfile.username)
            item.usernameText.text = "Вы вошли в игру";
        else
            item.usernameText.text = $"{username} вошел в игру";
    }
    public void OnPlayerLeft(string username)
    {
        if (string.IsNullOrEmpty(username)) return;

        var item = enterPool.Get();
        item.usernameText.text = $"{username} вышел";
    }
    public void AddSystemMessage(string text)
    {
        var item = enterPool.Get();
        item.usernameText.text = text;
    }

    public void OnChatMessage(ChatMessageResponse msg)
    {
        var item = messagePool.Get();

        item.SetMessage(
            avatarSprites[msg.avatar_id],
            msg.text,
            msg.username,
            msg.time
        );
    }

    public void AddDaySeparator()
    {
        daySeparatorPool.Get();
    }

    public void AddNightSeparator()
    {
        nightSeparatorPool.Get();
    }

    public void ShowNightDeaths(NightEndSummaryResponse response)
    {
        if (response == null || response.deaths.Length == 0) return;

        foreach (var player in response.deaths)
        {
            var item = killedPool.Get();

            item.SetKilledPlayer(
                player.username,
                player.role,
                smallAvatarSprites[player.avatar_id]
            );
        }
        if(!string.IsNullOrEmpty(response.healed.username))
        {
            var item = healedPool.Get();

            item.SetKilledPlayer(
                response.healed.username,
                response.healed.role,
                smallAvatarSprites[response.healed.avatar_id]
            );
        }
        if (!string.IsNullOrEmpty(response.blocked.username))
        {
            var item = blockedPool.Get();

            item.SetKilledPlayer(
                response.blocked.username,
                response.blocked.role,
                smallAvatarSprites[response.blocked.avatar_id]
            );
        }
    }

    public void ShowDayEnd(DayEndSummaryResponse response)
    {
        if (response == null || response.votes.Length == 0) return;

        foreach (var vote in response.votes)
        {
            var item = dayEndPool.Get();

            string fromName = vote.from.username;
            Color fromColor = Color.white;

            string toName = vote.to.username;
            Color toColor = Color.white;

            if (vote.from.user_id == GameState.Instance.userId)
            {
                fromName = "Вы";
                fromColor = new Color32(255, 208, 87, 255); 
            }
            else if (vote.to.user_id == GameState.Instance.userId)
            {
                toName = "Вы";
                toColor = new Color32(255, 208, 87, 255);
            }
            
            item.Init(
                fromName,
                toName,
                smallAvatarSprites[vote.from.avatar_id],
                smallAvatarSprites[vote.to.avatar_id],
                fromColor, toColor

            );
        }
        KilledPlayerPrefab killedPlayer = killedPool.Get();
        killedPlayer.SetKilledPlayer(response.killed.username, response.killed.role,
            avatarSprites[response.killed.avatar_id]);
    }

    // ===================== OPTIONAL =====================

    public void ClearChat()
    {
        foreach (Transform child in chatContent)
            child.gameObject.SetActive(false);
    }
}
