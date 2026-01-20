using UnityEngine;
using DG.Tweening;

public class MorePanelAnimator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform panel;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private GameObject overlay; // ⬅ ОВЕРЛЕЙ

    [Header("Animation Settings")]
    public float duration = 0.3f;
    public Ease ease = Ease.OutBack;

    private bool isOpen;
    private Vector2 hiddenPos;
    private Vector2 shownPos;
    private Tween currentTween;

    void Awake()
    {
        shownPos = panel.anchoredPosition;
        hiddenPos = shownPos + new Vector2(0, -40f);

        panel.anchoredPosition = hiddenPos;
        panel.localScale = Vector3.one * 0.9f;
        canvasGroup.alpha = 0f;

        overlay.SetActive(false); // ⬅
        gameObject.SetActive(false);
    }

    public void Toggle()
    {
        if (isOpen)
            Close();
        else
            Open();
    }

    public void Open()
    {
        if (isOpen) return;

        isOpen = true;
        gameObject.SetActive(true);
        overlay.SetActive(true); // ⬅ ВКЛЮЧАЕМ ОВЕРЛЕЙ

        currentTween?.Kill();

        currentTween = DOTween.Sequence()
            .Append(canvasGroup.DOFade(1f, duration))
            .Join(panel.DOAnchorPos(shownPos, duration))
            .Join(panel.DOScale(1f, duration).SetEase(ease));
    }

    public void Close()
    {
        if (!isOpen) return;

        isOpen = false;
        currentTween?.Kill();

        currentTween = DOTween.Sequence()
            .Append(canvasGroup.DOFade(0f, duration))
            .Join(panel.DOAnchorPos(hiddenPos, duration))
            .Join(panel.DOScale(0.9f, duration))
            .OnComplete(() =>
            {
                overlay.SetActive(false); // ⬅ ВЫКЛЮЧАЕМ ОВЕРЛЕЙ
                gameObject.SetActive(false);
            });
    }
}
