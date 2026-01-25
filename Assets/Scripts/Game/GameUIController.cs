using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Audio.ProcessorInstance;

public class GameUIController : MonoBehaviour
{
    private readonly List<CardVisualState> cachedCardStates = new();

    public bool isFirstDiscard = true;
    public bool isRequestMove = false;
    public CardSpritesDatabase cardSpritesDatabase;
    [Header("Animators")]
    public CardToCenterAnimator cardToCenterAnimator;
    [Header("Panels")]
    [SerializeField] GameObject readyButton;
    public GameObject timerPanel;
    public GameObject betButtons;
    [SerializeField] TextMeshProUGUI betText;
    public GameObject requestPlayDecisionButtons;
    [SerializeField] GameObject potPanel;
    [SerializeField] RectTransform potPanelRect;
    [SerializeField] TextMeshProUGUI potText;
    [SerializeField] Image trumpCardImage;
    [Header("Players")]
    public MainPlayerSlot mainPlayerSlot;
    [SerializeField] PlayerSlot[] playerSlots;
    public Dictionary<string, PlayerSlot> slotsById;
    public TimerUI mainTimer;

    private void RebuildSlotsDictionary()
    {
        slotsById = new Dictionary<string, PlayerSlot>();

        foreach (var slot in playerSlots)
        {
            if (string.IsNullOrEmpty(slot.playerId))
                continue;

            if (!slotsById.ContainsKey(slot.playerId))
                slotsById.Add(slot.playerId, slot);
        }
    }

    public void LoadJoinRoom(JoinedRoomResponse joinedRoomResponse)
    {
        //gameUIController.coinsText.text = GameState.Instance.userProfile.COINS.ToString();
        
        LoadJoinedRoomPlayers(joinedRoomResponse);
    }
   
    public void ShowPot(int value)
    {
        potPanel.SetActive(true);
        potText.text = MoneyFormatter.Format(value).ToString();
    }
    //public void UpdatePlayerCards(CurrentTrickPlayDTO[] currentTrick)
    //{
    //    foreach(CurrentTrickPlayDTO trick in currentTrick)
    //    {
    //        GetSlot(trick.playerId).ShowPlayedCard(trick.card);
    //    }
    //}
    public void ClearPlayersStates()
    {
        foreach (PlayerSlot playerSlot in playerSlots)
        {
            playerSlot.ClearPlayerState();
        }
    }
    public PlayerSlot GetSlot(string id)
    {
        if (slotsById.TryGetValue(id, out var slot))
            return slot;

        Debug.LogWarning($"PlayerSlot with id {id} not found");
        return null;
    }
    public void LoadJoinedRoomPlayers(JoinedRoomResponse joinedRoomResponse)
    {
        foreach (PlayerSlot playerSlot in playerSlots) 
        { 
            playerSlot.gameObject.SetActive(false);
        }
        int i = 0;
        foreach (RoomPlayer player in joinedRoomResponse.room.players) 
        {
            if(player.id == GameState.Instance.userProfile.id)
            {
                mainPlayerSlot.LoadMainPlayer(player);
                i++;
                return;
            }
            playerSlots[i].gameObject.SetActive(true);
            playerSlots[i].Init(player.id,player.name, player.balance, player.ready);
            i++;
        }
        RebuildSlotsDictionary();
    }
    public void LoadRoomUpdatePlayers(RoomUpdateResponse roomUpdateResponse)
    {
        foreach (PlayerSlot playerSlot in playerSlots)
        {
            playerSlot.gameObject.SetActive(false);
        }
        int i = 0;
        foreach (RoomPlayer player in roomUpdateResponse.room.players)
        {
            if (player.id == GameState.Instance.userProfile.id)
            {
                mainPlayerSlot.LoadMainPlayer(player);
                continue;
            }
            playerSlots[i].gameObject.SetActive(true);
            playerSlots[i].Init(player.id, player.name, player.balance, player.ready);
            i++;
        }
        RebuildSlotsDictionary();
    }
    public void LoadRequestPlayDecision(RequestPlayDecisionResponse response)
    {
        GameManager.Instance.isGameStarted = true;
        readyButton.SetActive(false);
        requestPlayDecisionButtons.SetActive(true);
        potPanel.SetActive(true);
        potText.text = response.pot.ToString();
        trumpCardImage.gameObject.SetActive(true);
        trumpCardImage.sprite = cardSpritesDatabase.GetSprite(response.trumpCard.code);
        if (isFirstDiscard)
        {
            StartCoroutine(mainPlayerSlot.LoadCards(response.yourCards));
            isFirstDiscard = false;
        }
    }
    public void LoadGameBidding(RequestBidResponse response)
    {
        betButtons.SetActive(true);
        betText.text = MoneyFormatter.Format(response.baseBet).ToString();
        timerPanel.SetActive(true);
        mainTimer.StartTimer(15f);
    }
    public void LoadTimerCurrentPlayer(GameUpdateResponse response)
    {
        foreach (var player in playerSlots) 
        {
            player.StopTimer();
        }
        if (string.IsNullOrEmpty(response.currentPlayer))
            return;
        GetSlot(response.currentPlayer).StartTimer(15);
    }
    public void LoadRequestMove(RequestMoveResponse response)
    {
        isRequestMove = true;
        foreach (var player in playerSlots)
        {
            player.StopTimer();
        }
        GameManager.Instance.handView.RecalculateCardIndex(response.yourCards);
        SetValidCards(response.validCards);
        ShowCurrentTricks(response.currentTrick);
    }
    public void ShowGameWinner(GameWinnerResponse response)
    {
        isFirstDiscard = true;
        isRequestMove = false;
        GameManager.Instance.isRequestDiscard = false;
        GameManager.Instance.isFirstGameUpdate = true;
        GameManager.Instance.isGameStarted = false;
        if(response.winner == mainPlayerSlot.id)
        {
            AnimateMoveAndReturn(potPanelRect, mainPlayerSlot.cardSpawnPoint);
            return;
        }
        AnimateMoveAndReturn(potPanelRect, GetSlot(response.winner).avatarRect);
    }
    private void SetValidCards(int[] validIndexes)
    {
        foreach (var card in GameManager.Instance.handView.cards)
        {
            card.SetBlocked(false);
        }
        foreach (var card in GameManager.Instance.handView.cards)
        {
            bool isValid = validIndexes.Contains(card.cardOrderId);
            card.SetBlocked(!isValid);
        }
    }
    public void ShowPlayedCards(CardPlayedResponse response)
    {
        ShowCurrentTricks(response.currentTrick);
        UnableValidCards();
    }
    public void ShowTrickComplete(TrickCompleteResponse response)
    {
        // 1️⃣ ждём пока карты ДОЛЕТЯТ до cardPos
        ShowCurrentTricks(response.allCards, () =>
        {
            // 2️⃣ небольшая пауза ПОСЛЕ выброса
            StartCoroutine(DelayBeforeCollect(response.winner));
        });
    }
    private IEnumerator DelayBeforeCollect(
        string winnerPlayerId,
        float delay = 0.25f
    )
    {
        // ⏱ пауза ПОСЛЕ выкладывания карт
        yield return new WaitForSeconds(delay);

        // 3️⃣ сбор карт к победителю
        CollectCardsToWinner(winnerPlayerId, () =>
        {
            // 4️⃣ карты ВСЕГДА выключаем и возвращаем
            foreach (var slot in playerSlots)
            {
                slot.SnapCardToHome();
                slot.ClearCardImage();
            }

            mainPlayerSlot.SnapCardToHome();
            mainPlayerSlot.ClearPlayedCard();

            // 5️⃣ win-анимация
            if (winnerPlayerId == mainPlayerSlot.id)
                mainPlayerSlot.ShowWinAnimation();
            else
                GetSlot(winnerPlayerId)?.ShowWinAnimation();
        });
    }

    private IEnumerator PlayWinAnimationWithDelay(
        string winnerPlayerId,
        float delay = 2.3f
    )
    {
        yield return new WaitForSeconds(delay);

        if (winnerPlayerId == mainPlayerSlot.id)
            mainPlayerSlot.ShowWinAnimation();
        else
            GetSlot(winnerPlayerId)?.ShowWinAnimation();
    }

    public void UpdateCurrentPot(float potValue)
    {
        potPanel.SetActive(true);
        potText.text = MoneyFormatter.Format(potValue).ToString();
        LayoutRebuilder.MarkLayoutForRebuild(potPanelRect);
    }
    public void ShowCurrentTricks(
    CurrentTrickPlayDTO[] tricks,
    System.Action onAllCardsPlaced = null
)
    {
        if (tricks == null || tricks.Length == 0)
        {
            onAllCardsPlaced?.Invoke();
            return;
        }

        int completed = 0;
        int total = tricks.Length;

        void OnOneCardPlaced()
        {
            completed++;
            if (completed == total)
                onAllCardsPlaced?.Invoke();
        }

        foreach (var trick in tricks)
        {
            if (trick.playerId == mainPlayerSlot.id)
            {
                mainPlayerSlot.AnimatePlayedCard(
                    cardSpritesDatabase.GetSprite(trick.card.code),
                    OnOneCardPlaced
                );
            }
            else
            {
                GetSlot(trick.playerId)
                    ?.ShowPlayedCard(trick.card, OnOneCardPlaced);
            }
        }
    }

    public void UnableValidCards()
    {
        foreach (var card in GameManager.Instance.handView.cards)
        {
            card.SetBlocked(false);
        }
    }

    public void CollectCardsToWinner(
    string winnerPlayerId,
    System.Action onComplete,
    float duration = 0.6f
)
    {
        RectTransform target =
            winnerPlayerId == mainPlayerSlot.id
                ? mainPlayerSlot.cardSpawnPoint
                : GetSlot(winnerPlayerId)?.avatarRect;

        if (target == null)
        {
            onComplete?.Invoke();
            return;
        }

        int activeTweens = 0;

        void OneDone()
        {
            activeTweens--;
            if (activeTweens == 0)
                onComplete?.Invoke();
        }

        // 🔹 ВСЕ PlayerSlot, ВКЛЮЧАЯ ПОБЕДИТЕЛЯ
        foreach (var slot in playerSlots)
        {
            var img = slot.GetPlayedCardImage();
            if (img == null)
                continue;

            activeTweens++;
            slot.CollectCardTo(target, duration, OneDone);
        }

        // 🔹 MainPlayerSlot ВСЕГДА
        var mainImg = mainPlayerSlot.GetPlayedCardImage();
        if (mainImg != null)
        {
            activeTweens++;
            mainPlayerSlot.CollectCardTo(target, duration, OneDone);
        }

        // 🛡 если вообще не было карт
        if (activeTweens == 0)
            onComplete?.Invoke();
    }


    private IEnumerator RestoreCardsAfterCollect()
    {
        yield return null; // обязательно один кадр

        foreach (var slot in playerSlots)
        {
            slot.SnapCardToHome();
            slot.ClearCardImage(); // ❗ выключаем
        }

        mainPlayerSlot.SnapCardToHome();
        mainPlayerSlot.ClearPlayedCard(); // ❗ выключаем
    }


    private IEnumerator RestoreCardsNextFrame()
    {
        yield return new WaitForSeconds(0.01f);

        foreach (var slot in playerSlots)
            slot.SnapCardToHome();

        mainPlayerSlot.SnapCardToHome();
    }
    public void AnimateMoveAndReturn(
    RectTransform moving,
    RectTransform target,
    float duration = 0.6f,
    System.Action onComplete = null
)
    {
        if (moving == null || target == null)
        {
            onComplete?.Invoke();
            return;
        }

        RectTransform parent = moving.parent as RectTransform;
        if (parent == null)
        {
            onComplete?.Invoke();
            return;
        }

        // 🔐 сохраняем ИСХОДНОЕ состояние
        Vector2 startPos = moving.anchoredPosition;
        Quaternion startRot = moving.rotation;
        Vector3 startScale = moving.localScale;

        moving.gameObject.SetActive(true);
        moving.DOKill();

        // 🎯 вычисляем позицию target В КООРДИНАТАХ parent
        Vector2 targetPos = WorldToAnchoredPosition(parent, target.position);

        moving
            .DOAnchorPos(targetPos, duration)
            .SetEase(Ease.InOutCubic)
            .OnComplete(() =>
            {
                // ⏮ вернуть обратно МГНОВЕННО
                moving.DOKill();
                moving.anchoredPosition = startPos;
                moving.rotation = startRot;
                moving.localScale = startScale;

                // ❌ выключаем объект
                moving.gameObject.SetActive(false);

                onComplete?.Invoke();
            });
    }


    private Vector2 WorldToAnchoredPosition(
        RectTransform parent,
        Vector3 worldPos
    )
    {
        Vector2 screenPoint =
            RectTransformUtility.WorldToScreenPoint(null, worldPos);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parent,
            screenPoint,
            null,
            out Vector2 localPoint
        );

        return localPoint;
    }

    public void CloseBetButtons()
    {
        betButtons.SetActive(false);
    }
    public void OpenBetButtons()
    {
        betButtons.SetActive(true);
    }
    private class CardVisualState
    {
        public RectTransform rect;
        public Vector2 anchoredPos;
        public Quaternion rotation;
        public Vector3 scale;
    }
}
