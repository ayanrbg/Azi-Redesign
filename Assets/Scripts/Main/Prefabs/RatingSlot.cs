using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RatingSlot : MonoBehaviour
{
    public TextMeshProUGUI placeText;
    public TextMeshProUGUI experienceText;
    public TextMeshProUGUI usernameText;
    public Image avatarImage;

    public void SetRatingSlot(string place, int exp, string username, Sprite avatar)
    {
        avatarImage.sprite = avatar;
        experienceText.text = exp.ToString();
        placeText.text = place;
        usernameText.text = username;
    }
}
