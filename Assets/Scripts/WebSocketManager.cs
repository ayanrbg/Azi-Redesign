using UnityEngine;
using System.Text;

public class WebSocketManager : MonoBehaviour
{
    public static WebSocketManager Instance;

    private NativeWebSocket.WebSocket ws;

    //private string userToken => GameState.Instance.UserToken;
    public System.Action OnConnected;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
    public async void ConnectWithAuth()
    {
        ws = new NativeWebSocket.WebSocket("ws://92.53.124.96:51801");

        ws.OnOpen += () =>
        {
            Debug.Log("WS Connected");
            SendAuth(PlayerPrefs.GetString("token"));
        };

        ws.OnMessage += (bytes) =>
        {
            string json = Encoding.UTF8.GetString(bytes);
            HandleMessage(json);
        };

        ws.OnClose += (code) => Debug.Log("WS Closed: " + code);
        ws.OnError += (err) => Debug.LogError("WS Error: " + err);

        await ws.Connect();
    }
    public void SendAuth(string token)
    {
        Send(new AuthRequest
        {
            type = "auth",
            token = token
        });
    }
    async public void Connect()
    {
        ws = new NativeWebSocket.WebSocket("ws://92.53.124.96:51801");

        ws.OnOpen += () =>
        {
            Debug.Log("WS Connected");
            OnConnected?.Invoke();
        };

        ws.OnMessage += (bytes) =>
        {
            string json = Encoding.UTF8.GetString(bytes);
            HandleMessage(json);
            
        };

        ws.OnClose += (code) => Debug.Log("WS Closed: " + code);
        ws.OnError += (err) => Debug.LogError("WS Error: " + err);

        await ws.Connect();
    }

    public async void Logout()
    {
        Debug.Log("LOGOUT START");

        // 1. Закрываем соединение
        if (ws != null)
        {
            if (ws.State == NativeWebSocket.WebSocketState.Open)
                await ws.Close();

            ws = null;
        }

        // 2. Чистим PlayerPrefs

        // 3. Сбрасываем GameState
        //GameState.Instance.ResetState();

        // 4. Переходим на Auth
        LoadingManager.Instance.LoadAuthScene();

        // 5. Переподключаем сокет (уже без токена)
        Connect();
    }
    
    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        ws?.DispatchMessageQueue();
#endif
    }

    async void OnApplicationQuit()
    {
        await ws.Close();
    }

    // ================= SEND =================

    public void SendJoinRoom(string id, string pass)
    {
        Send(new JoinRoomRequest { type = "joinRoom", data = new JoinRoomData{ roomId = id, password = pass} });
    }
    public void SendGetRooms()
    {
        Send(new GetRoomsRequest { type = "getRooms"});
    }
    async void Send(object obj)
    {
        if (ws.State != NativeWebSocket.WebSocketState.Open) return;
        string json = JsonUtility.ToJson(obj);
        await ws.SendText(json);
        Debug.Log(json);
    }

    // ================= RECEIVE =================

    void HandleMessage(string json)
    {
        Debug.Log(json);
        var baseMsg = JsonUtility.FromJson<WsMessage>(json);
        
        switch (baseMsg.type)
        {
            //case "authFailed":
            //    HandleAuthResult(json);
            //    break;
            case "authResult":
                HandleAuthResult(json);
                break;
            case "roomsList":
                HandleRoomsList(json);
                break;
            case "joinedRoom":
                HandleJoinedRoom(json);
                break;
            case "error":
                HandleError(json);
                break;
        }
    }

    // ================= HANDLERS =================
    #region Error
    void HandleError(string json)
    {
        var msg = JsonUtility.FromJson<ErrorResponse>(json);
        EventBus.RaiseError(msg);
    }
    #endregion
    #region AUTH
    void HandleAuthResult(string json)
    {
        var msg = JsonUtility.FromJson<AuthResultResponse>(json);
        AuthManager.Instance.HandleAuthResult(msg);
        GameState.Instance.SetProfile(msg);
        EventBus.RaiseAuthResult(msg);
    }
    #endregion
    #region Rooms
    void HandleRoomsList(string json)
    {
        var msg = JsonUtility.FromJson<RoomsListResponse>(json);
        EventBus.RaiseRoomsList(msg);
    }
    void HandleJoinedRoom(string json)
    {
        var msg = JsonUtility.FromJson<JoinedRoomResponse>(json);
        GameState.Instance.SetJoinedRoom(msg);
        EventBus.RaiseJoinedRoom(msg);
    }
    #endregion

    /*
    #region Auth
    void HandleLoginSuccess(string json)
    {
        var msg = JsonUtility.FromJson<LoginSuccessResponse>(json);
        GameState.Instance.UpdateToken(msg.token);
        SendAuth(userToken);
    }
    void HandleRegisterSuccess(string json)
    {
        var msg = JsonUtility.FromJson<LoginSuccessResponse>(json);
        GameState.Instance.UpdateToken(msg.token);
        SendAuth(userToken);

    }
    void HandleAuthSuccess(string json)
    {
        var msg = JsonUtility.FromJson<AuthSuccessResponse>(json);

        GameState.Instance.SetPlayerProfile(msg);
        EventBus.RaiseProfileUpdated(msg.userData);
        LoadingManager.Instance.LoadMainScene();

        
    }
    void HandleLoginFailed(string json)
    {
        var msg = JsonUtility.FromJson<LoginFailedResponse>(json);
        EventBus.RaiseLoginFailed(msg);
    }
    void HandleRegisterFailed(string json)
    {
        var msg = JsonUtility.FromJson<RegisterFailedResponse>(json);
        EventBus.RaiseRegisterFailed(msg);
    }
    #endregion
    #region RoomLobby
    void HandleGetRooms(string json)
    {
        var msg = JsonUtility.FromJson<GetRoomsSuccessResponse>(json);
        EventBus.RaiseRoomsUpdated(msg.rooms);
        //GameState.Instance.SetRooms(msg.rooms);
    }

    void HandleJoinRoom(string json)
    {
        var msg = JsonUtility.FromJson<JoinRoomSuccessResponse>(json);
        //GameState.Instance.SetCurrentRoom(msg);
        
        LoadingManager.Instance.LoadGameScene();
    }
    #endregion
    #region Game
    void HandleRoomUpdate(string json)
    {
        var msg = JsonUtility.FromJson<RoomUpdateResponse>(json);

        //GameState.Instance.ApplyRoomUpdate(msg);
    }
    void HandleChat(string json)
    {
        var msg = JsonUtility.FromJson<ChatMessageResponse>(json);
        EventBus.RaiseChatMessage(msg);
    }
    void HandleGameOver(string json)
    {
        var msg = JsonUtility.FromJson<GameOverResponse>(json);
        EventBus.RaiseGameOver(msg);
    }
    void HandleAutoStartTimer(string json)
    {
        var msg = JsonUtility.FromJson<AutoStartResponse>(json);
        EventBus.RaiseAutoTimer(msg);
    }
    void HandleCancelTimer(string json)
    {
        var msg = JsonUtility.FromJson<BaseMessage>(json);
        EventBus.RaiseCancelTimer(msg);
    }
    void HandleDayPlayers(string json)
    {
        var msg = JsonUtility.FromJson<DayPlayersListResponse>(json);
        GameState.Instance.SetDayPlayersList(msg);
        EventBus.RaiseDayPlayersList(msg);
    }
    void HandleFriendRequests(string json)
    {
        var msg = JsonUtility.FromJson<FriendRequestsListResponse>(json);
        GameState.Instance.SetFriendsRequests(msg);
        EventBus.RaiseFriendsRequests(msg);
    }
    
    void HandlePhase(string json)
    {
        var msg = JsonUtility.FromJson<PhaseUpdateResponse>(json);
        GameState.Instance.SetPhase(msg);
        EventBus.RaisePhaseUpdated(msg);
    }
    void HandleYourRole(string json)
    {
        var msg = JsonUtility.FromJson<YourRoleResponse>(json);
        GameState.Instance.SetYourRole(msg);
        EventBus.RaiseRoleReceived(msg);
    }

    void HandleNightAction(string json)
    {
        var msg = JsonUtility.FromJson<NightActionStartResponse>(json);
        GameState.Instance.SetNightAction(msg);
        EventBus.RaiseNightAction(msg);
    }

    void HandleDayEnd(string json)
    {
        var msg = JsonUtility.FromJson<DayEndSummaryResponse>(json);
        GameState.Instance.SetDaySummaryResponse(msg);
        EventBus.RaiseDayEnd(msg);
    }
    void HandleFriendsList(string json)
    {
        var msg = JsonUtility.FromJson<FriendsListResponse>(json);
        GameState.Instance.SetCurrentFriends(msg);
        EventBus.RaiseFriendsList(msg);
    }
    void HandleSearchUsersResult(string json)
    {
        var msg = JsonUtility.FromJson<SearchUsersResult>(json);
        EventBus.RaiseUserSearchResult(msg);
    }
    void HandleNightEnd(string json)
    {
        var msg = JsonUtility.FromJson<NightEndSummaryResponse>(json);
        GameState.Instance.SetNightEndSummaryResponse(msg);
        EventBus.RaiseNightEnd(msg);
    }
    void HandleVoteState(string json)
    {
        var msg = JsonUtility.FromJson<VoteStateUpdateResponse>(json);
        GameState.Instance.SetVoteStateUpdateResponse(msg);
    }
    void HandleReconnectingToGame(string json){
        var msg = JsonUtility.FromJson<RoomInfoResponse>(json);
        GameState.Instance.SetRoomInfo(msg);
    }
    void HandlePhaseTimerUpdate(string json)
    {
        var msg = JsonUtility.FromJson<PhaseTimerResponse>(json);
        EventBus.RaiseTimerPhaseUpdated(msg);
    }
    void HandleRatingResult(string json)
    {
        var msg = JsonUtility.FromJson<RatingResultResponse>(json);
        EventBus.RaiseRatingResponse(msg);
    }
    void HandleGameInvite(string json)
    {
        var msg = JsonUtility.FromJson<GameInvite>(json);
        EventBus.RaiseGameInvite(msg);
    }
    void HandleUserStats(string json)
    {
        var msg = JsonUtility.FromJson<UserStats>(json);
        GameState.Instance.SetProfileStats(msg);
        EventBus.RaiseUserStats(msg);
    }
    void HandleAvatarShop(string json)
    {
        var msg = JsonUtility.FromJson<AvatarShopResponse>(json);
        EventBus.RaiseAvatarShop(msg);
    }
    #endregion
    */
}
