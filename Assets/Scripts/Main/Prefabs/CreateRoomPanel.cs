using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoomPanel : MonoBehaviour
{
    [SerializeField] private Slider playerSlider;
    [SerializeField] private TextMeshProUGUI playerCountText;
    [SerializeField] private TMP_InputField nameField;
    [SerializeField] private TMP_InputField passwordField;
    public void OnSliderChanged()
    {
        playerCountText.text = playerSlider.value.ToString();
    }
    public void SendCreateRoom()
    {
        WebSocketManager.Instance.SendCreateRoom(nameField.text, passwordField.text,
            Convert.ToInt32(playerSlider.value));
    }
}
