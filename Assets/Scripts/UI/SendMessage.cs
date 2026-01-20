using UnityEngine;
using TMPro;

public class SendMessage : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    
    public void Send()
    {
        if(string.IsNullOrEmpty(inputField.text)) return;
        WebSocketManager.Instance.SendChat(inputField.text);
        inputField.text = null;
    }
}
