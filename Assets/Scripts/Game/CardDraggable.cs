using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class CardDraggable : MonoBehaviour,
    IPointerClickHandler,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{
    public int cardOrderId;
    [Header("View")]
    [SerializeField] private Image cardImage;

    [Header("Settings")]
    [SerializeField] private float dragScale = 1.1f;
    [SerializeField] private float returnDuration = 0.25f;
    public float yToDrag = 160f;
    private bool isBlocked;
    private CanvasGroup canvasGroup;

    private RectTransform rectTransform;
    private Canvas canvas;

    private Vector2 startAnchoredPos;
    private Quaternion startRotation;
    private Transform originalParent;
    private int originalSiblingIndex;

    public string cardCode;

    private bool isDragging;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // Инициализация карты
    public void Init(int orderId ,string code)
    {
        cardOrderId = orderId;
        cardCode = code;
        Debug.Log(cardCode);
        cardImage.sprite = GameManager.Instance.gameUIController.cardSpritesDatabase.GetSprite(cardCode);
    }
    public void ChangeIndex(int index)
    {
        cardOrderId = index;
    }
    // ---------------------------
    // DRAG
    // ---------------------------
    public void SetBlocked(bool blocked, float fadeDuration = 0.15f)
    {
        isBlocked = blocked;

        // Блокируем ввод
        canvasGroup.interactable = !blocked;
        canvasGroup.blocksRaycasts = !blocked;

        // Визуальное затемнение
        float targetAlpha = blocked ? 0.5f : 1f;

        canvasGroup.DOKill();
        canvasGroup.DOFade(targetAlpha, fadeDuration);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isBlocked)
            return;
        isDragging = true;

        rectTransform.DOKill();

        startAnchoredPos = rectTransform.anchoredPosition;
        startRotation = rectTransform.rotation;

        originalParent = rectTransform.parent;
        originalSiblingIndex = rectTransform.GetSiblingIndex();

        // Поверх всех UI
        rectTransform.SetAsLastSibling();

        // Визуальный эффект
        rectTransform.DOScale(dragScale, 0.15f);
        rectTransform.DORotateQuaternion(Quaternion.identity, 0.15f);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isBlocked)
            return;
        if (!isDragging) return;

        // Правильный drag для UI
        rectTransform.anchoredPosition +=
            eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isBlocked)
            return;
        isDragging = false;

        bool canPlay =
        rectTransform.anchoredPosition.y > yToDrag;

        if (canPlay && GameManager.Instance.gameUIController.isRequestMove)
        {
            Debug.Log("Карта сыграна = " + cardCode);
            WebSocketManager.Instance.SendPlayCard(cardOrderId); //костыль
            GameManager.Instance.handView.RemoveCardAt(cardOrderId);
        }
        else
        {
            ReturnToHand();
        }
    }

    // ---------------------------
    // RETURN
    // ---------------------------

    private void ReturnToHand()
    {
        rectTransform.SetParent(originalParent);
        rectTransform.SetSiblingIndex(originalSiblingIndex);

        rectTransform.DOAnchorPos(startAnchoredPos, returnDuration)
            .SetEase(Ease.OutCubic);

        rectTransform.DORotateQuaternion(startRotation, returnDuration);
        rectTransform.DOScale(1f, returnDuration);
    }

    // ---------------------------
    // CLICK
    // ---------------------------

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isBlocked)
            return;
        GameManager.Instance.handView.SelectCard(this);
        GameManager.Instance.SendDiscardCard(cardOrderId);
    }
}
