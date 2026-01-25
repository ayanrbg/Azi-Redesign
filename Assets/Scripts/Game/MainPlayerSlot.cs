using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainPlayerSlot : MonoBehaviour
{
    public string id;
    [SerializeField] private Image yourCardImage;
    public RectTransform cardPos;

    public HandView handView;
    [Header("Money")]
    [SerializeField] private TextMeshProUGUI balanceText;
    [SerializeField] private TextMeshProUGUI coinsText;
    public RectTransform cardSpawnPoint;

    private bool hasCardOnTable = false;
    private Sprite currentCardSprite;


    public void Awake()
    {
        id = GameState.Instance.userProfile.id;
    }
    public void SetPlayedCard(Sprite cardSprite)
    {
        yourCardImage.sprite = cardSprite;  
    }
    public void LoadMainPlayer(RoomPlayer playerData)
    {
        balanceText.text = MoneyFormatter.Format(playerData.balance).ToString();
    }
    public void SnapCardToHome()
    {
        if (yourCardImage == null || cardPos == null)
            return;

        RectTransform rect = yourCardImage.rectTransform;

        rect.DOKill();

        rect.anchoredPosition = cardPos.anchoredPosition;
        rect.rotation = cardPos.rotation;
        rect.localScale = Vector3.one;

        hasCardOnTable = false;
        currentCardSprite = null;

        yourCardImage.gameObject.SetActive(false);
    }

    public void CollectCardTo(
    RectTransform target,
    float duration,
    System.Action onComplete = null
)
    {
        if (!yourCardImage.gameObject.activeSelf || target == null)
        {
            onComplete?.Invoke();
            return;
        }

        RectTransform rect = yourCardImage.rectTransform;
        rect.DOKill();

        Vector2 targetPos = WorldToAnchoredPosition(
            rect.parent as RectTransform,
            target.position
        );

        rect.DOAnchorPos(targetPos, duration)
            .SetEase(Ease.InCubic)
            .OnComplete(() =>
            {
                yourCardImage.gameObject.SetActive(false);
                onComplete?.Invoke();
            });
    }


    public void AnimatePlayedCard(
    Sprite cardSprite,
    System.Action onComplete = null,
    float duration = 0.6f
)
    {
        if (yourCardImage == null || cardSpawnPoint == null || cardSprite == null)
        {
            onComplete?.Invoke();
            return;
        }

        RectTransform cardRect = yourCardImage.rectTransform;
        RectTransform parentRect = cardRect.parent as RectTransform;

        Vector2 targetPos = cardPos.anchoredPosition;
        Quaternion targetRot = cardPos.rotation;

        yourCardImage.sprite = cardSprite;
        yourCardImage.gameObject.SetActive(true);

        cardRect.DOKill();

        Vector2 startPos = WorldToAnchoredPosition(
            parentRect,
            cardSpawnPoint.position
        );

        cardRect.anchoredPosition = startPos;
        cardRect.rotation = Quaternion.identity;
        cardRect.localScale = Vector3.one;

        Sequence seq = DOTween.Sequence();

        seq.Join(cardRect.DOAnchorPos(targetPos, duration).SetEase(Ease.OutCubic));
        seq.Join(cardRect.DORotateQuaternion(targetRot, duration).SetEase(Ease.OutCubic));

        seq.OnComplete(() =>
        {
            onComplete?.Invoke();
        });
    }

    private void SetCardInstant(Sprite cardSprite)
    {
        yourCardImage.sprite = cardSprite;
        yourCardImage.gameObject.SetActive(true);

        hasCardOnTable = true;
        currentCardSprite = cardSprite;
    }
    public void ClearPlayedCard()
    {
        yourCardImage.gameObject.SetActive(false);
        hasCardOnTable = false;
        currentCardSprite = null;
    }
    public Image GetPlayedCardImage()
    {
        return yourCardImage != null && yourCardImage.gameObject.activeSelf
            ? yourCardImage
            : null;
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
    public void ShowWinAnimation()
    {
        if (yourCardImage == null || !yourCardImage.gameObject.activeSelf)
            return;

        RectTransform rect = yourCardImage.rectTransform;

        rect.DOKill();

        Sequence seq = DOTween.Sequence();

        seq.Append(rect.DOScale(1.15f, 0.15f));
        seq.Append(rect.DOScale(1f, 0.15f));
        seq.Append(rect.DOScale(1.15f, 0.15f));
        seq.Append(rect.DOScale(1f, 0.15f));
    }

    public IEnumerator LoadCards(CardDTO[] serverCards)
    {
        // 1. Полностью очистить руку
        yield return StartCoroutine(handView.ClearHand());

        // 2. Добавить карты с сервера
        for (int i = 0; i < serverCards.Length; i++)
        {
            yield return StartCoroutine(
                handView.AddCard(i.ToString(), serverCards[i].code)
            );
        }
    }
}
