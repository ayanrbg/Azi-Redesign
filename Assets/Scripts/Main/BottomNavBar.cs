using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BottomNavBar : MonoBehaviour
{
    [System.Serializable]
    public class Tab
    {
        public UIState state;
        public Button button;

        [Header("Icons")]
        public GameObject inactiveIcon;
        public GameObject activeIcon;

        [Header("Indicator (optional)")]
        public RectTransform indicator;
    }

    [Header("Tabs")]
    [SerializeField] private Tab[] tabs;

    [Header("Animation")]
    [SerializeField] private float duration = 0.25f;
    [SerializeField] private float activeScale = 1.15f;

    [Header("Dependencies")]
    [SerializeField] private UIStateManager stateManager;

    private UIState current;

    private void Start()
    {
        SelectInternal(UIState.Home, true);
    }

    // ===================== CORE =====================

    private void SelectInternal(UIState state, bool instant)
    {
        if (state == current) return;

        current = state;
        stateManager.SetState(state, instant);

        foreach (var tab in tabs)
        {
            bool isActive = tab.state == state;

            tab.activeIcon.SetActive(isActive);
            tab.inactiveIcon.SetActive(!isActive);

            Transform activeTf = tab.activeIcon.transform;
            Transform inactiveTf = tab.inactiveIcon.transform;

            if (instant)
            {
                activeTf.localScale = Vector3.one * (isActive ? activeScale : 1f);
                inactiveTf.localScale = Vector3.one;
            }
            else
            {
                // ✅ активная иконка
                activeTf
                    .DOScale(isActive ? activeScale : 1f, duration)
                    .SetEase(Ease.OutCubic);

                // ✅ неактивная ВСЕГДА = 1
                inactiveTf
                    .DOScale(1f, duration)
                    .SetEase(Ease.OutCubic);
            }

            if (tab.indicator != null)
            {
                tab.indicator
                    .DOScaleX(isActive ? 1f : 0f, duration)
                    .SetEase(Ease.OutCubic);
            }
        }
    }

    // ===================== BUTTON WRAPPERS =====================

    public void SelectStory() => SelectInternal(UIState.Story, false);
    public void SelectFriends() 
    {
        SelectInternal(UIState.Friends, false);
        WebSocketManager.Instance.SendGetFriendRequests();
        WebSocketManager.Instance.SendGetFriends();
    } 
    public void SelectHome() => SelectInternal(UIState.Home, false);
    public void SelectRating() => SelectInternal(UIState.Rating, false);
    public void SelectShop() => SelectInternal(UIState.Shop, false);
}
