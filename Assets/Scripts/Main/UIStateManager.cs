using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class UIStateManager : MonoBehaviour
{
    [System.Serializable]
    public class Screen
    {
        public UIState state;
        public RectTransform root;
        //public CanvasGroup canvasGroup;
    }

    [Header("Screens")]
    [SerializeField] private Screen[] screens;

    [Header("Animation")]
    [SerializeField] private float duration = 0.35f;
    [SerializeField] private Ease ease = Ease.OutCubic;

    private Dictionary<UIState, Screen> screenMap;
    private UIState currentState;
    private float screenWidth;

    // ===================== LIFECYCLE =====================

    private void Awake()
    {
        screenWidth = ((RectTransform)transform).rect.width;

        screenMap = new Dictionary<UIState, Screen>();

        foreach (var s in screens)
        {
            screenMap[s.state] = s;

            s.root.gameObject.SetActive(false);

            //// ❗ НЕ ТРОГАЕМ alpha
            //s.canvasGroup.interactable = false;
            //s.canvasGroup.blocksRaycasts = false;
        }

        // стартовый экран
        SetState(UIState.Home, true);
    }

    // ===================== PUBLIC API =====================

    public void SetState(UIState newState, bool instant = false)
    {
        if (newState == currentState)
            return;

        Screen next = screenMap[newState];
        Screen current = screenMap.ContainsKey(currentState)
            ? screenMap[currentState]
            : null;

        int direction = GetDirection(currentState, newState);

        if (instant || current == null)
        {
            if (current != null)
                current.root.gameObject.SetActive(false);

            ActivateScreenInstant(next);
            currentState = newState;
            return;
        }

        AnimateTransition(current, next, direction);
        currentState = newState;
    }

    // ===================== CORE LOGIC =====================

    private void ActivateScreenInstant(Screen screen)
    {
        screen.root.DOKill();

        screen.root.gameObject.SetActive(true);
        //screen.root.anchoredPosition = Vector2.zero;

        //screen.canvasGroup.interactable = true;
        //screen.canvasGroup.blocksRaycasts = true;
    }


    private void AnimateTransition(Screen from, Screen to, int direction)
    {
        //// 🔥 1. УБИВАЕМ ВСЕ СТАРЫЕ АНИМАЦИИ
        //from.root.DOKill();
        //to.root.DOKill();

        // 🔥 2. СТАРЫЙ ЭКРАН ВЫКЛЮЧАЕМ СРАЗУ
        from.root.gameObject.SetActive(false);
        //from.root.anchoredPosition = Vector2.zero;

        //from.canvasGroup.interactable = false;
        //from.canvasGroup.blocksRaycasts = false;

        // 🔥 3. ГОТОВИМ НОВЫЙ ЭКРАН
        to.root.gameObject.SetActive(true);
        //to.root.anchoredPosition = new Vector2(direction * screenWidth, 0);

        //to.canvasGroup.interactable = true;
        //to.canvasGroup.blocksRaycasts = true;

        //// 🔥 4. АНИМИРУЕМ ТОЛЬКО ЕГО
        //to.root
        //    .DOAnchorPos(Vector2.zero, duration)
        //    .SetEase(ease);
    }

    // ===================== UTILS =====================

    private int GetDirection(UIState from, UIState to)
    {
        if (from == to)
            return 0;

        return (int)to > (int)from ? 1 : -1;
    }
}
