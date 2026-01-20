using UnityEngine;

public class RoomLobbyManager : MonoBehaviour
{
    [SerializeField] private Transform contentLayout;
    [SerializeField] private GameObject roomSlotPrefab;
    [SerializeField] private CreateRoomPanelController roomPanelController;
    public PasswordPanel passwordPanel;


    public static RoomLobbyManager Instance { get; private set; }

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
        EventBus.OnRoomsUpdated += ShowRooms;
        EventBus.OnRoomJoined += JoinGame;

        if (GameState.Instance.Rooms != null && GameState.Instance.Rooms.Length > 0)
            ShowRooms(GameState.Instance.Rooms);

    }
    private void OnDisable()
    {
        EventBus.OnRoomsUpdated -= ShowRooms;
        EventBus.OnRoomJoined -= JoinGame;
    }
    private void Start()
    {
        WebSocketManager.Instance.SendGetRooms();
        InvokeRepeating(nameof(GetRoomsRoutine), 0f, 2f);
    }
    private void GetRoomsRoutine()
    {
        WebSocketManager.Instance.SendGetRooms();
    }
    private void ShowRooms(RoomInfo[] rooms)
    {
        foreach (Transform child in contentLayout)
        {
            Destroy(child.gameObject);
        }
        foreach (var room in rooms)
        {
            GameObject prefab = Instantiate(roomSlotPrefab, contentLayout);
            RoomSlot roomSlotScript = prefab.GetComponent<RoomSlot>();
            roomSlotScript.Init(room.id, room.name, room.level, room.current_players, room.max_players,
                room.roles, room.hasPassword, room.game_started);
        }
    }
    public void JoinGame(JoinRoomSuccessResponse response)
    {
        LoadingManager.Instance.LoadGameScene();
    }
    
    public void OpenCreatePanel()
    {
        roomPanelController.Open();
    }
    public void LoadMainScene()
    {
        LoadingManager.Instance.LoadMainScene();
    }
}
