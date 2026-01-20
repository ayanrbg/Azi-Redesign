using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FriendSlot : MonoBehaviour
{
    public int id;
    [SerializeField] private Image avatarImage;
    [SerializeField] private TextMeshProUGUI usernameText;
    [SerializeField] private TextMeshProUGUI rankText;
    [SerializeField] private GameObject addFriendIcon;
    [SerializeField] private GameObject onlineIcon;

    public void Init(int userId, Sprite avatarSprite, string username, 
        int level, bool isFriend, bool isOnline)
    {
        id = userId;
        avatarImage.sprite = avatarSprite;
        usernameText.text = username;
        rankText.text = level.ToString();
        addFriendIcon.SetActive(!isFriend);
        onlineIcon.SetActive(isOnline);
    }
    public void AddFriendRequest()
    {
        WebSocketManager.Instance.SendFriendRequest(id);
        addFriendIcon.SetActive(false);
    }
}
