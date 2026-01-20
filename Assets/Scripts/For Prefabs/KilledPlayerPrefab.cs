using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KilledPlayerPrefab : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI usernameText;
    [SerializeField] private RoleGameObject[] roles;
    [SerializeField] private Image avatarImage;

    public void SetKilledPlayer(string username, string role, Sprite avatar)
    {
        
        SetRole(role);
        usernameText.text = username;
        avatarImage.sprite = avatar;
    }
    private void SetRole(string role)
    {
        if(role == "mafia")
        {
            usernameText.color = Color.red;
            return;
        }
        foreach(RoleGameObject roleGameObject in roles)
        {
            if(role == roleGameObject.role.ToString())
            {
                roleGameObject.roleIcon.SetActive(true);
            }
        }
    }
}
