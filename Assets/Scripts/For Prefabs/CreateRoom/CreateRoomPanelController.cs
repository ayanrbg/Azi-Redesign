using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class CreateRoomPanelController : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private RectTransform panel;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float animationDuration = 0.25f;

    [Header("Inputs")]
    [SerializeField] private TMP_InputField roomNameInput;

    [Header("Rank (stub)")]
    [SerializeField] private Slider rankSlider;
    [SerializeField] private TMP_Text rankValueText;

    [Header("Players")]
    [SerializeField] private RangePlayersSlide playersSlider;

    [Header("Rules")]
    [SerializeField] private RoomRulesController rulesController;

    private bool isOpen;

    private void Awake()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        panel.localScale = Vector3.one * 0.9f;
        OnRankChanged(rankSlider.value);
    }
    private void Start()
    {
        if (GameState.Instance.isCreateButton)
        {
            Debug.Log(GameState.Instance.isCreateButton);
            Open();
            GameState.Instance.isCreateButton = false;
        }
    }
    #region PANEL ANIMATION

    public void Open()
    {
        if (isOpen) return;
        isOpen = true;

        gameObject.SetActive(true);
        canvasGroup.blocksRaycasts = true;

        DOTween.Sequence()
            .Append(canvasGroup.DOFade(1f, animationDuration))
            .Join(panel.DOScale(1f, animationDuration).SetEase(Ease.OutBack));
    }

    public void Close()
    {
        if (!isOpen) return;
        isOpen = false;

        canvasGroup.blocksRaycasts = false;

        DOTween.Sequence()
            .Append(canvasGroup.DOFade(0f, animationDuration))
            .Join(panel.DOScale(0.9f, animationDuration))
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
    }

    #endregion

    #region UI CALLBACKS

    // 🔹 Просто отображение значения
    public void OnRankChanged(float value)
    {
        rankValueText.text = Mathf.RoundToInt(value).ToString();
    }

    #endregion

    #region CREATE ROOM

    public void OnCreateRoomClicked()
    {
        if (string.IsNullOrWhiteSpace(roomNameInput.text))
        {
            Debug.LogWarning("Room name is empty");
            return;
        }

        CreateRoomRequest request = new CreateRoomRequest
        {
            name = roomNameInput.text,
            min_players = playersSlider.MinPlayers,
            max_players = playersSlider.MaxPlayers,
            level = Mathf.RoundToInt(rankSlider.value), // 🔥 просто передаём
            mafia_count = rulesController.GetMafiaCount(),
            roles = rulesController.GetSelectedRoles()
        };

        Debug.Log("CREATE ROOM REQUEST:");
        Debug.Log(JsonUtility.ToJson(request, true));

        WebSocketManager.Instance.SendCreateRoom(request);
        Close();
    }

    #endregion
}
