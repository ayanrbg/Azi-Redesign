using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RangePlayersSlide : MonoBehaviour
{
    [Header("Sliders")]
    [SerializeField] private Slider minSlider;  
    [SerializeField] private Slider maxSlider;

    [Header("Fill")]
    [SerializeField] private RectTransform fillRect;

    [Header("Text")]
    [SerializeField] private TMP_Text valueText;

    [Header("Limits")]
    [SerializeField] private int minPlayers = 11;
    [SerializeField] private int maxPlayers = 19;
    [SerializeField] private int minRange = 4;

    private RectTransform trackRect;
    [SerializeField] private RoomRulesController rulesController;


    private void Awake()
    {
        trackRect = minSlider.GetComponent<RectTransform>();

        minSlider.minValue = minPlayers;
        minSlider.maxValue = maxPlayers;

        maxSlider.minValue = minPlayers;
        maxSlider.maxValue = maxPlayers;

        minSlider.wholeNumbers = true;
        maxSlider.wholeNumbers = true;

        minSlider.value = minPlayers;
        maxSlider.value = maxPlayers;
    }

    private void OnEnable()
    {
        minSlider.onValueChanged.AddListener(OnMinChanged);
        maxSlider.onValueChanged.AddListener(OnMaxChanged);
        UpdateUI();
    }

    private void OnDisable()
    {
        minSlider.onValueChanged.RemoveAllListeners();
        maxSlider.onValueChanged.RemoveAllListeners();
    }

    private void OnMinChanged(float value)
    {
        if (maxSlider.value - value < minRange)
            minSlider.value = maxSlider.value - minRange;

        UpdateUI();
    }

    private void OnMaxChanged(float value)
    {
        if (value - minSlider.value < minRange)
            maxSlider.value = minSlider.value + minRange;

        UpdateUI();
    }

    private void UpdateUI()
    {
        minSlider.value = Mathf.Clamp(minSlider.value, minPlayers, maxPlayers - minRange);
        maxSlider.value = Mathf.Clamp(maxSlider.value, minPlayers + minRange, maxPlayers);

        float minPercent = Mathf.InverseLerp(minPlayers, maxPlayers, minSlider.value);
        float maxPercent = Mathf.InverseLerp(minPlayers, maxPlayers, maxSlider.value);

        float width = trackRect.rect.width;

        fillRect.anchoredPosition = new Vector2(width * minPercent, 0);
        fillRect.sizeDelta = new Vector2(width * (maxPercent - minPercent), fillRect.sizeDelta.y);

        valueText.text = $"{(int)minSlider.value} — {(int)maxSlider.value}";
        rulesController.OnPlayersCountChanged();
    }

    // 🔹 ПОЛЕЗНО: получить значения из другого скрипта
    public int MinPlayers => (int)minSlider.value;
    public int MaxPlayers => (int)maxSlider.value;
}
