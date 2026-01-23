using TMPro;
using UnityEngine;

public class EnterPasswordPanel : MonoBehaviour
{
    public string enterId;
    [SerializeField] private TMP_InputField passInput;
    [SerializeField] private GameObject incorrectGO;
    [SerializeField] private TextMeshProUGUI incorrectText;
    [SerializeField] private GameObject correctGO;
    
    public void SendPasswordAndEnter()
    {
        WebSocketManager.Instance.SendJoinRoom(enterId, passInput.text);
        incorrectGO.SetActive(false);
    }
    private void OnEnable()
    {
        EventBus.OnError += HandleError;
    }
    private void OnDisable()
    {
        EventBus.OnError -= HandleError;
    }
    private void HandleError(ErrorResponse error)
    {
        incorrectGO.SetActive(true);
        incorrectText.text = error.message;
    }
}
