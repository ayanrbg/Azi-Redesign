using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainPlayerSlot : MonoBehaviour
{
    public string id;
    [SerializeField] private Image yourCardImage;
    public HandView handView;
    [Header("Money")]
    [SerializeField] private TextMeshProUGUI balanceText;
    [SerializeField] private TextMeshProUGUI coinsText;
    

    public void Awake()
    {
        id = GameState.Instance.userProfile.id;
    }
    public void SetPlayedCard(Sprite cardSprite)
    {
        yourCardImage.sprite = cardSprite;  
    }
    public void LoadMainPlayer(RoomPlayer playerData)
    {
        balanceText.text = MoneyFormatter.Format(playerData.balance).ToString();
    }
    public IEnumerator LoadCards(CardDTO[] serverCards)
    {
        // 1. Полностью очистить руку
        yield return StartCoroutine(handView.ClearHand());

        // 2. Добавить карты с сервера
        for (int i = 0; i < serverCards.Length; i++)
        {
            yield return StartCoroutine(
                handView.AddCard(i.ToString(), serverCards[i].code)
            );
        }
    }
}
