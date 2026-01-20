using TMPro;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    #region FIELDS

    public static GameManager Instance { get; private set; }
    
    [Header("UI Controller")]
    [SerializeField] private GameUIController ui;

    [Space]
    [Header("Chat Manager")]
    [SerializeField] private ChatManager chatManager;


    [Space]
    [Header("About Panel")]
    [SerializeField] private GameObject aboutPanel;
    [SerializeField] private RoleGameObject[] roleObjects;
    [SerializeField] private RoleGameObject[] roleIcons;
    [SerializeField] private PlayerSlot[] playerSlots;
    [SerializeField] private TextMeshProUGUI playerCountText;

    [Space]
    [Header("Game Panel")]
    [SerializeField] private GameObject gameContainer;
    [SerializeField] private RoleCounter roleCounterScript;
    [SerializeField] private PhaseHandler phaseHandlerScript;
    [SerializeField] private Transform playersListTransform;
    [SerializeField] private GameObject playersListPrefab;
    [SerializeField] private PlayerSlot[] dayPlayerSlots;
    [SerializeField] private PlayerSlot[] voteNightPlayerSlots;
    [SerializeField] private PlayerSlot[] voteDayPlayerSlots;

    [Header("Your Role Text")]
    [SerializeField] private TextMeshProUGUI redYourRoleText;
    [SerializeField] private TextMeshProUGUI blueYourRoleText;
    [SerializeField] private TextMeshProUGUI yellowYourRoleText;
    [SerializeField] private TextMeshProUGUI greenYourRoleText;
    [SerializeField] private TextMeshProUGUI pinkYourRoleText;
    [SerializeField] private TextMeshProUGUI orangeYourRoleText;
    
    [Space]
    [Header("Avatars")]
    public Sprite[] avatarSprites;
    [SerializeField] private Sprite[] smallAvatarSprites;
    #endregion

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
    private void Start()
    {
        if(!GameState.Instance.IsReconnectReady) return;
        UpdateRoom(GameState.Instance.roomUpdateResponse);
        LoadRoom(GameState.Instance.CurrentRoom);
        ShowGamePhase(GameState.Instance.CurrentPhase);
        LoadDayPlayersList(GameState.Instance.DayPlayersList);
        UpdateTimer(GameState.Instance.PhaseTimer);
    }
    private void OnEnable()
    {
        EventBus.OnRoomUpdated += UpdateRoom;
        EventBus.OnChatMessage += chatManager.OnChatMessage;
        EventBus.OnAutoTimer += GameStartTimer;
        EventBus.OnCancelAutoTimer += GameCancelAutoTimer;
        EventBus.OnRoleReceived += DisplayYourRole;
        EventBus.OnDayPlayersList += LoadDayPlayersList;
        EventBus.OnPhaseUpdated += ShowGamePhase;
        EventBus.OnNightAction += ShowNightVotePlayersList;
        EventBus.OnVoteStateUpdated += ShowVoteState;
        EventBus.OnNightEnd += chatManager.ShowNightDeaths;
        EventBus.OnDayEnd += chatManager.ShowDayEnd;
        EventBus.OnGameOver += ShowWinner;
        EventBus.OnTimerPhaseUpdated += UpdateTimer;

        LoadRoom(GameState.Instance.CurrentRoom);
        UpdateRoom(GameState.Instance.roomUpdateResponse);
        DisplayYourRole(GameState.Instance.YourRole);
    }
    private void OnDisable()
    {
        EventBus.OnRoomUpdated -= UpdateRoom;
        EventBus.OnChatMessage -= chatManager.OnChatMessage;
        EventBus.OnAutoTimer -= GameStartTimer;
        EventBus.OnCancelAutoTimer -= GameCancelAutoTimer;
        EventBus.OnRoleReceived -= DisplayYourRole;
        EventBus.OnDayPlayersList -= LoadDayPlayersList;
        EventBus.OnPhaseUpdated -= ShowGamePhase;
        EventBus.OnNightAction -= ShowNightVotePlayersList;
        EventBus.OnVoteStateUpdated -= ShowVoteState;
        EventBus.OnNightEnd -= chatManager.ShowNightDeaths;
        EventBus.OnDayEnd -= chatManager.ShowDayEnd;
        EventBus.OnGameOver -= ShowWinner;
        EventBus.OnTimerPhaseUpdated -= UpdateTimer;
    }
    #region Update Info
    private void LoadRoom(RoomFullInfo roomData)
    {
        if(roomData == null) return;
        ui.SetRoomName(roomData.name);
        ui.SetRoomLevel(roomData.level.ToString());
        switch (roomData.phase)
        {
            case "waiting":
                ui.SetStatus("Идет подбор");
                break;  
        }

        ActivateRoleObjects(roomData.roles);
        ActivateRoleIcons(roomData.roles);
        UpdatePlayers(roomData.players);
        playerCountText.text = roomData.playerCount +"/" + roomData.max_players.ToString();
    }
    private void UpdatePlayers(RoomPlayer[] players)
    {
        int i = 0;
        if (playerSlots == null) return;
        foreach(PlayerSlot playerSlot in playerSlots)
        {
            playerSlot.gameObject.SetActive(false);
        }
        foreach (RoomPlayer player in players)
        {
            playerSlots[i].SetPlayerSlot(player.id, avatarSprites[player.avatar_id], player.username);
            i++;
        }
    }
    #endregion
    #region CHAT
    private void UpdateRoom(RoomUpdateResponse roomUpdateResponse)
    {
    if (roomUpdateResponse == null)
        return;
    
    chatManager.OnPlayerEnter(roomUpdateResponse.player_enter.username);
    chatManager.OnPlayerLeft(roomUpdateResponse.player_left.username);

    UpdatePlayers(roomUpdateResponse.players);
    ui.playerCountText.text =
        roomUpdateResponse.playerCount + "/" + roomUpdateResponse.maxPlayerCount;
    }
    

    #endregion
    #region ABOUT
    
    private void ActivateRoleObjects(string[] roles)
    {
        foreach (string role in roles)
        {
            foreach (var item in roleObjects)
            {
                if (item.role.ToString() == role)
                {
                    if (item.roleIcon != null)
                    {
                        item.roleIcon.SetActive(true);
                    }
                }
            }
        }
    }
    private void ActivateRoleIcons(string[] roles)
    {
        foreach (string role in roles)
        {
            foreach (var item in roleIcons)
            {
                if (item.role.ToString() == role)
                {
                    if (item.roleIcon != null)
                    {
                        item.roleIcon.SetActive(true);
                    }
                }
            }
        }
    }

    #endregion
    #region Game
    #region Auto-Start
    
    public void LeaveRoom()
    {
        WebSocketManager.Instance.SendLeaveRoom();
        LoadingManager.Instance.LoadRoomScene();
    }
    private void GameStartTimer(AutoStartResponse autoStartResponse)
    {
        Timer.Instance.StartTimer(autoStartResponse.seconds, ui.statusText,
            "Игра начнется через ", " секунд");
    }
    private void GameCancelAutoTimer(BaseMessage msg)
    {
        Timer.Instance.StopTimer();        
        ui.statusText.text = "Идет подбор";
    }
    #endregion
    #region Day phase
    private void LoadDayPlayersList(DayPlayersListResponse dayPlayersListResponse)
    {
        if (dayPlayersListResponse == null) { Debug.Log("PUSTO day players list");  return; }
        ui.EnterGameMode(); // 🔹
        ui.SetStatus($"День {dayPlayersListResponse.day}");
        roleCounterScript.DisplayRolePoints(
            dayPlayersListResponse.stats.alive_mafia,
            dayPlayersListResponse.stats.dead_mafia, 
            dayPlayersListResponse.stats.alive_peaceful,
            dayPlayersListResponse.stats.dead_peaceful
            );
        ShowDayPlayersList(dayPlayersListResponse);
    }
    private void ShowVoteState(VoteStateUpdateResponse response) //�������� - ���������� � �����
    {
        if (response.voteType == "day") {
            ShowDayVoteState(response);
        }
        else
        {
            ShowNightVoteState(response);
        }
    }
    private void UpdateTimer(PhaseTimerResponse phaseTimerResponse)
    {
        if (phaseTimerResponse == null) { Debug.Log("PUSTO phase timer"); return; }
        phaseHandlerScript.UpdateTimer(phaseTimerResponse.seconds_left, phaseTimerResponse.phase);
    }
    private void ShowGamePhase(PhaseUpdateResponse phaseUpdateResponse)
    {
        if(phaseUpdateResponse == null) return;
        switch (phaseUpdateResponse.phase)
        {
            case "day":
                phaseHandlerScript.SetDayTimer(phaseUpdateResponse.duration);
                ui.EnableDayPlayerPanel();
                ui.DisableVoteNightPlayersPanel();

                ui.SetChatBlockPanel(false);

                chatManager.AddDaySeparator();
                break;
            case "night":
                phaseHandlerScript.SetNightTimer(phaseUpdateResponse.duration);
                ui.DisableVoteDayPlayersPanel();
                ui.SetChatBlockPanel(true);

                chatManager.AddNightSeparator();
                
                break;
            case "vote":
                ui.EnableVoteDayPlayersPanel();
                ui.DisableDayPlayerPanel();
                ui.DisableVoteNightPlayersPanel();
                phaseHandlerScript.SetVoteTimer(phaseUpdateResponse.duration);

                ui.SetChatBlockPanel(false);
                break;
        }
    }
    private void ShowDayPlayersList(DayPlayersListResponse response)
    {
        int i = 0;
        foreach (PlayerSlot playerSlot in dayPlayerSlots)
        {
            playerSlot.gameObject.SetActive(false);
        }
        foreach (PlayerSlot playerSlot in voteDayPlayerSlots)
        {
            playerSlot.gameObject.SetActive(false);
        }
        foreach (VotePlayer player in response.players)
        {
            dayPlayerSlots[i].SetPlayerSlot(player.user_id, avatarSprites[player.avatar_id],
                player.username, player.is_mafia);
            
            voteDayPlayerSlots[i].SetPlayerSlot(player.user_id, avatarSprites[player.avatar_id],
                player.username, player.is_mafia);
            i++;
        }
    }
    private void ShowNightVotePlayersList(NightActionStartResponse response)
    {
        ui.DisableDayPlayerPanel();
        ui.DisableVoteNightPlayersPanel();
        
        switch (response.role)
        {
            case "mafia":
                ui.ShowMafiaVote();
                break;
            case "doctor":
                ui.ShowDoctorVote();
                break;
            case "sherif":
                ui.ShowSherifVote();
                break;
            case "lover":
                ui.ShowLoverVote();
                break;
            case "priest":
                ui.ShowPriestVote();
                break;
            case "bodyguard":
                ui.ShowBodyguardVote();
                break;
            case "sniper":
                ui.ShowSniperVote();
                break;
        }
        int i = 0;
        foreach (PlayerSlot playerSlot in voteNightPlayerSlots)
        {
            playerSlot.gameObject.SetActive(false);
        }
        foreach (VotePlayer player in response.players)
        {
            voteNightPlayerSlots[i].SetPlayerSlot(player.user_id, avatarSprites[player.avatar_id],
                player.username, player.is_mafia);
            i++;
            
        }
    }
    private void ShowDayVoteState(VoteStateUpdateResponse dayVoteStateResponse)
    {
        int i = 0;
        foreach (VoteResultPlayer player in dayVoteStateResponse.players)
        {
            voteDayPlayerSlots[i].ShowVotes(player.user_id, player.votes, player.is_mafia);
            i++;
        }
    }
    private void ShowNightVoteState(VoteStateUpdateResponse nightVoteStateResponse)
    {
        int i = 0;
        foreach (VoteResultPlayer player in nightVoteStateResponse.players)
        {
            voteNightPlayerSlots[i].ShowVotes(player.user_id, player.votes, player.is_mafia);
            i++;
        }
    }
    private void DisplayYourRole(YourRoleResponse role)
    {
        if (role == null) return;
        switch (role.role)
        {
            case "mafia":
                ShowRoleMafia(redYourRoleText, "Мафия");
                break;
            case "doctor":
                ShowRoleCitizen(blueYourRoleText, "Доктор");
                break;
            case "sherif":
                ShowRoleCitizen(blueYourRoleText, "Шериф");
                break;
            case "citizen":
                ShowRoleCitizen(blueYourRoleText, "Мирный");
                break;
            case "sniper":
                ShowRoleCitizen(orangeYourRoleText, "Снайпер");
                break;
            case "lover":
                ShowRoleCitizen(pinkYourRoleText, "Любовница");
                break;
            case "bodyguard":
                ShowRoleCitizen(yellowYourRoleText, "Телохранитель");
                break;
            case "priest":
                ShowRoleCitizen(yellowYourRoleText, "Священник");
                break;
        }
    }

    private void ShowRoleMafia(TextMeshProUGUI text, string name)
        {
            text.gameObject.SetActive(true);
            text.text = name;

            // ��������� ���������
            text.alpha = 0f;
            text.transform.localScale = Vector3.one * 0.8f;

            Sequence seq = DOTween.Sequence();

            seq.Append(text.DOFade(1f, 0.4f).SetEase(Ease.OutQuad));
            seq.Join(text.transform.DOScale(1.1f, 0.6f).SetEase(Ease.OutBack));

            seq.AppendInterval(0.8f);

            seq.Append(text.DOFade(0f, 0.5f).SetEase(Ease.InQuad));
            seq.Join(text.transform.DOScale(1f, 0.5f).SetEase(Ease.InOutSine));

            seq.OnComplete(() =>
            {
                text.gameObject.SetActive(false);
            });
        }

    private void ShowRoleCitizen(TextMeshProUGUI text, string role)
        {
            text.gameObject.SetActive(true);
            text.text = role;

            text.alpha = 0f;
            text.transform.localScale = Vector3.one * 0.8f;

            Sequence seq = DOTween.Sequence();

            seq.Append(text.DOFade(1f, 0.4f).SetEase(Ease.OutQuad));
            seq.Join(text.transform.DOScale(1.1f, 0.6f).SetEase(Ease.OutBack));

            seq.AppendInterval(0.8f);

            seq.Append(text.DOFade(0f, 0.5f).SetEase(Ease.InQuad));
            seq.Join(text.transform.DOScale(1f, 0.5f).SetEase(Ease.InOutSine));

            seq.OnComplete(() =>
            {
                text.gameObject.SetActive(false);
            });
        }

    private void ShowWinner(GameOverResponse response)
    {
        ui.DisableGamePanels();
        Timer.Instance.StopTimer();
        ui.DisableChoosePanels();
        ui.EnableChatPanels();
        
        if (response.winner == "mafia")
        {
            ShowRoleMafia(redYourRoleText, "Победила мафия!");
            chatManager.AddSystemMessage("Мафия побеждает");
        }
        else
        {
            ShowRoleCitizen(blueYourRoleText, "Победили мирные!");
            chatManager.AddSystemMessage("Мирные побеждают");
        }
    }
    #endregion

    #endregion
}
