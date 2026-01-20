using UnityEngine;
using System.Text;

public class WebSocketManager : MonoBehaviour
{
    public static WebSocketManager Instance;

    private NativeWebSocket.WebSocket ws;

    private string userToken => GameState.Instance.UserToken;
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

    async void Start()
    {
        Connect();
    }
    async public void Connect()
    {
        ws = new NativeWebSocket.WebSocket("ws://89.35.125.173:8080");

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
        PlayerPrefs.DeleteKey("login");
        PlayerPrefs.DeleteKey("password");
        PlayerPrefs.Save();

        // 3. Сбрасываем GameState
        GameState.Instance.ResetState();

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

    public void SendRegister(string u, string p, string pp)
    {
        Send(new RegisterRequest { type = "register", username = u, password = p, password_confirm =  pp});
        PlayerPrefs.SetString("login", u);
        PlayerPrefs.SetString("password", p);
        PlayerPrefs.Save();
    }

    public void SendLogin(string u, string p)
    {
        Send(new LoginRequest { type = "login", username = u, password = p });
        PlayerPrefs.SetString("login", u);
        PlayerPrefs.SetString("password", p);
        PlayerPrefs.Save();
    }

    public void SendAuth(string token)
    {
        Send(new AuthRequest { type = "auth", token = userToken });
    }

    public void SendGetRooms()
    {
        Send(new GetRoomsRequest { type = "get_rooms", token = userToken });
    }

    public void SendCreateRoom(CreateRoomRequest req)
    {
        req.type = "create_room";
        req.token = userToken;
        Send(req);
    }
    public void SendGetRating()
    {
        RatingRequest req = new RatingRequest();
        req.type = "get_rating";
        req.limit = 25;
        req.token = userToken;
        Send(req);
    }
    public void SendGetFriends()
    {
        Send(new GetFriendRequestsResponse { type = "get_friends", token = userToken });
    }
    public void SendGetFriendRequests()
    {
        Send(new GetFriendRequestsResponse { type = "get_friend_requests", token = userToken });
    }
    public void SendGetAvatarShop()
    {
        Send(new AvatarShopRequest { type = "get_avatar_shop", token = userToken });
    }
    public void SendChangeAvatar(int avatarId)
    {
        Send(new ChangeAvatarRequest { type = "change_avatar", avatar_id = avatarId, token = userToken });
    }
    public void SendGetUserStats(int userId)
    {
        Send(new GetUserStatsResponse { type = "get_user_stats", user_id = userId,token = userToken });
    }
    public void SendFriendRequest(int userIdToSend)
    {
        Send(new SendFriendRequest { type = "send_friend_request", to_user_id = userIdToSend, token = userToken });
    }
    public void SendInviteGameRequest(int userIdToSend)
    {
        Send(new SendGameInviteRequest { type = "invite_to_game", friend_id = userIdToSend, token = userToken });
    }
    public void SendSearchUsers(string word)
    {
        Send(new SearchUsersRequest { type = "search_users", query = word, token = userToken });
    }
    public void SendAcceptRequest(int requestId)
    {
        Send(new RespondFriendRequestResponse { type = "respond_friend_request", action = "accept",
            request_id = requestId, token = userToken });
    }
    public void SendDeclineRequest(int requestId)
    {
        Send(new RespondFriendRequestResponse
        {
            type = "respond_friend_request",
            action = "decline",
            request_id = requestId,
            token = userToken
        });
    }
    public void SendJoinRoom(int roomId, string password)
    {
        Send(new JoinRoomRequest
        {
            type = "join_room",
            token = userToken,
            roomId = roomId,
            password = password
        });
    }

    public void SendLeaveRoom()
    {
        Send(new LeaveRoomRequest { type = "leave_room", token = userToken });
    }

    public void SendStartGame()
    {
        Send(new StartGameRequest { type = "start_game", token = userToken });
    }

    public void SendChat(string text)
    {
        Send(new SendChatRequest { type = "send_chat", token = userToken, text = text });
    }

    public void SendDayVote(int id)
    {
        Send(new DayVoteRequest { type = "day_vote", token = userToken, targetId = id });
        
    }

    public void SendNightAction(int id)
    {
        Send(new NightActionRequest { type = "night_action", token = userToken, targetId = id });
        
    }

    async void Send(object obj)
    {
        if (ws.State != NativeWebSocket.WebSocketState.Open) return;
        string json = JsonUtility.ToJson(obj);
        await ws.SendText(json);
    }

    // ================= RECEIVE =================

    void HandleMessage(string json)
    {
        BaseMessage baseMsg = JsonUtility.FromJson<BaseMessage>(json);
       
        switch (baseMsg.type)
        {
            case "login_success":
                HandleLoginSuccess(json);
                break;
            case "register_success":
                HandleRegisterSuccess(json);
                break;
            case "auth_success":
                HandleAuthSuccess(json);
                break;
            case "login_failed":
                HandleLoginFailed(json);
                break;
            case "register_failed":
                HandleRegisterFailed(json);
                break;

            case "get_rooms_success":
                HandleGetRooms(json);
                break;

            case "create_room_success":
                HandleJoinRoom(json);
                break;

            case "join_room_success":
                HandleJoinRoom(json);
                break;

            case "room_update":
                HandleRoomUpdate(json);
                break;

            case "chat_message":
                HandleChat(json);
                break;
            case "auto_start_timer":
                HandleAutoStartTimer(json);
                break;
            case "auto_start_cancelled":
                HandleCancelTimer(json);
                break;
            case "day_players_list":
                HandleDayPlayers(json);
                break;

            case "phase_update":
                HandlePhase(json);
                break;

            case "your_role":
                HandleYourRole(json);
                break;

            case "night_action_start":
                HandleNightAction(json);
                break;
            case "vote_state_update":
                HandleVoteState(json);
                break;
            case "day_end_summary":
                HandleDayEnd(json);
                break;

            case "night_end_summary":
                HandleNightEnd(json);
                break;
            case "game_over":
                HandleGameOver(json);
                break;
            case "room_info":
                HandleReconnectingToGame(json);
                break;
            case "phase_timer":
                HandlePhaseTimerUpdate(json);
                break;
            case "rating_result":
                HandleRatingResult(json);
                break;
            case "friends_list":
                HandleFriendsList(json);
                break;
            case "search_users_result":
                HandleSearchUsersResult(json);
                break;
            case "friend_requests_list":
                HandleFriendRequests(json);
                break;
            case "game_invite":
                HandleGameInvite(json);
                break;
            case "user_stats":
                HandleUserStats(json);
                break;
            case "avatar_shop":
                HandleAvatarShop(json);  
                break;
        }
    }

    // ================= HANDLERS =================
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
        GameState.Instance.SetRooms(msg.rooms);
    }

    void HandleJoinRoom(string json)
    {
        var msg = JsonUtility.FromJson<JoinRoomSuccessResponse>(json);
        GameState.Instance.SetCurrentRoom(msg);
        
        LoadingManager.Instance.LoadGameScene();
    }
    #endregion
    #region Game
    void HandleRoomUpdate(string json)
    {
        var msg = JsonUtility.FromJson<RoomUpdateResponse>(json);

        GameState.Instance.ApplyRoomUpdate(msg);
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
}
