using System.Collections;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSlot : MonoBehaviour
{
    public string playerId;
    [SerializeField] private GameObject[] cards = new GameObject[3];
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI balanceText;
    [SerializeField] private Image avatarImage;
    [SerializeField] private Image cardImage;
    [Header("Status Icons")]
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private GameObject blackStatusIcon;
    [SerializeField] private GameObject greenStatusIcon;
    [SerializeField] private GameObject blueStatusIcon;
    [SerializeField] private GameObject redStatusIcon;
    [Header("Bet Panel")]
    [SerializeField] private TextMeshProUGUI betValueText;
    [SerializeField] private GameObject betPanel;
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
    private void SetReadyPlayer()
    {
        greenStatusIcon.SetActive(true);
        statusText.text = "готов";
    }
    public void ShowPlayedCard(CardDTO playCardRequest)
    {
        cardImage.gameObject.SetActive(false);
        cardImage.sprite = GameManager.Instance.gameUIController.
            cardSpritesDatabase.GetSprite(playCardRequest.code);

        StartCoroutine(ShowCardRoutine());
        GameManager.Instance.gameUIController.cardToCenterAnimator.Animate(
            cards[cards.Length].gameObject.GetComponent<RectTransform>());
    }
    IEnumerator ShowCardRoutine()
    {
        yield return new WaitForSeconds(0.6f);
        cardImage.gameObject.SetActive(true);
        //анимация появления карты
    }
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
}
