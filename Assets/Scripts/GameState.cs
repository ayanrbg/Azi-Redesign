using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public static GameState Instance;
    

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadSavedToken();
        }
        else Destroy(gameObject);
    }

    public void ResetState()
    {
        UserToken = null;
        PlayerProfile = null;

        CurrentRoom = null;
        PlayerProfile = null;
        userId = 0;
        roomUpdateResponse = null;
        HasRoomInfo = false;
        Rooms = null;
        CurrentFriendsList = null;
        CurrentFriendsRequests = null;
        CurrentPhase = null;
        CurrentPlayers = null;
        CurrentVoteState = null;
        CurrentRoom = null;
        CurrentProfileStats = null;
        IsGameStateReady = false;
        DayPlayersList = null;
        dayEndSummaryResponse = null;
        nightEndSummaryResponse = null;
        NightActionStartResponse = null;
        voteStateUpdateResponse = null;
    }

    public bool isCreateButton = false;
    // ===================== TOKEN =========================

    public string UserToken;

    public void UpdateToken(string newToken)
    {
        UserToken = newToken;
        PlayerPrefs.Save();
    }

    void LoadSavedToken()
    {
        if (PlayerPrefs.HasKey("TOKEN"))
            UserToken = PlayerPrefs.GetString("TOKEN");
    }

    // ===================== PROFILE =======================

    public UserData PlayerProfile;
    public int userId;

    public void SetPlayerProfile(AuthSuccessResponse profile)
    {
        userId = profile.userId;
        PlayerProfile = profile.userData;

     
    }

    // ===================== LOBBY ROOMS ===================

    public RoomInfo[] Rooms;

    public void SetRooms(RoomInfo[] rooms)
    {
        Rooms = rooms;
    }

    // ===================== CURRENT ROOM ==================

    public RoomFullInfo CurrentRoom;    // Только комната
    public RoomPlayer[] CurrentPlayers; // Текущий список игроков
    public RoomUpdateResponse roomUpdateResponse;

    public void SetCurrentRoom(JoinRoomSuccessResponse room)
    {
        CurrentRoom = room.room;
        CurrentPlayers = room.room.players;

        EventBus.RaiseRoomJoined(room);
    }

    public void ApplyRoomUpdate(RoomUpdateResponse update)
    {
        if (CurrentRoom == null)
            return;
        roomUpdateResponse = update;
        CurrentRoom.players = update.players;
        CurrentPlayers = update.players;

        EventBus.RaiseRoomUpdated(update);
    }
    // ===================== PROFILE ======================

    public UserStats CurrentProfileStats;

    public void SetProfileStats(UserStats msg)
    {
        CurrentProfileStats = msg;
    }
    // ===================== FRIENDS ======================

    public FriendsListResponse CurrentFriendsList;
    public FriendRequestsListResponse CurrentFriendsRequests;

    public void SetCurrentFriends(FriendsListResponse msg)
    {
        CurrentFriendsList = msg;
    }
    public void SetFriendsRequests(FriendRequestsListResponse msg)
    {
        CurrentFriendsRequests = msg;
    }
    // ===================== GAME DATA ======================

    public PhaseUpdateResponse CurrentPhase;
    public YourRoleResponse YourRole;
    public DayPlayersListResponse DayPlayersList;
    public VoteStateUpdateResponse CurrentVoteState;
    public NightActionStartResponse NightActionStartResponse;
    public DayEndSummaryResponse dayEndSummaryResponse;
    public NightEndSummaryResponse nightEndSummaryResponse;
    public VoteStateUpdateResponse voteStateUpdateResponse;

    public void SetPhase(PhaseUpdateResponse msg)
    {
        CurrentPhase = msg;
        HasPhase = true;
        TryFinishReconnect();
    }

    public void SetYourRole(YourRoleResponse msg)
    {
        YourRole = msg;
        HasYourRole = true;
        TryFinishReconnect();
    }

    public void SetDayPlayersList(DayPlayersListResponse msg)
    {
        DayPlayersList = msg;
        HasDayPlayers = true;
        TryFinishReconnect();
    }

    public void SetVoteState(VoteStateUpdateResponse msg)
    {
        CurrentVoteState = msg;
        EventBus.RaiseVoteStateUpdated(msg);
    }
    public void SetNightAction(NightActionStartResponse msg)
    {
        NightActionStartResponse = msg;

        TryFinishReconnect();
    }
    public void SetDaySummaryResponse(DayEndSummaryResponse msg)
    {
        dayEndSummaryResponse = msg;
    }
    public void SetNightEndSummaryResponse(NightEndSummaryResponse msg)
    {
        nightEndSummaryResponse = msg;
    }
    public void SetVoteStateUpdateResponse(VoteStateUpdateResponse msg)
    {
        voteStateUpdateResponse = msg;
        EventBus.RaiseVoteStateUpdated(msg);
    }

    // ===================== RECONNECT ======================
    public bool IsReconnecting = false;
    public bool IsGameStateReady = false;

    public PhaseTimerResponse PhaseTimer;
        // ===== RECONNECT FLAGS =====
    public bool HasRoomInfo = false;
    public bool HasYourRole = false;
    public bool HasPhase = false ;
    public bool HasDayPlayers = false;

    public bool IsReconnectReady =>
        HasRoomInfo & HasYourRole & HasPhase & HasDayPlayers;
    public void SetRoomInfo(RoomInfoResponse msg)
    {
        CurrentRoom = msg.room;

        HasRoomInfo = true;
        TryFinishReconnect();
    }

    public void SetPhaseTimer(PhaseTimerResponse msg)
    {
        PhaseTimer = msg;
    }
    
    private bool reconnectSceneLoaded = false;

    private void TryFinishReconnect()
    {
        if (!IsReconnectReady)
            return;

        if (reconnectSceneLoaded)
            return;

        reconnectSceneLoaded = true;

        Debug.Log("✅ Reconnect data ready → loading GameScene");

        LoadingManager.Instance.LoadGameScene();
    }

}
