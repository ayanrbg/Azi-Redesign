using System;
using System.Collections.Generic;
using static UnityEngine.Audio.IAudioGenerator;

// ================= BASE =================

[Serializable]
public class BaseMessage
{
    public string type;
}

// ================= CLIENT -> SERVER =================

[Serializable]
public class RegisterRequest : BaseMessage
{
    public string username;
    public string password;
    public string password_confirm;
}

[Serializable]
public class LoginRequest : BaseMessage
{
    public string username;
    public string password;
}

[Serializable]
public class AuthRequest : BaseMessage
{
    public string token;
}
[Serializable]
public class LoginFailedResponse : BaseMessage
{
    public string message;
}
[Serializable]
public class RegisterFailedResponse : BaseMessage
{
    public string message;
}

[Serializable]
public class GetRoomsRequest : BaseMessage
{
    public string token;
}

[Serializable]
public class CreateRoomRequest : BaseMessage
{
    public string token;
    public string name;
    public int min_players;
    public int max_players;
    public int level;
    public int mafia_count;
    public string[] roles;
}

[Serializable]
public class JoinRoomRequest : BaseMessage
{
    public string token;
    public int roomId;
    public string password;
}

[Serializable]
public class LeaveRoomRequest : BaseMessage
{
    public string token;
}

[Serializable]
public class StartGameRequest : BaseMessage
{
    public string token;
}

[Serializable]
public class SendChatRequest : BaseMessage
{
    public string token;
    public string text;
}

[Serializable]
public class DayVoteRequest : BaseMessage
{
    public string token;
    public int targetId;
}

[Serializable]
public class NightActionRequest : BaseMessage
{
    public string token;
    public int targetId;
}

// ================= SERVER -> CLIENT =================

[Serializable]
public class LoginSuccessResponse : BaseMessage
{
    public string token;
}

[Serializable]
public class AuthSuccessResponse : BaseMessage
{
    public int userId;
    public UserData userData;

}

[Serializable]
public class UserData
{
    public string username;
    public int balance;
    public int experience;
    public int level;
    public int avatar_id;
}

// ---------- ROOMS ----------

[Serializable]
public class GetRoomsSuccessResponse : BaseMessage
{
    public RoomInfo[] rooms;
}

[Serializable]
public class CreateRoomSuccessResponse : BaseMessage { }

[Serializable]
public class JoinRoomSuccessResponse : BaseMessage
{
    public RoomFullInfo room;
}

[Serializable]
public class RoomUpdateResponse : BaseMessage
{
    public RoomPlayer[] players;
    public int playerCount;
    public RoomPlayer player_enter;
    public RoomPlayer player_left;
    public int maxPlayerCount;
}

// ---------- CHAT ----------

[Serializable]
public class ChatMessageResponse : BaseMessage
{
    public int user_id;
    public string username;
    public int avatar_id;
    public string text;
    public string time;
}

// ---------- GAME ----------
[Serializable]
public class AutoStartResponse : BaseMessage
{
    public int seconds;
}

[Serializable]
public class DayPlayersListResponse : BaseMessage
{
    public int day;
    public VotePlayer[] players;
    public DayStats stats;
}

[Serializable]
public class PhaseUpdateResponse : BaseMessage
{
    public string phase;
    public int duration;
}

[Serializable]
public class YourRoleResponse : BaseMessage
{
    public string role;
    public int[] mafiaList;
}

[Serializable]
public class NightActionStartResponse : BaseMessage
{
    public string role;
    public int duration;
    public VotePlayer[] players;
}

[Serializable]
public class DayEndSummaryResponse : BaseMessage
{
    public VotePair[] votes;
    public KilledPlayer killed;
}
[Serializable]
public class VotePair
{
    public VoteUser from;
    public VoteUser to;
}
[Serializable]
public class VoteUser
{
    public int user_id;
    public string username;
    public int avatar_id;
}


[Serializable]
public class NightEndSummaryResponse : BaseMessage
{
    public KilledPlayer[] deaths;
    public KilledPlayer healed;
    public KilledPlayer blocked;
}

[Serializable]
public class VoteStateUpdateResponse : BaseMessage
{
    public string voteType;
    public VoteResultPlayer[] players;
}
[Serializable]
public class GameOverResponse : BaseMessage
{
    public string winner;
    public GameResultPlayer[] players;
}
// ---------- PHASE TIMER ----------

[Serializable]
public class PhaseTimerResponse : BaseMessage
{
    public string phase;
    public int seconds_left;
}

// ---------- ROOM INFO (RECONNECT) ----------

[Serializable]
public class RoomInfoResponse : BaseMessage
{
    public RoomFullInfo room;
}


// ================= MODELS =================
[Serializable]
public class RoomInviteInfo
{
    public int id;
    public string name;
    public int level;
    public string[] roles;
}
[Serializable]
public class RoomInfo
{
    public int id;
    public string name;
    public int level;
    public int min_players;
    public int max_players;
    public string[] roles;
    public int current_players;
    public bool hasPassword;
    public bool game_started;
}

[Serializable]
public class RoomFullInfo
{
    public int id;
    public string name;
    public string password;
    public int min_players;
    public int max_players;
    public int level;
    public string[] roles;
    public int created_by;
    public string created_at;
    public string phase;
    public string phase_end_time;
    public bool game_started;
    public int alive_count;
    public int mafia_count;
    public int playerCount;
    public RoomPlayer[] players;
}

[Serializable]
public class RoomPlayer
{
    public int id;
    public string username;
    public int avatar_id;
}

[Serializable]
public class VotePlayer
{
    public int user_id;
    public string username;
    public int avatar_id;
    public bool is_mafia;
}

[Serializable]
public class VoteResultPlayer
{
    public int user_id;
    public string username;
    public int avatar_id;
    public int votes;
    public bool is_mafia;
}
[Serializable]
public class GameResultPlayer
{
    public int user_id;
    public string username;
    public int avatar_id;
    public string role;
    public bool is_alive;
}

[Serializable]
public class KilledPlayer
{
    public int user_id;
    public string username;
    public int avatar_id;
    public string role;
}

[Serializable]
public class DayStats
{
    public int alive_peaceful;
    public int dead_peaceful;
    public int alive_mafia;
    public int dead_mafia;
}
// ---------- PROFILE ----------
[Serializable]
public class GetUserStatsResponse : BaseMessage
{
    public int user_id;
    public string token;
}
[Serializable]
public class UserStats : BaseMessage 
{
    public int user_id;
    public string username;
    public int avatar_id;
    public int level;
    public ProfileStats stats;
}
[Serializable]
public class ProfileStats
{
    public int games_played;
    public int mafia_games;
    public int mafia_wins;
    public int peaceful_games;
    public int peaceful_wins;
}
// ---------- AVATAR SHOP ----------
[Serializable]
public class AvatarShopRequest : BaseMessage
{
    public string token;
}
[Serializable]
public class ChangeAvatarRequest : BaseMessage
{
    public string token;
    public int avatar_id;
}
// ---------- RATING ----------
[Serializable]
public class RatingRequest : BaseMessage
{
    public int limit;
    public string token;
}
[Serializable]
public class RatingResultResponse : BaseMessage
{
    public RatingPlayer[] top;
    public RatingPlayer me;
}

[Serializable]
public class RatingPlayer
{
    public string place;
    public int user_id;
    public string username;
    public int avatar_id;
    public int experience;
}
// ---------- FRIEND REQUEST SENT ----------

[Serializable]
public class FriendRequestSentResponse : BaseMessage
{
    public int to_user_id;
}
// ---------- FRIEND REQUESTS LIST ----------

[Serializable]
public class FriendRequestsListResponse : BaseMessage
{
    public FriendRequest[] requests;
}
[Serializable]
public class GetFriendRequestsResponse : BaseMessage
{
    public string token;
}
[Serializable]
public class FriendRequest
{
    public int id;
    public int user_id;
    public string username;
    public int avatar_id;
    public string created_at;
    public int level;
}
[Serializable]
public class RespondFriendRequestResponse : BaseMessage
{
    public string token;
    public int request_id;
    public string action;
}
// ---------- FRIEND ADDED ----------

[Serializable]
public class FriendAddedResponse : BaseMessage
{
    public AddFriendUser user;
}

[Serializable]
public class AddFriendUser
{
    public int user_id;
    public string username;
    public int avatar_id;
}
[Serializable]
public class FriendsListResponse : BaseMessage
{
    public FriendUser[] friends;
}
[Serializable]
public class FriendUser
{
    public int user_id;
    public string username;
    public int avatar_id;
    public bool is_online;
    public int level;
}
[Serializable]
public class SendFriendRequest : BaseMessage
{
    public int to_user_id;
    public string token;
}
[Serializable]
public class SearchUsersResult : BaseMessage
{
    public SearchUser[] users;
}
[Serializable]
public class SearchUsersRequest : BaseMessage
{
    public string query;
    public string token;
}
[Serializable]
public class SearchUser
{
    public int user_id;
    public string username;
    public int avatar_id;
    public int level;
    public bool isFriend;
    public bool is_online;
}
// ---------- GAME INVITE ----------

[Serializable]
public class GameInvite : BaseMessage
{
    public RoomPlayer from;
    public RoomInviteInfo room;
}
[Serializable]
public class SendGameInviteRequest : BaseMessage
{
    public int friend_id;
    public string token;
}
