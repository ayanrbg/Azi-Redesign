using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InviteFriendPrefab : MonoBehaviour
{
    private int userId;
    [SerializeField] Image avatarImage;
    [SerializeField] TextMeshProUGUI usernameText;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] GameObject isOnlineIcon;
    [SerializeField] GameObject sendIcon;
    public void Init(int id, Sprite avatarSprite, string username, int level, bool isOnline)
    {
        userId = id;
        avatarImage.sprite = avatarSprite;
        usernameText.text = username;
        levelText.text = level.ToString();
        isOnlineIcon.SetActive(isOnline);
        sendIcon.SetActive(true);
    }
    public void SendInvite()
    {
        WebSocketManager.Instance.SendInviteGameRequest(userId);
        sendIcon.SetActive(false);
    }
}
