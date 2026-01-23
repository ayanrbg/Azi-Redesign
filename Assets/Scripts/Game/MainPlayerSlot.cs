using UnityEngine;
using UnityEngine.UI;

public class MainPlayerSlot : MonoBehaviour
{
    public int id;
    [SerializeField] private Image cardImage;
    public void SetPlayedCard(Sprite cardSprite)
    {
        cardImage.sprite = cardSprite;  
    }
}
