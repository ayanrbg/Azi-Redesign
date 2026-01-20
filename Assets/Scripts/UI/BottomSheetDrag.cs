using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class BottomSheetDrag : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform targetPanel;

    [Header("Positions")]
    public float openedY = 0f;
    public float closedY = -800f;

    [Header("Settings")]
    public float snapSpeed = 0.3f;

    public bool anchoredToBottom = true;
    [SerializeField] private bool onStartOpen;

    private float minY;
    private float maxY;

    void Start()
    {
        minY = Mathf.Min(openedY, closedY);
        maxY = Mathf.Max(openedY, closedY);

        if (onStartOpen)
        {
            SetY(closedY);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        targetPanel.DOKill();
    }

    public void OnDrag(PointerEventData eventData)
    {
        float deltaY = anchoredToBottom
            ? eventData.delta.y
            : -eventData.delta.y;

        float newY = targetPanel.anchoredPosition.y + deltaY;
        newY = Mathf.Clamp(newY, minY, maxY);

        SetY(newY);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float currentY = targetPanel.anchoredPosition.y;
        float middleY = (openedY + closedY) * 0.5f;

        if (currentY > middleY)
            Open();
        else
            Close();
    }

    public void Open()
    {
        targetPanel
            .DOAnchorPosY(openedY, snapSpeed)
            .SetEase(Ease.OutQuad);
    }

    public void Close()
    {
        targetPanel
            .DOAnchorPosY(closedY, snapSpeed)
            .SetEase(Ease.OutQuad);
    }

    private void SetY(float y)
    {
        targetPanel.anchoredPosition = new Vector2(
            targetPanel.anchoredPosition.x,
            y
        );
    }
}
