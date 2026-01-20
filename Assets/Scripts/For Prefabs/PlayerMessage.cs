using TMPro;
using UnityEngine;

using UnityEngine.UI;

public class PlayerMessage : MonoBehaviour
{
    [SerializeField] private Image avatarImage;
    public TextMeshProUGUI usernameText;
    public TextMeshProUGUI messageText;
    public TextMeshProUGUI timeText;

    public void SetMessage(Sprite avatarSprite, string text, string username, string time)
    {
        messageText.text = text;
        usernameText.text = username;
        timeText.text = time;
        avatarImage.sprite = avatarSprite;
    }
}
