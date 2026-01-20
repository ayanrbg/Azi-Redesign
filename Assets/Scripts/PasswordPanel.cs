using UnityEngine;
using TMPro;
using DG.Tweening;

public class PasswordPanel : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform panel;
    [SerializeField] private TMP_InputField passwordInput;

    [Header("Animation")]
    [SerializeField] private float duration = 0.3f;
    [SerializeField] private Ease ease = Ease.OutCubic;

    private Vector2 shownPos;
    private Vector2 hiddenPos;
    private Tween tween;
    private bool isOpen;

    public int enterRoomId;

    private void OnEnable()
    {
        // Чтобы не мешала в Scene View
        if (!Application.isPlaying)
            return;

        shownPos = panel.anchoredPosition;
        hiddenPos = shownPos + new Vector2(0f, -40f);

        panel.anchoredPosition = hiddenPos;
        panel.localScale = Vector3.one * 0.95f;

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        passwordInput.text = string.Empty;
        isOpen = false;
    }

    // ===================== OPEN =====================

    public void Open()
    {
        if (isOpen) return;
        isOpen = true;

        tween?.Kill();

        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        tween = DOTween.Sequence()
            .Append(canvasGroup.DOFade(1f, duration))
            .Join(panel.DOAnchorPos(shownPos, duration))
            .Join(panel.DOScale(1f, duration).SetEase(ease))
            .OnComplete(() =>
            {
                passwordInput.ActivateInputField();
            });
    }

    // ===================== CLOSE =====================

    public void Close()
    {
        if (!isOpen) return;
        isOpen = false;

        tween?.Kill();

        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        tween = DOTween.Sequence()
            .Append(canvasGroup.DOFade(0f, duration))
            .Join(panel.DOAnchorPos(hiddenPos, duration))
            .Join(panel.DOScale(0.95f, duration));
    }

    // ===================== BUTTON =====================

    public void OnSubmitClicked()
    {
        string password = passwordInput.text;

        // 🔒 логика будет позже
        WebSocketManager.Instance.SendJoinRoom(enterRoomId, password);

        // временно просто закрываем
        Close();
    }
}
