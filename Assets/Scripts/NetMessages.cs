using System;
using System.Collections.Generic;

#region BASE

[Serializable]
public class WsMessage
{
    public string type;
}

[Serializable]
public class WsMessage<T> : WsMessage
{
    public T data;
}

#endregion

// =====================================================
// CLIENT → SERVER
// =====================================================

#region AUTH

[Serializable]
public class AuthRequest : WsMessage
{
    public string token;
}
[Serializable]
public class AuthFailed : WsMessage
{
    public string message;
    public Error errors;
}
[Serializable]
public class Error
{
    public string[] email;
}


#endregion

#region ROOMS

// ---------- CREATE ROOM ----------

[Serializable]
public class CreateRoomData
{
    public string name;
    public int bet;
    public string password;
    public int maxPlayers;
    public string icon; // H, D, C, S
}

[Serializable]
public class CreateRoomRequest : WsMessage<CreateRoomData> { }

// ---------- GET ROOMS ----------

[Serializable]
public class GetRoomsRequest : WsMessage { }

// ---------- JOIN ROOM ----------

[Serializable]
public class JoinRoomData
{
    public string roomId;
    public string password;
}

[Serializable]
public class JoinRoomRequest : WsMessage
{
    public JoinRoomData data;

}

// ---------- LEAVE ROOM ----------

[Serializable]
public class LeaveRoomRequest : WsMessage { }

#endregion

#region GAME FLOW

// ---------- READY ----------

[Serializable]
public class ReadyRequest : WsMessage { }

// ---------- DECIDE PLAYING ----------

[Serializable]
public class DecidePlayingData
{
    public bool play;
}

[Serializable]
public class DecidePlayingRequest : WsMessage<DecidePlayingData> { }

// ---------- DISCARD CARD ----------

[Serializable]
public class DiscardCardData
{
    public int cardIndex;
}

[Serializable]
public class DiscardCardRequest : WsMessage<DiscardCardData> { }

// ---------- BID ACTION ----------

[Serializable]
public class BidActionData
{
    public string action; // raise | pass
}

[Serializable]
public class BidActionRequest : WsMessage<BidActionData> { }

// ---------- PLAY CARD ----------

[Serializable]
public class PlayCardData
{
    public int cardIndex;
}

[Serializable]
public class PlayCardRequest : WsMessage<PlayCardData> { }

// ---------- DECLARE RAZNOMAST ----------

[Serializable]
public class DeclareRaznomastRequest : WsMessage { }

// ---------- INVITE AZI ----------

[Serializable]
public class InviteAziData
{
    public int playerId;
}

[Serializable]
public class InviteAziRequest : WsMessage<InviteAziData> { }

#endregion

// =====================================================
// SERVER → CLIENT
// =====================================================

#region AUTH RESPONSE

[Serializable]
public class AuthResultResponse : WsMessage
{
    public bool success;
    public AuthUser user;
}

[Serializable]
public class AuthUser
{
    public int id;
    public string name;
    public int balance;
    public int coins;
}

#endregion

#region ROOMS RESPONSE

// ---------- ROOMS LIST ----------

[Serializable]
public class RoomsListResponse : WsMessage
{
    public RoomInfo[] rooms;
}

[Serializable]
public class RoomInfo
{
    public string id;
    public string name;
    public int bet;
    public bool hasPassword;
    public int players;
    public int maxPlayers;
    public string icon;
    public string status;
}

// ---------- ROOM CREATED ----------

[Serializable]
public class RoomCreatedResponse : WsMessage
{
    public int roomId;
}

// ---------- JOINED ROOM ----------

[Serializable]
public class JoinedRoomResponse : WsMessage
{
    public RoomState room;
}

[Serializable]
public class RoomState
{
    public int id;
    public string name;
    public int bet;
    public int maxPlayers;
    public string icon;
    public RoomPlayer[] players;
    public string status;
}

[Serializable]
public class RoomPlayer
{
    public int id;
    public string name;
    public bool ready;
    public int balance;
}

// ---------- ROOM UPDATE ----------

[Serializable]
public class RoomUpdateResponse : WsMessage
{
    public RoomState room;
}

#endregion

#region GAME RESPONSE

// ---------- CARDS DEALT ----------

[Serializable]
public class CardsDealtResponse : WsMessage
{
    public string[] cards;
    public string trump;
    public int dealer;
}

// ---------- REQUEST PLAY DECISION ----------

[Serializable]
public class RequestPlayDecisionResponse : WsMessage { }

// ---------- PLAYERS DECIDED ----------

[Serializable]
public class PlayersDecidedResponse : WsMessage
{
    public int[] playingPlayers;
}

// ---------- REQUEST DISCARD ----------

[Serializable]
public class RequestDiscardResponse : WsMessage { }

// ---------- BIDDING STARTED ----------

[Serializable]
public class BiddingStartedResponse : WsMessage
{
    public int currentBidder;
    public int pot;
    public int raiseCount;
}

// ---------- BID ACTION RESULT ----------

[Serializable]
public class BidActionResultResponse : WsMessage
{
    public int player;
    public string action;
    public int pot;
    public int raiseCount;
}

// ---------- BIDDING COMPLETE ----------

[Serializable]
public class BiddingCompleteResponse : WsMessage
{
    public int winner;
    public int pot;
}

// ---------- GAME STARTED ----------

[Serializable]
public class GameStartedResponse : WsMessage
{
    public int currentPlayer;
    public string trump;
}

// ---------- REQUEST MOVE ----------

[Serializable]
public class RequestMoveResponse : WsMessage
{
    public int[] validCards;
}

// ---------- CARD PLAYED ----------

[Serializable]
public class CardPlayedResponse : WsMessage
{
    public int player;
    public string card;
    public TrickCard[] trickCards;
}

[Serializable]
public class TrickCard
{
    public int player;
    public string card;
}

// ---------- TRICK COMPLETE ----------

[Serializable]
public class TrickCompleteResponse : WsMessage
{
    public int winner;
    public Dictionary<string, int> tricks;
}

// ---------- GAME COMPLETE ----------

[Serializable]
public class GameCompleteResponse : WsMessage
{
    public int winner;
    public int pot;
    public Dictionary<string, int> newBalances;
}

// ---------- AZI ----------

[Serializable]
public class AziResponse : WsMessage
{
    public int[] playersWithOneTrick;
    public string message;
}

// ---------- RAZNOMAST DECLARED ----------

[Serializable]
public class RaznomastDeclaredResponse : WsMessage
{
    public int player;
}

// ---------- AZI INVITE ----------

[Serializable]
public class AziInviteResponse : WsMessage
{
    public int from;
    public int to;
}
// ---------- ERROR ----------

[Serializable]
public class ErrorResponse : WsMessage
{
    public string message;
}


#endregion
