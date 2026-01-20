using DG.Tweening;
using UnityEngine;


[System.Serializable]
public class AvatarSlotResponse
{
    public int avatar_id;
    public string code;
    public int price;
    public bool owned;
    public bool selected;
}
[System.Serializable]
public class AvatarShopResponse : BaseMessage
{
    public AvatarSlotResponse[] avatars;
}
public class AvatarShopController : MonoBehaviour
{
    
    [Header("Profile Panel Reference")]
    [SerializeField] private GameObject avatarShopPanel;      // ❗ выключена в сцене
    [SerializeField] private RectTransform panel;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Basic Info")]
    [SerializeField] private AvatarSlotPrefab[] avatarSlots;

    [Header("Animation")]
    [SerializeField] private float duration = 0.35f;
    [SerializeField] private Ease ease = Ease.OutBack;

    private Vector2 shownPos;
    private Vector2 hiddenPos;
    private Tween currentTween;
    private bool isOpen;

    private void Awake()
    {
        //// ❗ панель может быть выключена
        //avatarShopPanel.SetActive(true);

        //shownPos = panel.anchoredPosition;
        //hiddenPos = shownPos + new Vector2(0, -500f);

        //panel.anchoredPosition = hiddenPos;
        //panel.localScale = Vector3.one * 0.9f;

        //canvasGroup.alpha = 0f;
        //canvasGroup.blocksRaycasts = false;
        //canvasGroup.interactable = false;

        avatarShopPanel.SetActive(false);
    }
    public void OpenPanel()
    {
        WebSocketManager.Instance.SendGetAvatarShop();
        Open();
    }
    public void LoadAvatars(AvatarShopResponse avatarShopResponse)
    {
        foreach(var slot in avatarSlots)
        {
            slot.gameObject.SetActive(false);
        }
        int i = 0;
        foreach (var avatarResponse in avatarShopResponse.avatars)
        {
            
            avatarSlots[i].Init(avatarResponse.avatar_id, 
                MainManager.Instance.largeAvatarSprites[avatarResponse.avatar_id],
                avatarResponse.price, avatarResponse.selected, avatarResponse.owned);
            avatarSlots[i].gameObject.SetActive(true);
            i++;
            
        }
        
    }
    private void Open()
    {
        if (isOpen) return;
        isOpen = true;

        avatarShopPanel.SetActive(true);
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
                avatarShopPanel.SetActive(false);
            //});
    }

}
