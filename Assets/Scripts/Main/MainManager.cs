using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    #region Singleton
    public static MainManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
        else
        {
            Instance = this;
        }
    }
    #endregion

    #region Main

    public Sprite[] avatarSprites;
    public Sprite[] largeAvatarSprites;

    [Header("RATING")]
    [SerializeField] private RatingSlot yourSlot;
    [SerializeField] private RatingSlot[] ratingSlots;
    [SerializeField] private FriendsAndRequestListController friendsListController;
    [SerializeField] private InvitePanelController invitePanelController;
    [SerializeField] private ProfileController profileController;
    [SerializeField] private AvatarShopController avatarShopController;
    #endregion


    #region Rating


    #endregion
    private void Start()
    {
        profileController.SetupPlayerInfo(GameState.Instance.PlayerProfile);
        WebSocketManager.Instance.SendGetRating();
        WebSocketManager.Instance.SendGetFriends();
        WebSocketManager.Instance.SendGetUserStats(GameState.Instance.userId);
    }
    private void OnEnable()
    {
        EventBus.OnRatingReceive += LoadRatingScreen;
        EventBus.OnFriendsList += friendsListController.LoadFriendsList;
        EventBus.OnSearchUsers += friendsListController.LoadPlayersList;
        EventBus.OnFriendsRequestList += friendsListController.LoadRequests;
        EventBus.OnGameInvite += invitePanelController.LoadInvite;
        EventBus.OnRoomJoined += LoadGame;
        EventBus.OnUserStats += profileController.LoadProfile;
        EventBus.OnUserStats += profileController.SetupPlayerInfo;
        EventBus.OnAvatarShop += avatarShopController.LoadAvatars;
    }
    private void OnDisable()
    {
        EventBus.OnRatingReceive -= LoadRatingScreen;
        EventBus.OnFriendsList -= friendsListController.LoadFriendsList;
        EventBus.OnSearchUsers -= friendsListController.LoadPlayersList;
        EventBus.OnFriendsRequestList -= friendsListController.LoadRequests;
        EventBus.OnGameInvite -= invitePanelController.LoadInvite;
        EventBus.OnRoomJoined -= LoadGame;
        EventBus.OnUserStats -= profileController.LoadProfile;
        EventBus.OnAvatarShop -= avatarShopController.LoadAvatars;
        EventBus.OnUserStats -= profileController.SetupPlayerInfo;
    }
    
    private void LoadRatingScreen(RatingResultResponse response)
    {
        yourSlot.SetRatingSlot(response.me.place, response.me.experience, response.me.username,
            avatarSprites[response.me.avatar_id]);
        int i = 0;
        foreach (RatingSlot slot in ratingSlots) 
        {
            slot.gameObject.SetActive(false);
        }
        foreach(RatingPlayer playerSlot in response.top)
        {
            ratingSlots[i].gameObject.SetActive(true);
            ratingSlots[i].SetRatingSlot(playerSlot.place, playerSlot.experience,
                playerSlot.username, avatarSprites[playerSlot.avatar_id]);
            i++;
        }
        i = 0;
    }
    private void LoadGame(JoinRoomSuccessResponse response)
    {
        LoadingManager.Instance.LoadGameScene();
    }
    
    #region MethodsForButtons
    public void OnPlayClicked()
    {
        LoadingManager.Instance.LoadRoomScene();
    }
    public void OnCreateGameClicked()
    {
        LoadingManager.Instance.LoadRoomScene();
        GameState.Instance.isCreateButton = true;
    }
    public void OnLogoutClicked()
    {
        WebSocketManager.Instance.Logout();
    }


    #endregion

}
