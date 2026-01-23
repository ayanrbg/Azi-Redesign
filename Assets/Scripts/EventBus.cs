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

}
