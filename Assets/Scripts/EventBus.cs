using System;

public static class EventBus
{

    public static Action<UserData> OnProfileUpdated;
    public static void RaiseProfileUpdated(UserData profile)
        => OnProfileUpdated?.Invoke(profile);
    // LOGIN FAILED
    public static Action<LoginFailedResponse> OnLoginFailed;
    public static void RaiseLoginFailed(LoginFailedResponse msg)
        => OnLoginFailed?.Invoke(msg);
    // REGISTER FAILED
    public static Action<RegisterFailedResponse> OnRegisterFailed;
    public static void RaiseRegisterFailed(RegisterFailedResponse msg)
        => OnRegisterFailed?.Invoke(msg);

    // ??????? (??????)
    public static Action<RoomInfo[]> OnRoomsUpdated;
    public static void RaiseRoomsUpdated(RoomInfo[] rooms)
        => OnRoomsUpdated?.Invoke(rooms);


    // ?? ??????? ????? ? ???????
    public static Action<JoinRoomSuccessResponse> OnRoomJoined;
    public static void RaiseRoomJoined(JoinRoomSuccessResponse room)
        => OnRoomJoined?.Invoke(room);


    // ?????????? ??????? ? ???????
    public static Action<RoomUpdateResponse> OnRoomUpdated;
    public static void RaiseRoomUpdated(RoomUpdateResponse update)
        => OnRoomUpdated?.Invoke(update);


    // ???
    public static Action<ChatMessageResponse> OnChatMessage;
    public static void RaiseChatMessage(ChatMessageResponse msg)
        => OnChatMessage?.Invoke(msg);

    // ????-??????
    public static Action<AutoStartResponse> OnAutoTimer;
    public static void RaiseAutoTimer(AutoStartResponse msg)
        => OnAutoTimer?.Invoke(msg);
    public static Action<BaseMessage> OnCancelAutoTimer;
    public static void RaiseCancelTimer(BaseMessage msg)
        => OnCancelAutoTimer?.Invoke(msg);

    // ??????? ????
    public static Action<PhaseUpdateResponse> OnPhaseUpdated;
    public static void RaisePhaseUpdated(PhaseUpdateResponse msg)
        => OnPhaseUpdated?.Invoke(msg);

    // ?????????? ???????
    public static Action<PhaseTimerResponse> OnTimerPhaseUpdated;
    public static void RaiseTimerPhaseUpdated(PhaseTimerResponse msg)
        => OnTimerPhaseUpdated?.Invoke(msg);


    // ???? - ?????? ???????
    public static Action<DayPlayersListResponse> OnDayPlayersList;
    public static void RaiseDayPlayersList(DayPlayersListResponse msg)
        => OnDayPlayersList?.Invoke(msg);


    // ???? ????
    public static Action<YourRoleResponse> OnRoleReceived;
    public static void RaiseRoleReceived(YourRoleResponse msg)
        => OnRoleReceived?.Invoke(msg);


    // ?????? ????????
    public static Action<NightActionStartResponse> OnNightAction;
    public static void RaiseNightAction(NightActionStartResponse msg)
        => OnNightAction?.Invoke(msg);


    // ????????? ???
    public static Action<DayEndSummaryResponse> OnDayEnd;
    public static void RaiseDayEnd(DayEndSummaryResponse msg)
        => OnDayEnd?.Invoke(msg);


    // ????????? ????
    public static Action<NightEndSummaryResponse> OnNightEnd;
    public static void RaiseNightEnd(NightEndSummaryResponse msg)
        => OnNightEnd?.Invoke(msg);
    // SEARCH USERS
    public static Action<SearchUsersResult> OnSearchUsers;
    public static void RaiseUserSearchResult(SearchUsersResult msg)
        => OnSearchUsers?.Invoke(msg);
    // FRIENDS
    public static Action<FriendsListResponse> OnFriendsList;
    public static void RaiseFriendsList(FriendsListResponse msg)
        => OnFriendsList?.Invoke(msg);
    // PROFILE
    public static Action<UserStats> OnUserStats;
    public static void RaiseUserStats(UserStats msg)
        => OnUserStats?.Invoke(msg);

    // FRIEND REQUEST
    public static Action<FriendRequestsListResponse> OnFriendsRequestList;
    public static void RaiseFriendsRequests(FriendRequestsListResponse msg)
        => OnFriendsRequestList?.Invoke(msg);
    // GAME INVITE
    public static Action<GameInvite> OnGameInvite;
    public static void RaiseGameInvite(GameInvite msg)
        => OnGameInvite?.Invoke(msg);

    // AVATAR SHOP
    public static Action<AvatarShopResponse> OnAvatarShop;
    public static void RaiseAvatarShop(AvatarShopResponse msg)
        => OnAvatarShop?.Invoke(msg);

    // ?????????? ???????????
    public static Action<VoteStateUpdateResponse> OnVoteStateUpdated;
    public static void RaiseVoteStateUpdated(VoteStateUpdateResponse msg)
        => OnVoteStateUpdated?.Invoke(msg);

    // ????????? ????
    public static Action<GameOverResponse> OnGameOver;
    public static void RaiseGameOver(GameOverResponse msg)
        => OnGameOver?.Invoke(msg);
    // ????????? ????
    public static Action<RatingResultResponse> OnRatingReceive;
    public static void RaiseRatingResponse(RatingResultResponse msg)
        => OnRatingReceive?.Invoke(msg);

    //// ?????????
    //public static Action<ActiveRoomInfo> OnReconnectRoomInfo;
    //public static void RaiseReconnectReady(ActiveRoomInfo msg)
    //    => OnReconnectRoomInfo?.Invoke(msg);
}
