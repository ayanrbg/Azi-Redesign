using TMPro;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance { get; private set; }

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
    #endregion
    
    public GameUIController gameUIController;
    //[SerializeField] private RectTransform card1;
    public HandView handView;
    public bool isRequestDiscard = false;
    public bool isFirstGameUpdate = true;
    public bool isGameStarted = false;

    //public void PlayAnimationCard()
    //{
    //    card1.anchoredPosition = new Vector2(-800, 0);

    //    card1.DOAnchorPos(Vector2.zero, 0.6f)
    //        .SetEase(Ease.OutBack);
    //}
    private void OnEnable()
    {
        gameUIController.LoadJoinRoom(GameState.Instance.joinedRoomData);
        EventBus.OnJoinedRoom += gameUIController.LoadJoinRoom;
        EventBus.OnRoomUpdate += gameUIController.LoadRoomUpdatePlayers;
        EventBus.OnRequestDecision += gameUIController.LoadRequestPlayDecision;
        EventBus.OnRequestDiscard += LoadRequestDiscardCard;
        EventBus.OnGameUpdateDiscarding += LoadGameUpdateDiscarding;
        EventBus.OnGameUpdateBidding += LoadGameUpdateBidding;
        EventBus.OnRequestBid += gameUIController.LoadGameBidding;
        EventBus.OnRequestMove += gameUIController.LoadRequestMove;
        EventBus.OnGameUpdatePlaying += LoadGameUpdatePlaying;
        EventBus.OnCardPlayed += gameUIController.ShowPlayedCards;
        EventBus.OnTrickComplete += gameUIController.ShowTrickComplete;
        EventBus.OnGameWinner += gameUIController.ShowGameWinner;
        //EventBus.OnPlayedCard += gameUIController.ShowPlayedCard;
    }
    private void OnDisable()
    {
        EventBus.OnJoinedRoom -= gameUIController.LoadJoinRoom;
        EventBus.OnRoomUpdate -= gameUIController.LoadRoomUpdatePlayers;
        EventBus.OnRequestDecision -= gameUIController.LoadRequestPlayDecision;
        EventBus.OnRequestDiscard -= LoadRequestDiscardCard;
        EventBus.OnGameUpdateDiscarding -= LoadGameUpdateDiscarding;
        EventBus.OnGameUpdateDiscarding -= LoadGameUpdateBidding;
        EventBus.OnRequestBid -= gameUIController.LoadGameBidding;
        EventBus.OnRequestMove -= gameUIController.LoadRequestMove;
        EventBus.OnGameUpdatePlaying -= LoadGameUpdatePlaying;
        EventBus.OnCardPlayed -= gameUIController.ShowPlayedCards;
        EventBus.OnTrickComplete -= gameUIController.ShowTrickComplete;
        EventBus.OnGameWinner -= gameUIController.ShowGameWinner;
        //EventBus.OnPlayedCard += gameUIController.ShowPlayedCard;
    }
    
    public void SendReady()
    {
        WebSocketManager.Instance.SendReady();
    }
    public void SendPlayDecision(bool value)
    {
        WebSocketManager.Instance.SendDecidePlay(value);
    }
    public void SendLeaveRoom()
    {
        WebSocketManager.Instance.SendLeaveRoom();
        LoadingManager.Instance.LoadMainScene();
    }
    private void LoadRequestDiscardCard(RequestDiscardResponse response)
    {
        isRequestDiscard = true; //*
        gameUIController.requestPlayDecisionButtons.SetActive(false);
        gameUIController.ClearPlayersStates();
    }
    private void LoadGameUpdateDiscarding(GameUpdateResponse response)
    {
        isRequestDiscard = false; //когда сделают бэк и ограничат вызовы - можно убрать а то UI старадет
        gameUIController.requestPlayDecisionButtons.SetActive(false);
        gameUIController.betButtons.SetActive(false);
        gameUIController.mainTimer.StopTimer();
        gameUIController.timerPanel.SetActive(false);
        gameUIController.LoadTimerCurrentPlayer(response);
        gameUIController.UnableValidCards();
        gameUIController.isRequestMove = false;
        if (isFirstGameUpdate)
        {
            StartCoroutine(gameUIController.mainPlayerSlot.LoadCards(response.yourCards));
        }
        isFirstGameUpdate = false;
    }
    private void LoadGameUpdateBidding(GameUpdateResponse response)
    {
        isRequestDiscard = false; //*
        gameUIController.requestPlayDecisionButtons.SetActive(false);
        gameUIController.betButtons.SetActive(false);
        gameUIController.mainTimer.StopTimer();
        gameUIController.timerPanel.SetActive(false);
        gameUIController.LoadTimerCurrentPlayer(response);
        gameUIController.UnableValidCards();
        gameUIController.isRequestMove = false;
        gameUIController.UpdateCurrentPot(response.pot);
        if (isFirstGameUpdate)
        {
            StartCoroutine(gameUIController.mainPlayerSlot.LoadCards(response.yourCards));
        }
        isFirstGameUpdate = false;

    }
    private void LoadGameUpdatePlaying(GameUpdateResponse response)
    {
        isRequestDiscard = false; //*
        gameUIController.mainTimer.StopTimer();
        gameUIController.timerPanel.SetActive(false);
        gameUIController.LoadTimerCurrentPlayer(response);
        gameUIController.UpdateCurrentPot(response.pot);
        gameUIController.isRequestMove = false;
        isFirstGameUpdate = false;
    }
    private void LoadGameEnd()
    {
        isRequestDiscard = false; //*
        isFirstGameUpdate = true;
    }
    
    public void Bet()
    {
        WebSocketManager.Instance.SendBet();
    }
    public void Pass()
    {
        WebSocketManager.Instance.SendPass();
    }
    public void SendDiscardCard(int cardId)
    {
        if (!isRequestDiscard) return;
        WebSocketManager.Instance.SendDiscardCard(cardId);
    }
}
