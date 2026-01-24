using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerSlot : MonoBehaviour
{
    public string playerId;
    [SerializeField] private GameObject[] cards = new GameObject[3];
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI balanceText;
    [SerializeField] private Image avatarImage;
    [SerializeField] private Image cardImage;
    [SerializeField] private TimerUI timer;
    [Header("Status Icons")]
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private GameObject blackStatusIcon;
    [SerializeField] private GameObject greenStatusIcon;
    [SerializeField] private GameObject blueStatusIcon;
    [SerializeField] private GameObject redStatusIcon;
    [Header("Bet Panel")]
    [SerializeField] private TextMeshProUGUI betValueText;
    [SerializeField] private GameObject betPanel;
    [Header("Card Fly Animation")]
    [SerializeField] private float flyDuration = 0.6f;
    [SerializeField] private Ease flyEase = Ease.OutCubic;


    private Vector2 cardTargetAnchoredPos;
    private Quaternion cardTargetRotation;

    public void Init(string id,string username, 
        //int avatar_id,
        int balance, bool ready)
    {
        playerId = id;
        nameText.text = username;
        //avatarImage.sprite = AvatarManager.Instance.avatarSprites152x152[avatar_id];
        balanceText.text = MoneyFormatter.Format(balance);
        if(ready) SetReadyPlayer();
    }
    public void StartTimer(int seconds)
    {
        timer.StartReverseTimer(seconds);
    }
    public void StopTimer()
    {
        timer.StopTimer();
    }
    public void ShowPlayedCard(CardDTO card)
    {
        betPanel.SetActive(false);
        //cardImage.gameObject.SetActive(true);
        //cardImage.sprite = GameManager.Instance.
        //    gameUIController.cardSpritesDatabase.GetSprite(card.code);
        FlyCardFromAvatar(card);
    }
    private void SetReadyPlayer()
    {
        greenStatusIcon.SetActive(true);
        statusText.text = "готов";
    }
    public void ClearPlayerState()
    {
        statusText.text = "";
        blackStatusIcon.SetActive(true);
        greenStatusIcon.SetActive(false);
        blueStatusIcon.SetActive(false);
        redStatusIcon.SetActive(false);
    }
    //public void ShowPlayedCard(CardDTO playCardRequest)
    //{
    //    cardImage.gameObject.SetActive(false);
    //    cardImage.sprite = GameManager.Instance.gameUIController.
    //        cardSpritesDatabase.GetSprite(playCardRequest.code);

    //    StartCoroutine(ShowCardRoutine());
    //    GameManager.Instance.gameUIController.cardToCenterAnimator.Animate(
    //        cards[cards.Length].gameObject.GetComponent<RectTransform>());
    //}
    //IEnumerator ShowCardRoutine()
    //{
    //    yield return new WaitForSeconds(0.6f);
    //    cardImage.gameObject.SetActive(true);
    //    //анимация появления карты
    //}
    private void SetActiveCards(int count)
    {
        foreach (GameObject card in cards)
        {
            card.SetActive(false);
        }
        for (int i = 0; i < count; i++)
        {
            cards[i].SetActive(true);
        }
    }
    public void FlyCardFromAvatar(CardDTO card)
    {
        RectTransform cardRect = cardImage.rectTransform;
        RectTransform avatarRect = avatarImage.rectTransform;

        // 1️⃣ Запоминаем конечное положение карты (ГДЕ ОНА ДОЛЖНА ЛЕЖАТЬ)
        cardTargetAnchoredPos = cardRect.anchoredPosition;
        cardTargetRotation = cardRect.rotation;

        // 2️⃣ Инициализация карты
        cardImage.sprite = GameManager.Instance
            .gameUIController.cardSpritesDatabase.GetSprite(card.code);

        cardImage.gameObject.SetActive(true);

        // 3️⃣ Переводим позицию аватара в локальные координаты cardImage.parent
        Vector2 avatarLocalPos = WorldToAnchoredPosition(
            cardRect.parent as RectTransform,
            avatarRect.position
        );

        // 4️⃣ Ставим карту в позицию аватара
        cardRect.anchoredPosition = avatarLocalPos;
        cardRect.rotation = Quaternion.identity;
        cardRect.localScale = Vector3.one;

        cardRect.DOKill();

        // 5️⃣ Анимация полёта
        Sequence seq = DOTween.Sequence();

        seq.Join(
            cardRect.DOAnchorPos(cardTargetAnchoredPos, flyDuration)
                .SetEase(flyEase)
        );

        seq.Join(
            cardRect.DORotateQuaternion(cardTargetRotation, flyDuration)
                .SetEase(flyEase)
        );
    }
    private Vector2 WorldToAnchoredPosition(
        RectTransform parent,
        Vector3 worldPos
    )
    {
        Vector2 screenPoint =
            RectTransformUtility.WorldToScreenPoint(null, worldPos);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parent,
            screenPoint,
            null,
            out Vector2 localPoint
        );

        return localPoint;
    }

}
