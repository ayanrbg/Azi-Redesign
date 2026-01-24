using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour
{
    public bool isFirstDiscard = true;
    public CardSpritesDatabase cardSpritesDatabase;
    [Header("Animators")]
    public CardToCenterAnimator cardToCenterAnimator;
    [Header("Panels")]
    [SerializeField] GameObject readyButton;
    public GameObject timerPanel;
    public GameObject betButtons;
    [SerializeField] TextMeshProUGUI betText;
    public GameObject requestPlayDecisionButtons;
    [SerializeField] GameObject potPanel;
    [SerializeField] TextMeshProUGUI potText;
    [SerializeField] Image trumpCardImage;
    [Header("Players")]
    public MainPlayerSlot mainPlayerSlot;
    [SerializeField] PlayerSlot[] playerSlots;
    public Dictionary<string, PlayerSlot> slotsById;
    public TimerUI mainTimer;

    private void RebuildSlotsDictionary()
    {
        slotsById = new Dictionary<string, PlayerSlot>();

        foreach (var slot in playerSlots)
        {
            if (string.IsNullOrEmpty(slot.playerId))
                continue;

            if (!slotsById.ContainsKey(slot.playerId))
                slotsById.Add(slot.playerId, slot);
        }
    }

    public void LoadJoinRoom(JoinedRoomResponse joinedRoomResponse)
    {
        //gameUIController.coinsText.text = GameState.Instance.userProfile.COINS.ToString();
        
        LoadJoinedRoomPlayers(joinedRoomResponse);
    }
   
    public void ShowPot(int value)
    {
        potPanel.SetActive(true);
        potText.text = MoneyFormatter.Format(value).ToString();
    }
    //public void UpdatePlayerCards(CurrentTrickPlayDTO[] currentTrick)
    //{
    //    foreach(CurrentTrickPlayDTO trick in currentTrick)
    //    {
    //        GetSlot(trick.playerId).ShowPlayedCard(trick.card);
    //    }
    //}
    public void ClearPlayersStates()
    {
        foreach (PlayerSlot playerSlot in playerSlots)
        {
            playerSlot.ClearPlayerState();
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
            if(player.id == GameState.Instance.userProfile.id)
            {
                mainPlayerSlot.LoadMainPlayer(player);
                i++;
                return;
            }
            playerSlots[i].gameObject.SetActive(true);
            playerSlots[i].Init(player.id,player.name, player.balance, player.ready);
            i++;
        }
        RebuildSlotsDictionary();
    }
    public void LoadRoomUpdatePlayers(RoomUpdateResponse roomUpdateResponse)
    {
        foreach (PlayerSlot playerSlot in playerSlots)
        {
            playerSlot.gameObject.SetActive(false);
        }
        int i = 0;
        foreach (RoomPlayer player in roomUpdateResponse.room.players)
        {
            if (player.id == GameState.Instance.userProfile.id)
            {
                mainPlayerSlot.LoadMainPlayer(player);
                continue;
            }
            playerSlots[i].gameObject.SetActive(true);
            playerSlots[i].Init(player.id, player.name, player.balance, player.ready);
            i++;
        }
        RebuildSlotsDictionary();
    }
    public void LoadRequestPlayDecision(RequestPlayDecisionResponse response)
    {
        readyButton.SetActive(false);
        requestPlayDecisionButtons.SetActive(true);
        potPanel.SetActive(true);
        potText.text = response.pot.ToString();
        trumpCardImage.gameObject.SetActive(true);
        trumpCardImage.sprite = cardSpritesDatabase.GetSprite(response.trumpCard.code);
        if (isFirstDiscard)
        {
            StartCoroutine(mainPlayerSlot.LoadCards(response.yourCards));
            isFirstDiscard = false;
        }
    }
    public void LoadGameBidding(RequestBidResponse response)
    {
        betButtons.SetActive(true);
        betText.text = MoneyFormatter.Format(response.baseBet).ToString();
        timerPanel.SetActive(true);
        mainTimer.StartTimer(15f);
    }
    public void LoadTimerCurrentPlayer(GameUpdateResponse response)
    {
        foreach (var player in playerSlots) 
        {
            player.StopTimer();
        }
        if (string.IsNullOrEmpty(response.currentPlayer))
            return;
        GetSlot(response.currentPlayer).StartTimer(15);
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
