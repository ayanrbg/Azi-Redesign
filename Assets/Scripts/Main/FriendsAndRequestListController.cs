using TMPro;
using UnityEngine;

public class FriendsAndRequestListController : MonoBehaviour
{
    [SerializeField] private FriendSlot[] friendSlots;
    [SerializeField] private RequestSlot[] requestSlots;
    [SerializeField] private Sprite[] avatarSprites;
    [SerializeField] private GameObject friendSeparator;
    [SerializeField] private GameObject requestSeparator;
    [SerializeField] private GameObject emptyFriends;
    [SerializeField] private GameObject emptyRequests;
    [SerializeField] private TextMeshProUGUI nothingFoundText;
    [SerializeField] private TMP_InputField inputField;
    private void Start()
    {
        LoadFriendsList(GameState.Instance.CurrentFriendsList);
        LoadRequests(GameState.Instance.CurrentFriendsRequests);
    }
    public void LoadFriendsList(FriendsListResponse response)
    {
        nothingFoundText.gameObject.SetActive(false);
        friendSeparator.SetActive(true);
        emptyFriends.SetActive(false);
        ClearFriendSlots();

        if(response.friends.Length == 0)
        {
            emptyFriends.SetActive(true);
            return;
        }
        
        int i = 0;
        foreach (FriendUser user in response.friends)
        {
            friendSlots[i].Init(user.user_id, avatarSprites[user.avatar_id],
                user.username, user.level, true,
                user.is_online);
            friendSlots[i].gameObject.SetActive(true);
            i++;
        }
    }
    private void ClearFriendSlots()
    {
        foreach (FriendSlot slot in friendSlots)
        { 
            slot.gameObject.SetActive(false); 
        }
        
    }
    public void LoadPlayersList(SearchUsersResult searchUsersResult)
    {
        if (string.IsNullOrEmpty(inputField.text))
        {
            LoadFriendsList(GameState.Instance.CurrentFriendsList);
            LoadRequests(GameState.Instance.CurrentFriendsRequests);
            return;
        }
        nothingFoundText.gameObject.SetActive(false);
        ClearFriendSlots();
        friendSeparator.SetActive(false);
        requestSeparator.SetActive(false);
        emptyFriends.SetActive(false);
        emptyRequests.SetActive(false);

        if (searchUsersResult.users.Length == 0)
        {
            nothingFoundText.gameObject.SetActive(true);
            nothingFoundText.text = "По запросу \" " + inputField.text + "\" \\nничего не найдено";
            return;
        }
        
        
        int i = 0;
        foreach (SearchUser user in searchUsersResult.users)
        {
            friendSlots[i].Init(user.user_id, avatarSprites[user.avatar_id],
            user.username, user.level,
            user.isFriend, user.is_online
                );
            friendSlots[i].gameObject.SetActive(true);
            i++;
        }
    }
    public void SendSearch(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            Debug.Log("пусто");
            LoadFriendsList(GameState.Instance.CurrentFriendsList);
            LoadRequests(GameState.Instance.CurrentFriendsRequests);
        }
        Debug.Log(value);
        WebSocketManager.Instance.SendSearchUsers(value);
    }
    public void LoadRequests(FriendRequestsListResponse friendRequests)
    {
        nothingFoundText.gameObject.SetActive(false);
        emptyRequests.SetActive(false);
        requestSeparator.SetActive(true);
        ClearRequests();

        if (friendRequests.requests.Length == 0)
        {
            emptyRequests.SetActive(true);
            return;
        }
        int i = 0;
        foreach (FriendRequest user in friendRequests.requests)
        {
            requestSlots[i].Init(user.id, avatarSprites[user.avatar_id],
                user.username, user.level);
            requestSlots[i].gameObject.SetActive(true);
            i++;
        }
    }
    private void ClearRequests()
    {
        foreach (RequestSlot slot in requestSlots)
        {
            slot.gameObject.SetActive(false);
        }
    }
}
