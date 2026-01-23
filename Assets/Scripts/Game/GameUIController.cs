using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUIController : MonoBehaviour
{

    public CardSpritesDatabase cardSpritesDatabase;
    [Header("Animators")]
    public CardToCenterAnimator cardToCenterAnimator;
    [Header("Money")]
    [SerializeField] private TextMeshProUGUI balanceText;
    [SerializeField] private TextMeshProUGUI coinsText;
    [Header("Panels")]
    [SerializeField] GameObject betButtons;
    [SerializeField] GameObject potPanel;
    [SerializeField] TextMeshProUGUI potText;
    [Header("Players")]
    public MainPlayerSlot mainPlayerSlot;
    [SerializeField] PlayerSlot[] playerSlots;
    private Dictionary<string, PlayerSlot> slotsById;

    private void Awake()
    {
        slotsById = new Dictionary<string, PlayerSlot>();

        foreach (var slot in playerSlots)
        {
            if (!slotsById.ContainsKey(slot.playerId))
                slotsById.Add(slot.playerId, slot);
        }
    }
    public void LoadJoinRoom(JoinedRoomResponse joinedRoomResponse)
    {
        balanceText.text = GameState.Instance.userProfile.balance.ToString();
        //gameUIController.coinsText.text = GameState.Instance.userProfile.COINS.ToString();

        LoadJoinedRoomPlayers(joinedRoomResponse);
    }
   
    public void ShowPot(int value)
    {
        potPanel.SetActive(true);
        potText.text = MoneyFormatter.Format(value).ToString();
    }
    public void UpdatePlayerCards(CurrentTrickPlayDTO[] currentTrick)
    {
        foreach(CurrentTrickPlayDTO trick in currentTrick)
        {
            GetSlot(trick.playerId).ShowPlayedCard(trick.card);
        }
    }
    public PlayerSlot GetSlot(string id)
    {
        if (slotsById.TryGetValue(id, out var slot))
            return slot;

        Debug.LogWarning($"PlayerSlot with id {id} not found");
        return null;
    }
    public void LoadJoinedRoomPlayers(JoinedRoomResponse joinedRoomResponse)
    {
        foreach (PlayerSlot playerSlot in playerSlots) 
        { 
            playerSlot.gameObject.SetActive(false);
        }
        int i = 0;
        foreach (RoomPlayer player in joinedRoomResponse.room.players) 
        {
            playerSlots[i].gameObject.SetActive(true);
            playerSlots[i].Init(player.id,player.name, player.balance, player.ready);
            i++;
        }
    }
    public void CloseBetButtons()
    {
        betButtons.SetActive(false);
    }
    public void OpenBetButtons()
    {
        betButtons.SetActive(true);
    }
}
