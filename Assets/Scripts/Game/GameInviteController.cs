using DG.Tweening;
using UnityEngine;

public class GameInviteController : MonoBehaviour
{
    [Header("Slots")]
    [SerializeField] private InviteFriendPrefab[] friendSlots;
    [Header("References")]
    [SerializeField] private GameObject invitePanel;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform panel;
    [SerializeField] private MorePanelAnimator morePanel;


    [Header("Animation")]
    [SerializeField] private float duration = 0.3f;
    [SerializeField] private Ease ease = Ease.OutCubic;

    private Tween tween;
    private Vector2 hiddenPos;
    private Vector2 shownPos;
    private Tween currentTween;
    private bool isOpen;
    private void OnEnable()
    {
        EventBus.OnFriendsList += LoadFriends;
    }
    private void OnDisable()
    {
        EventBus.OnFriendsList -= LoadFriends;
    }
    private void LoadFriends(FriendsListResponse friendsList)
    {
        foreach(InviteFriendPrefab pref in friendSlots)
        {
            pref.gameObject.SetActive(false);
        }
        int i = 0;
        foreach (FriendUser friend in friendsList.friends)
        {
            friendSlots[i].Init(friend.user_id,  GameManager.Instance.avatarSprites[friend.avatar_id],
                friend.username, friend.level, friend.is_online);
            friendSlots[i].gameObject.SetActive(true);
            i++;
        }
    }
    public void OpenPanel()
    {
        morePanel.Close();
        Open();
        WebSocketManager.Instance.SendGetFriends();
        
    }
    private void Awake()
    {
        // ❗ панель может быть выключена
        //invitePanel.SetActive(true);

        //shownPos = panel.anchoredPosition;
        //hiddenPos = shownPos + new Vector2(0, -500f);

        //panel.anchoredPosition = hiddenPos;
        //panel.localScale = Vector3.one * 0.9f;

        //canvasGroup.alpha = 0f;
        //canvasGroup.blocksRaycasts = false;
        //canvasGroup.interactable = false;

        invitePanel.SetActive(false);
    }
    private void Open()
    {
        if (isOpen) return;
        isOpen = true;

        invitePanel.SetActive(true);
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
                invitePanel.SetActive(false);
            //});
    }
}
