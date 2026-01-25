using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerSlot : MonoBehaviour
{
    public string playerId;
    [SerializeField] private GameObject[] cards = new GameObject[3];
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI balanceText;
    [SerializeField] private Image avatarImage;
    public RectTransform avatarRect;
    [SerializeField] private Image cardImage;
    [SerializeField] private TimerUI timer; 
    [SerializeField] private Image winImage;
    public RectTransform cardPos;

    [Header("Status Icons")]
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private GameObject blackStatusIcon;
    [SerializeField] private GameObject greenStatusIcon;
    [SerializeField] private GameObject blueStatusIcon;
    [SerializeField] private GameObject redStatusIcon;
    [Header("Bet Panel")]
    [SerializeField] private TextMeshProUGUI betValueText;
    [SerializeField] private GameObject betPanel;
    [Header("Card Fly Animation")]
    [SerializeField] private float flyDuration = 0.6f;
    [SerializeField] private Ease flyEase = Ease.OutCubic;

    private bool hasCardOnTable = false;
    private string currentCardCode;

    private Vector2 cardTargetAnchoredPos;
    private Quaternion cardTargetRotation;

    public void Init(string id,string username, 
        //int avatar_id,
        int balance, bool ready)
    {
        playerId = id;
        nameText.text = username;
        //avatarImage.sprite = AvatarManager.Instance.avatarSprites152x152[avatar_id];
        balanceText.text = MoneyFormatter.Format(balance);
        if(ready && !GameManager.Instance.isGameStarted) SetReadyPlayer();
    }
    public void StartTimer(int seconds)
    {
        timer.fillImage.gameObject.SetActive(true);
        timer.StartReverseTimer(seconds);
    }
    public void StopTimer()
    {
        timer.StopTimer();
        timer.fillImage.gameObject.SetActive(false);
    }
    public void ShowPlayedCard(CardDTO card, System.Action onComplete = null)
    {
        betPanel.SetActive(false);

        if (hasCardOnTable && currentCardCode == card.code)
        {
            onComplete?.Invoke();
            return;
        }

        if (hasCardOnTable && currentCardCode != card.code)
        {
            SetCardInstant(card);
            onComplete?.Invoke();
            return;
        }

        FlyCardFromAvatar(card, onComplete);
    }
    public void SnapCardToHome()
    {
        if (cardImage == null || cardPos == null)
            return;

        RectTransform rect = cardImage.rectTransform;

        rect.DOKill();

        rect.anchoredPosition = cardPos.anchoredPosition;
        rect.rotation = cardPos.rotation;
        rect.localScale = Vector3.one;

        // 🔴 ВАЖНО
        hasCardOnTable = false;
        currentCardCode = null;

        cardImage.gameObject.SetActive(false);
    }

    public void CollectCardTo(
    RectTransform target,
    float duration,
    System.Action onComplete = null
)
    {
        if (!cardImage.gameObject.activeSelf || target == null)
        {
            onComplete?.Invoke();
            return;
        }

        RectTransform rect = cardImage.rectTransform;
        rect.DOKill();

        Vector2 targetPos = WorldToAnchoredPosition(
            rect.parent as RectTransform,
            target.position
        );

        rect.DOAnchorPos(targetPos, duration)
            .SetEase(Ease.InCubic)
            .OnComplete(() =>
            {
                cardImage.gameObject.SetActive(false);
                onComplete?.Invoke();
            });
    }


    private void SetCardInstant(CardDTO card)
    {
        cardImage.sprite = GameManager.Instance
            .gameUIController.cardSpritesDatabase.GetSprite(card.code);

        cardImage.gameObject.SetActive(true);

        hasCardOnTable = true;
        currentCardCode = card.code;
    }

    public void ClearCardImage()
    {
        cardImage.gameObject.SetActive(false);
        hasCardOnTable = false;
        currentCardCode = null;
    }
    private void SetReadyPlayer()
    {
        greenStatusIcon.SetActive(true);
        statusText.text = "готов";
    }
    public void ClearPlayerState()
    {
        statusText.text = "";
        blackStatusIcon.SetActive(true);
        greenStatusIcon.SetActive(false);
        blueStatusIcon.SetActive(false);
        redStatusIcon.SetActive(false);
    }
    public void ResetPlayedCard()
    {
        hasCardOnTable = false;
        currentCardCode = null;
        cardImage.gameObject.SetActive(false);
    }

    public void ShowWinAnimation()
    {
        if (winImage == null)
            return;

        RectTransform rect = winImage.rectTransform;

        // На всякий случай убиваем старые анимации
        rect.DOKill();

        winImage.gameObject.SetActive(true);

        rect.localScale = Vector3.one;

        float totalDuration = 2f;
        int pulses = 3;

        float singlePulseDuration = totalDuration / pulses / 2f;
        float scaleUpValue = 1.25f;

        Sequence seq = DOTween.Sequence();

        for (int i = 0; i < pulses; i++)
        {
            seq.Append(
                rect.DOScale(scaleUpValue, singlePulseDuration)
                    .SetEase(Ease.OutCubic)
            );

            seq.Append(
                rect.DOScale(1f, singlePulseDuration)
                    .SetEase(Ease.InCubic)
            );
        }

        seq.OnComplete(() =>
        {
            rect.localScale = Vector3.one;
            winImage.gameObject.SetActive(false);
        });
    }
    public Image GetPlayedCardImage()
    {
        return cardImage != null && cardImage.gameObject.activeSelf
            ? cardImage
            : null;
    }

    //public void ShowPlayedCard(CardDTO playCardRequest)
    //{
    //    cardImage.gameObject.SetActive(false);
    //    cardImage.sprite = GameManager.Instance.gameUIController.
    //        cardSpritesDatabase.GetSprite(playCardRequest.code);

    //    StartCoroutine(ShowCardRoutine());
    //    GameManager.Instance.gameUIController.cardToCenterAnimator.Animate(
    //        cards[cards.Length].gameObject.GetComponent<RectTransform>());
    //}
    //IEnumerator ShowCardRoutine()
    //{
    //    yield return new WaitForSeconds(0.6f);
    //    cardImage.gameObject.SetActive(true);
    //    //анимация появления карты
    //}
    private void SetActiveCards(int count)
    {
        foreach (GameObject card in cards)
        {
            card.SetActive(false);
        }
        for (int i = 0; i < count; i++)
        {
            cards[i].SetActive(true);
        }
    }
    public void FlyCardFromAvatar(CardDTO card, System.Action onComplete = null)
    {
        RectTransform cardRect = cardImage.rectTransform;
        RectTransform avatarRect = avatarImage.rectTransform;

        cardTargetAnchoredPos = cardPos.anchoredPosition;
        cardTargetRotation = cardPos.rotation;

        cardImage.sprite = GameManager.Instance
            .gameUIController.cardSpritesDatabase.GetSprite(card.code);

        cardImage.gameObject.SetActive(true);

        Vector2 avatarLocalPos = WorldToAnchoredPosition(
            cardRect.parent as RectTransform,
            avatarRect.position
        );

        cardRect.anchoredPosition = avatarLocalPos;
        cardRect.rotation = Quaternion.identity;
        cardRect.localScale = Vector3.one;

        cardRect.DOKill();

        Sequence seq = DOTween.Sequence();

        seq.Join(
            cardRect.DOAnchorPos(cardTargetAnchoredPos, flyDuration)
                .SetEase(flyEase)
        );

        seq.Join(
            cardRect.DORotateQuaternion(cardTargetRotation, flyDuration)
                .SetEase(flyEase)
        );

        seq.OnComplete(() =>
        {
            hasCardOnTable = true;
            currentCardCode = card.code;
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

}
