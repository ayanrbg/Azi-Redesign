using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class InvitePanelController : MonoBehaviour
{
    private int roomId;
    [Header("Root")]
    [SerializeField] private CanvasGroup overlay;
    [SerializeField] private RectTransform panel;
    [SerializeField] private RectTransform glow;
    [SerializeField] private Image avatarImage;
    [SerializeField] private Button buttonAccept;
    [SerializeField] private Button buttonDecline;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI usernameText;
    [SerializeField] private TextMeshProUGUI roomNameText;
    [SerializeField] private TextMeshProUGUI roomLevelText;

    [Header("Roles Icons")]
    [SerializeField] private RoleGameObject[] roleIcons;
    // RoleIconView Ч маленький скрипт, покажу ниже

    [Header("Animation")]
    [SerializeField] private float duration = 0.35f;
    [SerializeField] private Ease panelEase = Ease.OutBack;

    private Vector2 hiddenPos;
    private Vector2 shownPos;
    private bool isOpen;

    // ===================== LIFECYCLE =====================

    private void Awake()
    {
        buttonAccept.onClick.AddListener(OnAccept);
        buttonDecline.onClick.AddListener(OnDecline);
        shownPos = panel.anchoredPosition;
        hiddenPos = shownPos + new Vector2(0, -500f);

        panel.anchoredPosition = hiddenPos;
        panel.localScale = Vector3.one * 0.9f;

        overlay.alpha = 0f;
        overlay.blocksRaycasts = false;

        gameObject.SetActive(false);

        // glow вращаетс€ всегда
        glow.DORotate(new Vector3(0, 0, -360f), 10f, RotateMode.FastBeyond360)
            .SetLoops(-1)
            .SetEase(Ease.Linear);
    }

    // ===================== PUBLIC API =====================

    public void Init(
        string username,
        Sprite avatarSprite,
        int room_id,
        string roomName,
        int roomLevel, 
        string[] roles
    )
    {
        roomId = room_id;
        usernameText.text = username;
        roomNameText.text = roomName;
        roomLevelText.text = roomLevel.ToString();
        avatarImage.sprite = avatarSprite;

        foreach (var icon in roleIcons)
            icon.roleIcon.SetActive(false);

        foreach (var roleName in roles)
        {
            foreach (var icon in roleIcons)
            {
                if (roleName == icon.role.ToString())
                {
                    icon.roleIcon.SetActive(true);
                    break;
                }
            }
        }
    }
    public void LoadInvite(GameInvite gameInvite)
    {
        Init(gameInvite.from.username, MainManager.Instance.avatarSprites[gameInvite.from.avatar_id],
            gameInvite.room.id, gameInvite.room.name,
            gameInvite.room.level, gameInvite.room.roles);
        Open();
    }
    public void Open()
    {
        if (isOpen) return;
        isOpen = true;

        gameObject.SetActive(true);
        overlay.blocksRaycasts = true;

        DOTween.Sequence()
            .Append(overlay.DOFade(1f, 0.2f))
            .Append(panel.DOAnchorPos(shownPos, duration))
            .Join(panel.DOScale(1f, duration).SetEase(panelEase));
    }

    public void Close()
    {
        if (!isOpen) return;
        isOpen = false;

        overlay.blocksRaycasts = false;

        DOTween.Sequence()
            .Append(panel.DOAnchorPos(hiddenPos, duration * 0.8f))
            .Join(panel.DOScale(0.9f, duration * 0.8f))
            .Append(overlay.DOFade(0f, 0.2f))
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
    }

    // ===================== UI BUTTONS =====================

    public void OnAccept()
    {
        WebSocketManager.Instance.SendJoinRoom(roomId, "");
        Debug.Log(roomId + " - ќтправил");
        Close();
    }

    public void OnDecline()
    {
        Debug.Log("ZAKRYT'");
        Close();
    }
}
