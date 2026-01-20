using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AvatarSlotPrefab : MonoBehaviour
{
    public int id = 0;
    public TextMeshProUGUI priceText;
    public GameObject ownedIcon;
    public GameObject selectedIcon;
    public GameObject priceGO;
    public Image avatarImage;
    public void Init(int avatar_id, Sprite avatar_sprite, int price, bool isSelected, bool isOwned)
    {
        avatarImage.sprite = avatar_sprite;
        id = avatar_id;

        priceGO.SetActive(true);
        selectedIcon.SetActive(false);
        ownedIcon.SetActive(false);

        
        priceText.text = price.ToString();

        if (isOwned)
        {
            selectedIcon.SetActive(false);
            ownedIcon.SetActive(true);
            priceGO.SetActive(false);
        }
        if (isSelected)
        {
            selectedIcon.SetActive(true);
            ownedIcon.SetActive(false);
            priceGO.SetActive(false);
        }
        
        
    }
    public void SendBuyAvatar()
    {
        WebSocketManager.Instance.SendChangeAvatar(id);
    }
}
