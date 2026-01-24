using System;

public static class EventBus
{
    //ERROR
    public static Action<ErrorResponse> OnError;
    public static void RaiseError(ErrorResponse msg)
        => OnError?.Invoke(msg);
    #region AUTH
    //AUTH RESULT 
    public static Action<AuthResultResponse> OnAuthResult;
    public static void RaiseAuthResult(AuthResultResponse msg)
        => OnAuthResult?.Invoke(msg);
   
    //AUTH RESULT 
    public static Action<RoomsListResponse> OnRoomsList;
    public static void RaiseRoomsList(RoomsListResponse msg)
        => OnRoomsList?.Invoke(msg);
    #endregion
    #region Rooms
    //JOIN ROOM
    public static Action<JoinedRoomResponse> OnJoinedRoom;
    public static void RaiseJoinedRoom(JoinedRoomResponse msg)
        => OnJoinedRoom?.Invoke(msg);
    #endregion
    #region GAME
    //ROOM UPDATE
    public static Action<RoomUpdateResponse> OnRoomUpdate;
    public static void RaiseRoomUpdate(RoomUpdateResponse msg)
        => OnRoomUpdate?.Invoke(msg);
    //REQUEST PLAY DECISION
    public static Action<RequestPlayDecisionResponse> OnRequestDecision;
    public static void RaiseRequestPlayDecision(RequestPlayDecisionResponse msg)
        => OnRequestDecision?.Invoke(msg);
    //REQUEST DISCARD
    public static Action<RequestDiscardResponse> OnRequestDiscard;
    public static void RaiseRequestDiscard(RequestDiscardResponse msg)
        => OnRequestDiscard?.Invoke(msg);

    //GAME UPDATE DISCARDING
    public static Action<GameUpdateResponse> OnGameUpdateDiscarding;
    public static void RaiseGameUpdateDiscarding(GameUpdateResponse msg)
        => OnGameUpdateDiscarding?.Invoke(msg);

    //GAME UPDATE BIDDING
    public static Action<GameUpdateResponse> OnGameUpdateBidding;
    public static void RaiseGameUpdateBidding(GameUpdateResponse msg)
        => OnGameUpdateBidding?.Invoke(msg);
    //BID REQUEST 
    public static Action<RequestBidResponse> OnRequestBid;
    public static void RaiseRequestBid(RequestBidResponse msg)
        => OnRequestBid?.Invoke(msg);
    #endregion
}
