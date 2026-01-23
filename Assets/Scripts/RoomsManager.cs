using TMPro;
using UnityEngine;

public class RoomsManager : MonoBehaviour
{

    [SerializeField] private EnterPasswordPanel passwordPanel;
    public static RoomsManager Instance { get; private set; }
    [SerializeField] private RoomSlot[] roomSlots;
    [SerializeField] private TextMeshProUGUI balanceText;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
        else
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        EventBus.OnRoomsList += LoadRooms;
        EventBus.OnJoinedRoom += JoinRoom;
    }
    private void OnDisable()
    {
        EventBus.OnRoomsList -= LoadRooms;
        EventBus.OnJoinedRoom -= JoinRoom;
    }
    public void LoadRooms(RoomsListResponse roomsListResponse)
    {
        foreach(RoomSlot slot in roomSlots)
        {
            slot.gameObject.SetActive(false);
        }
        int i = 0;
        foreach (RoomInfo roomInfo in roomsListResponse.rooms) {
            roomSlots[i].gameObject.SetActive(true);
            roomSlots[i].Init(roomInfo.id, roomInfo.name, roomInfo.players, 
                roomInfo.maxPlayers, roomInfo.hasPassword);
            i++;
        }
    }
    public void JoinRoom(JoinedRoomResponse joinedRoomResponse)
    {
        LoadingManager.Instance.LoadGameScene();
    }
    public void OpenPasswordPanel(string id)
    {
        passwordPanel.enterId = id;
        passwordPanel.gameObject.SetActive(true);
    }
}
