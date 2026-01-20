using DG.Tweening;
using UnityEngine;

public class SettingsController : MonoBehaviour
{
    [Header("Profile Panel Reference")]
    [SerializeField] private GameObject settingsPanel;      // ❗ выключена в сцене
    [SerializeField] private RectTransform panel;
    [SerializeField] private CanvasGroup canvasGroup;
    
    [Header("Animation")]
    [SerializeField] private float duration = 0.35f;
    [SerializeField] private Ease ease = Ease.OutBack;

    private Vector2 shownPos;
    private Vector2 hiddenPos;
    private Tween currentTween;
    private bool isOpen;

    private void Awake()
    {
        // ❗ панель может быть выключена
        //settingsPanel.SetActive(true);

        //shownPos = panel.anchoredPosition;
        //hiddenPos = shownPos + new Vector2(0, -500f);

        //panel.anchoredPosition = hiddenPos;
        //panel.localScale = Vector3.one * 0.9f;

        //canvasGroup.alpha = 0f;
        //canvasGroup.blocksRaycasts = false;
        //canvasGroup.interactable = false;

        settingsPanel.SetActive(false);
    }
    public void Open()
    {
        if (isOpen) return;
        isOpen = true;

        settingsPanel.SetActive(true);
        //canvasGroup.blocksRaycasts = true;
        //canvasGroup.interactable = true;

        //currentTween?.Kill();

        //currentTween = DOTween.Sequence()
        //    .Append(canvasGroup.DOFade(1f, 0.2f))
        //    .Append(panel.DOAnchorPos(shownPos, duration))
        //    .Join(panel.DOScale(1f, duration).SetEase(ease));
    }
    public void Close()
    {
        if (!isOpen) return;
        isOpen = false;

        //canvasGroup.blocksRaycasts = false;
        //canvasGroup.interactable = false;

        //currentTween?.Kill();

        //currentTween = DOTween.Sequence()
        //    .Append(panel.DOAnchorPos(hiddenPos, duration * 0.8f))
        //    .Join(panel.DOScale(0.9f, duration * 0.8f))
        //    .Append(canvasGroup.DOFade(0f, 0.2f))
        //    .OnComplete(() =>
        //    {
                settingsPanel.SetActive(false);
            //});
    }
}
