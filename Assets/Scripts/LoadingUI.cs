using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LoadingUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Slider progressSlider;
    [SerializeField] private TextMeshProUGUI progressText;

    [Header("Animation")]
    [SerializeField] private float fadeDuration = 0.25f;

    private void Awake()
    {
        // ⛔ Панель есть в сцене, но она не мешает
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;

        progressSlider.minValue = 0;
        progressSlider.maxValue = 100;
        progressSlider.wholeNumbers = true;
        progressSlider.value = 0;

        progressText.text = "0%";
    }

    public void Show()
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;

        canvasGroup
            .DOFade(1f, fadeDuration)
            .SetEase(Ease.OutQuad);
    }

    public void Hide()
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;

        canvasGroup
            .DOFade(0f, fadeDuration)
            .SetEase(Ease.InQuad);
    }

    public void SetProgress(float normalizedProgress)
    {
        int percent = Mathf.RoundToInt(normalizedProgress * 100f);

        progressSlider.value = percent;
        progressText.text = percent + "%";
    }
}
