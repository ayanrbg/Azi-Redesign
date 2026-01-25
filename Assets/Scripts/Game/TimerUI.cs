using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerUI : MonoBehaviour
{
    public enum FillMode
    {
        Decrease, // 1 → 0
        Increase  // 0 → 1
    }

    [Header("UI")]
    public Image fillImage;
    [SerializeField] private TextMeshProUGUI timeText;

    [Header("Settings")]
    [SerializeField] private float defaultDuration = 15f;

    private float duration;
    private float timeLeft;
    private bool isRunning;
    private FillMode fillMode = FillMode.Decrease;

    // ---------------------------
    // PUBLIC API
    // ---------------------------

    /// <summary>
    /// Обычный таймер: fill убывает
    /// </summary>
    public void StartTimer(float seconds)
    {
        StartTimer(seconds, FillMode.Decrease);
    }

    /// <summary>
    /// Реверсивный таймер: fill заполняется
    /// </summary>
    public void StartReverseTimer(float seconds)
    {
        StartTimer(seconds, FillMode.Increase);
    }

    private void StartTimer(float seconds, FillMode mode)
    {
        duration = seconds;
        timeLeft = seconds;
        fillMode = mode;
        isRunning = true;

        UpdateUI();
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        isRunning = false;
        timeLeft = duration;
        UpdateUI();
    }

    public bool IsRunning => isRunning;

    // ---------------------------
    // UPDATE
    // ---------------------------

    private void Update()
    {
        if (!isRunning)
            return;

        timeLeft -= Time.deltaTime;

        if (timeLeft <= 0f)
        {
            timeLeft = 0f;
            isRunning = false;
            OnTimerFinished();
        }

        UpdateUI();
    }

    // ---------------------------
    // UI
    // ---------------------------

    private void UpdateUI()
    {
        float normalized = Mathf.Clamp01(timeLeft / duration);

        if (fillImage != null)
        {
            fillImage.fillAmount =
                fillMode == FillMode.Decrease
                    ? normalized
                    : 1f - normalized;
        }

        if (timeText != null)
        {
            timeText.text = Mathf.CeilToInt(timeLeft).ToString();
        }
    }

    // ---------------------------
    // EVENTS
    // ---------------------------

    protected virtual void OnTimerFinished()
    {
        Debug.Log("Timer finished");
    }
}
