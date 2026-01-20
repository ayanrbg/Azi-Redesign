using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting.Antlr3.Runtime;

public class RulesPanelController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject rulesPanel;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform panel;
    [SerializeField] private MorePanelAnimator morePanel = null;

    [Header("Animation")]
    [SerializeField] private float duration = 0.3f;
    [SerializeField] private Ease ease = Ease.OutCubic;

    private Tween tween;
    private Vector2 hiddenPos;
    private Vector2 shownPos;
    private Tween currentTween;
    private bool isOpen;

    private void Awake()
    {
        // ❗ панель может быть выключена
        //rulesPanel.SetActive(true);

        //shownPos = panel.anchoredPosition;
        //hiddenPos = shownPos + new Vector2(0, -500f);

        //panel.anchoredPosition = hiddenPos;
        //panel.localScale = Vector3.one * 0.9f;

        //canvasGroup.alpha = 0f;
        //canvasGroup.blocksRaycasts = false;
        //canvasGroup.interactable = false;

        rulesPanel.SetActive(false);
    }
    public void Open()
    {
        if (isOpen) return;
        isOpen = true;

        rulesPanel.SetActive(true);
        //canvasGroup.blocksRaycasts = true;
        //canvasGroup.interactable = true;

        //currentTween?.Kill();

        //currentTween = DOTween.Sequence()
        //    .Append(canvasGroup.DOFade(1f, 0.2f))
        //    .Append(panel.DOAnchorPos(shownPos, duration))
        //    .Join(panel.DOScale(1f, duration).SetEase(ease));

    }
    public void OpenForGameScene()
    {
        morePanel.Close();
        Open();
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
        //        rulesPanel.SetActive(false);
        //    });
        rulesPanel.SetActive(false);
    }
}
