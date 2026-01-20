using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RequestSlot : MonoBehaviour
{
    public int requestId;
    [SerializeField] private Image avatarImage;
    [SerializeField] private TextMeshProUGUI usernameText;
    [SerializeField] private TextMeshProUGUI rankText;

    public void Init(int request_id, Sprite avatarSprite, string username,
        int level)
    {
        requestId = request_id;
        avatarImage.sprite = avatarSprite;
        usernameText.text = username;
        rankText.text = level.ToString();
    }
    public void AcceptRequest()
    {
        WebSocketManager.Instance.SendAcceptRequest(requestId);
    }
    public void DeclineRequest()
    {
        WebSocketManager.Instance.SendDeclineRequest(requestId);
    }
}
