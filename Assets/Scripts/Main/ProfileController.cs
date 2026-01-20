using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ProfileController : MonoBehaviour
{
    [Header("Profile HEADER")]
    [SerializeField] private Image avatarImageHeader;
    [SerializeField] private TextMeshProUGUI expTextHeader;
    [SerializeField] private TextMeshProUGUI levelTextHeader;
    [SerializeField] private TextMeshProUGUI moneyTextHeader;
    [SerializeField] private Slider moneySliderHeader;
    [SerializeField] private Slider experienceSliderHeader;

    [Header("Profile Panel Reference")]
    [SerializeField] private GameObject profilePanel;      // ❗ выключена в сцене
    [SerializeField] private RectTransform panel;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Basic Info")]
    [SerializeField] private TextMeshProUGUI usernameText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Image avatarImage;

    [Header("Stats")]
    [SerializeField] private TextMeshProUGUI gamesPlayedText;
    [SerializeField] private TextMeshProUGUI mafiaGamesText;
    [SerializeField] private TextMeshProUGUI mafiaWinsText;
    [SerializeField] private TextMeshProUGUI peacefulGamesText;
    [SerializeField] private TextMeshProUGUI peacefulWinsText;

    [Header("Animation")]
    [SerializeField] private float duration = 0.35f;
    [SerializeField] private Ease ease = Ease.OutBack;

    private Vector2 shownPos;
    private Vector2 hiddenPos;
    private Tween currentTween;
    private bool isOpen;

    // ===================== INIT =====================

    private void Awake()
    {
        //// ❗ панель может быть выключена
        //profilePanel.SetActive(true);

        //shownPos = panel.anchoredPosition;
        //hiddenPos = shownPos + new Vector2(0, -500f);

        //panel.anchoredPosition = hiddenPos;
        //panel.localScale = Vector3.one * 0.9f;

        //canvasGroup.alpha = 0f;
        //canvasGroup.blocksRaycasts = false;
        //canvasGroup.interactable = false;

        profilePanel.SetActive(false);
    }

    // ===================== PUBLIC API =====================
    public void SetupPlayerInfo(UserData userdata)
    {
        avatarImageHeader.sprite = MainManager.Instance.avatarSprites[
            GameState.Instance.PlayerProfile.avatar_id];

        expTextHeader.text = userdata.level + " РАНГ";
        //experienceSliderHeader.value = 100;
        //moneyTextHeader.text = "500";
    }
    public void SetupPlayerInfo(UserStats userStats)
    {
        avatarImageHeader.sprite = MainManager.Instance.avatarSprites[userStats.avatar_id];

        expTextHeader.text = userStats.level + " РАНГ";
        //expTextHeader.text = "100";
        //experienceSliderHeader.value = 100;
        //moneyTextHeader.text = "500";
    }
    public void Init(
        string username,
        int level,
        Sprite avatar,
        ProfileStats stats
    )
    {
        usernameText.text = username;
        levelText.text = level.ToString();
        avatarImage.sprite = avatar;

        gamesPlayedText.text = stats.games_played.ToString();
        mafiaGamesText.text = stats.mafia_games.ToString();
        mafiaWinsText.text = stats.mafia_wins.ToString();
        peacefulGamesText.text = stats.peaceful_games.ToString();
        peacefulWinsText.text = stats.peaceful_wins.ToString();
    }
    public void LoadProfile(UserStats userStats)
    {
        Init(userStats.username, userStats.level,
            MainManager.Instance.largeAvatarSprites[userStats.avatar_id], userStats.stats);
    }
    public void Open()
    {
        if (isOpen) return;
        isOpen = true;

        profilePanel.SetActive(true);
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
                profilePanel.SetActive(false);
            //});
    }
}
