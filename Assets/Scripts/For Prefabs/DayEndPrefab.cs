using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DayEndPrefab : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI usernameFromText;
    [SerializeField] private TextMeshProUGUI usernameToText;
    [SerializeField] private Image avatarFromImage;
    [SerializeField] private Image avatarToImage;

    public void Init(string usernameFrom, string usernameTo, Sprite avatarFrom,
        Sprite avatarTo, Color colorFrom, Color colorTo)
    {
        usernameFromText.color = colorFrom;
        usernameToText.color = colorTo;
        usernameFromText.text = usernameFrom;
        usernameToText.text = usernameTo;
        avatarFromImage.sprite = avatarFrom;
        avatarToImage.sprite = avatarTo;
    }
}
