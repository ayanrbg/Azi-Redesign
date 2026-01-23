using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomSlot : MonoBehaviour
{
    public string id;
    [SerializeField] private TextMeshProUGUI roomNameText;
    [SerializeField] private TextMeshProUGUI playerCountText;
    [SerializeField] private TextMeshProUGUI betText;
    [SerializeField] private Image[] avatarImages = new Image[3];
    private bool hasPassword = false;

    public void Init(string roomId, string name, int playerCount, int maxPlayers, bool hasPass)
    {
        id = roomId;
        roomNameText.text = name;
        playerCountText.text = playerCount+" из "+maxPlayers+" игроков";
        hasPassword = hasPass;
        LoadRandomPlayerSprites();
    }
    private void LoadRandomPlayerSprites()
    {
        foreach(Image playerImage in avatarImages)
        {
            playerImage.sprite = AvatarManager.Instance.avatarSprites26x26[Random.Range(0,5)];
        }
    }
    public void JoinRoom()
    {
        if(hasPassword == true)
        {
            RoomsManager.Instance.OpenPasswordPanel(id);
        }
        else
        {
            WebSocketManager.Instance.SendJoinRoom(id, "");
        }
        
    }
}
