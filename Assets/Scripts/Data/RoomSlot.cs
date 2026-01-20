using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class RoleGameObject
{
    public Role role;
    public GameObject roleIcon;
}
public class RoomSlot : MonoBehaviour
{
    public int id;
    [SerializeField] private RoleGameObject[] roles;

    [SerializeField] private TextMeshProUGUI nameRoomText;
    [SerializeField] private TextMeshProUGUI rankRoomText;
    [SerializeField] private TextMeshProUGUI playerCountText;
    [SerializeField] private Slider playerSlider;
    [SerializeField] private GameObject lockIcon;
    [SerializeField] private TextMeshProUGUI statusText;
    private bool hasPass = false;
    public void Init(int roomId, string roomName, int roomRank,
        int playerCount, int maxPlayerCount, string[] roleNames, bool hasPassword, bool isPlaying)
    {
        id = roomId;
        nameRoomText.text = roomName;
        rankRoomText.text = roomRank.ToString(); 
        playerCountText.text = playerCount.ToString() + "/"
            + maxPlayerCount.ToString();
        if (isPlaying) statusText.text = "Набор закрыт";
        if (hasPassword) hasPass = true;
        lockIcon.SetActive(hasPassword);
        SetPlayerSlider(playerCount, maxPlayerCount);
        foreach (RoleGameObject go in roles)
        {
            go.roleIcon.SetActive(false);
        }
        foreach (string roleName in roleNames) 
        {
            SetRoleByName(roleName);
        }
    }
    
    private void SetRoleByName(string name)
    {
        
        foreach (var item in roles)
        {
            if (item.role.ToString() == name)
            {
                if (item.roleIcon != null)
                {
                    item.roleIcon.SetActive(true);
                }
                return;
            }
        }
    }

    private void SetPlayerSlider(int currentValue, int maxValue)
    {
        playerSlider.maxValue = maxValue;
        playerSlider.value = currentValue;
    }
    public void EnterRoom()
    {
        if (hasPass)
        {
            RoomLobbyManager.Instance.passwordPanel.Open();
            RoomLobbyManager.Instance.passwordPanel.enterRoomId = id;
        }
        else 
        {
            WebSocketManager.Instance.SendJoinRoom(id, "");
        }
            
    }
}
