using TMPro;
using UnityEngine;

public class GameUIController : MonoBehaviour
{
    [Header("Top Info")]
    [SerializeField] public TextMeshProUGUI statusText;
    [SerializeField] private TextMeshProUGUI roomNameText;
    [SerializeField] private TextMeshProUGUI roomLevelText;

    [Header("Chat / About Icons")]
    [SerializeField] private GameObject chatIconActive;
    [SerializeField] private GameObject chatIconInactive;
    [SerializeField] private GameObject chatOutline;
    [SerializeField] private GameObject aboutIconActive;
    [SerializeField] private GameObject aboutIconInactive;
    [SerializeField] private GameObject aboutOutline;
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI voteNightPlayersTitleText;
    [SerializeField] public TextMeshProUGUI playerCountText;

    [Header("Panels")]
    [SerializeField] private GameObject chatAndAboutPanel;
    [SerializeField] private GameObject chatBlockPanel;
    [SerializeField] private GameObject chatPanel;
    [SerializeField] private GameObject aboutPanel;
    [SerializeField] private GameObject gameContainer;
    [SerializeField] private BottomSheetDrag dayPlayersPanel;
    [SerializeField] private BottomSheetDrag voteNightPlayersPanel;
    [SerializeField] private BottomSheetDrag voteDayPlayersPanel;

    // ===================== PUBLIC API =====================

    public void SetStatus(string text)
    {
        statusText.text = text;
    }
    public void SetRoomName(string name)
    {
        roomNameText.text = name;
    }
    public void SetRoomLevel(string level)
    {
        roomLevelText.text = level;
    }
    public void EnableGamePanels()
    {
        chatAndAboutPanel.SetActive(false);
        chatIconActive.SetActive(false);
        aboutPanel.SetActive(false);
        gameContainer.SetActive(true);
        chatPanel.SetActive(true);
    }
    public void DisableGamePanels()
    {
        chatAndAboutPanel.SetActive(true);
        chatIconActive.SetActive(true);
        aboutPanel.SetActive(true);
        gameContainer.SetActive(false);
        chatPanel.SetActive(false);
    }
    public void EnableChatPanels()
    {
        chatIconInactive.SetActive(false);
        aboutOutline.SetActive(false);
        aboutIconActive.SetActive(false);
        aboutIconInactive.SetActive(true);
        chatIconActive.SetActive(true);
        chatOutline.SetActive(true);

        aboutPanel.SetActive(false);
        chatPanel.SetActive(true);
    }
    public void EnableAboutPanels()
    {
        chatIconInactive.SetActive(true);
        aboutOutline.SetActive(true);
        aboutIconActive.SetActive(true);
        aboutIconInactive.SetActive(false);
        chatIconActive.SetActive(false);
        chatOutline.SetActive(false);

        chatPanel.SetActive(false);
        aboutPanel.SetActive(true);
    }
    public void SetChatBlockPanel(bool value)
    {
        chatBlockPanel.SetActive(value);
    }
    public void DisableVoteNightPlayersPanel()
    {
        voteNightPlayersPanel.gameObject.SetActive(false);
        voteNightPlayersPanel.Close();
    }
    public void DisableVoteDayPlayersPanel()
    {
        voteDayPlayersPanel.gameObject.SetActive(false);
        voteNightPlayersPanel.Close();
    }
    public void EnableVoteDayPlayersPanel()
    {
        voteDayPlayersPanel.gameObject.SetActive(true);
        voteDayPlayersPanel.Open();
    }
    public void DisableChoosePanels(){
        voteDayPlayersPanel.gameObject.SetActive(false);
        voteDayPlayersPanel.Close();
        voteNightPlayersPanel.gameObject.SetActive(false);
        voteNightPlayersPanel.Close();
        dayPlayersPanel.gameObject.SetActive(false);
        dayPlayersPanel.Close();
    }
    public void EnableDayPlayerPanel()
    {
        dayPlayersPanel.gameObject.SetActive(true);
        dayPlayersPanel.Open();
    }
    public void DisableDayPlayerPanel()
    {
        dayPlayersPanel.gameObject.SetActive(false);
        dayPlayersPanel.Close();
    }
    public void ShowMafiaVote()
    {
        voteNightPlayersPanel.gameObject.SetActive(true);
        voteNightPlayersPanel.Open();
        voteNightPlayersTitleText.text = "Выбрать жертву";
        voteNightPlayersTitleText.color = new Color(198, 36, 14); //red
        chatBlockPanel.SetActive(false);
    }
    public void ShowDoctorVote()
    {
        voteNightPlayersPanel.gameObject.SetActive(true);
        voteNightPlayersPanel.Open();
        voteNightPlayersTitleText.text = "Вылечить игрока";
        voteNightPlayersTitleText.color = new Color(133, 207, 87); //green 
    }
    public void ShowSherifVote()
    {
        voteNightPlayersPanel.gameObject.SetActive(true);
        voteNightPlayersPanel.Open();
        voteNightPlayersTitleText.text = "Узнать роль";
        voteNightPlayersTitleText.color = new Color(104, 88, 203); //blue
    }
    public void ShowLoverVote()
    {
        voteNightPlayersPanel.gameObject.SetActive(true);
        voteNightPlayersPanel.Open();
        voteNightPlayersTitleText.text = "Заблокировать действие";
        voteNightPlayersTitleText.color = new Color(203, 88, 166); //pink
    }
    public void ShowPriestVote()
    {
        voteNightPlayersPanel.gameObject.SetActive(true);
        voteNightPlayersPanel.Open();
        voteNightPlayersTitleText.text = "Лишить права голоса";
        voteNightPlayersTitleText.color = new Color(203, 178, 88); //yellow
    }
    public void ShowBodyguardVote()
    {
        voteNightPlayersPanel.gameObject.SetActive(true);
        voteNightPlayersPanel.Open();
        voteNightPlayersTitleText.text = "Защитить игрока";
        voteNightPlayersTitleText.color = new Color(203, 178, 88); //yellow
    }
    public void ShowSniperVote()
    {
        voteNightPlayersPanel.gameObject.SetActive(true);
        voteNightPlayersPanel.Open();
        voteNightPlayersTitleText.text = "Выстрелить (1 раз)";
        voteNightPlayersTitleText.color = new Color(214, 155, 62); //orange
    }

    public void ShowChat()
    {
        chatIconActive.SetActive(true);
        chatIconInactive.SetActive(false);
        chatOutline.SetActive(true);

        aboutIconActive.SetActive(false);
        aboutIconInactive.SetActive(true);
        aboutOutline.SetActive(false);

        chatPanel.SetActive(true);
        aboutPanel.SetActive(false);
    }

    public void ShowAbout()
    {
        chatIconActive.SetActive(false);
        chatIconInactive.SetActive(true);
        chatOutline.SetActive(false);

        aboutIconActive.SetActive(true);
        aboutIconInactive.SetActive(false);
        aboutOutline.SetActive(true);

        chatPanel.SetActive(false);
        aboutPanel.SetActive(true);
    }

    public void EnterGameMode()
    {
        chatAndAboutPanel.SetActive(false);
        gameContainer.SetActive(true);
        chatPanel.SetActive(true);
    }

    public void ExitGameMode()
    {
        chatAndAboutPanel.SetActive(true);
        gameContainer.SetActive(false);
    }
}
