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
    #region Rooms
    public void SendJoinRoom(string id, string pass)
    {
        Send(new JoinRoomRequest { type = "joinRoom", data = new JoinRoomData{ roomId = id, password = pass} });
    }
    public void SendGetRooms()
    {
        Send(new GetRoomsRequest { type = "getRooms"});
    }
    public void SendCreateRoom(string roomName, string pass, int max_players)
    {
        Send(new CreateRoomRequest { type = "createRoom", data = 
            new CreateRoomData {name = roomName, password = pass, bet = 300, icon = "H", maxPlayers = max_players} });
    }
    #endregion
    #region Game
    public void SendLeaveRoom()
    {
        Send(new LeaveRoomRequest { type = "leaveRoom" });
    }
    public void SendReady()
    {
        Send(new ReadyRequest { type = "ready" });
    }
    public void SendDecidePlay(bool value)
    {
        Send(new DecidePlayingRequest { type = "decidePlaying", data = new DecidePlayingData { play = value} });
    }
    public void SendDiscardCard(int cardId)
    {
        Send(new DiscardCardRequest { type = "discardCard", data = new DiscardCardData { cardIndex = cardId } });
    }
    public void SendBet()
    {
        Send(new BidActionRequest
        {
            type = "bidAction",
            data = new BidActionData
            {
                action = "raise",
                amount = null
            }
        });
    }
    public void SendPlayCard(int index)
    {
        Send(new PlayCardRequest
        { type = "playCard", data = new PlayCardData 
        {
            cardIndex = index,
        }
        });
    }
    public void SendPass()
    {
        Send(new BidActionRequest
        {
            type = "bidAction",
            data = new BidActionData
            {
                action = "pass",
                amount = null
            }
        });
    }
    #endregion
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
            case "error":
                HandleError(json);
                break;
            case "authResult":
                HandleAuthResult(json);
                break;
            case "roomsList":
                HandleRoomsList(json);
                break;
            case "joinedRoom":
                HandleJoinedRoom(json);
                break;
            case "roomUpdate":
                HandleRoomUpdate(json);
                break;
            case "requestPlayDecision":
                HandleRequestPlayDecision(json);
                break;
            case "requestDiscard":
                HandleRequestDiscard(json);
                break;
            case "gameUpdate":
                HandleGameUpdate(json);
                break;
            case "requestBid":
                HandleRequestBid(json);
                break;
            case "requestMove":
                HandleRequestMove(json);
                break;
            case "cardPlayed":
                HandleCardPlayed(json);
                break;
            case "trickComplete":
                HandleTrickComplete(json);
                break;
            case "gameWinner":
                HandleGameWinner(json);
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
    #region GAME
    void HandleRoomUpdate(string json)
    {
        var msg = JsonUtility.FromJson<RoomUpdateResponse>(json);
        EventBus.RaiseRoomUpdate(msg);
    }
    void HandleRequestPlayDecision(string json)
    {
        var msg = JsonUtility.FromJson<RequestPlayDecisionResponse>(json);
        EventBus.RaiseRequestPlayDecision(msg);
    }
    void HandleRequestDiscard(string json)
    {
        var msg = JsonUtility.FromJson<RequestDiscardResponse>(json);
        EventBus.RaiseRequestDiscard(msg);
    }
    void HandleGameUpdate(string json)
    {
        var msg = JsonUtility.FromJson<GameUpdateResponse>(json);
        switch (msg.phase)
        {
            case "discarding":
                EventBus.RaiseGameUpdateDiscarding(msg);
                break;
            case "bidding":
                EventBus.RaiseGameUpdateBidding(msg);
                break;
            case "playing":
                EventBus.RaiseGameUpdatePlaying(msg);
                break;
        }
    }
    void HandleRequestBid(string json)
    {
        var msg = JsonUtility.FromJson<RequestBidResponse>(json);
        EventBus.RaiseRequestBid(msg);
    }
    void HandleRequestMove(string json)
    {
        var msg = JsonUtility.FromJson<RequestMoveResponse>(json);
        EventBus.RaiseRequestMove(msg);
    }
    void HandleCardPlayed(string json)
    {
        var msg = JsonUtility.FromJson<CardPlayedResponse>(json);
        EventBus.RaiseCardPlayed(msg);
    }
    void HandleTrickComplete(string json)
    {
        var msg = JsonUtility.FromJson<TrickCompleteResponse>(json);
        EventBus.RaiseTrickComplete(msg);
    }
    void HandleGameWinner(string json)
    {
        var msg = JsonUtility.FromJson<GameWinnerResponse>(json);
        EventBus.RaiseGameWinner(msg);
    }
    #endregion

}
