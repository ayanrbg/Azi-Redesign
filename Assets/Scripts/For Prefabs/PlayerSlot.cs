using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSlot : MonoBehaviour
{
    public int userId;
    [SerializeField] private TextMeshProUGUI usernameText;
    
    [SerializeField] private Image avatarImage;
    [SerializeField] private GameObject labelImageMafia = null;
    [SerializeField] private GameObject labelImageYou = null;
    [SerializeField] private GameObject counterImage = null;
    [SerializeField] private TextMeshProUGUI counterText = null;
    
    public void SetPlayerSlot(int id, Sprite avatarSprite, string username)
    {
        userId = id;

        gameObject.SetActive(true);
        usernameText.text = username;
        avatarImage.sprite = avatarSprite; 
    }
    public void SetPlayerSlot(int id, Sprite avatarSprite, string username, bool isMafia)
    {
        CleanSlot();
        userId = id;
        if(userId == GameState.Instance.userId) 
            labelImageYou.SetActive(true);
        
        gameObject.SetActive(true);
        usernameText.text = username;
        avatarImage.sprite = avatarSprite;
        if (isMafia)
        {
            labelImageMafia.SetActive(true);
            usernameText.color = Color.red;
        }
    }
    private void CleanSlot()
    {
        if(labelImageYou != null)
            labelImageYou.SetActive(false);
        if(labelImageMafia != null)
            labelImageMafia?.SetActive(false);
        
        if(counterImage!= null)
        {
            counterImage.SetActive(false);
            counterText.text = "";
        }
    }
    public void ShowVotes(int userId, int voteCount, bool isMafia)
    {
        if (userId == GameState.Instance.userId)
            labelImageYou.SetActive(true);

        if (isMafia)
        {
            labelImageMafia.SetActive(true);
        }

        if (voteCount > 0)
        {
            counterImage.SetActive(true);
            counterText.text = voteCount.ToString();
        }
        else 
        {
            counterImage.SetActive(false);
            counterText.text = "";
        }
    }
    
    public void SendNightAction()
    {
        WebSocketManager.Instance.SendNightAction(userId);
    }
    public void SendDayAction()
    {
        WebSocketManager.Instance.SendDayVote(userId);
    }
}
