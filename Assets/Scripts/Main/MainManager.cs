using TMPro;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    [Header("Player Profile")]
    [SerializeField] private TextMeshProUGUI balanceText;
    [Header("Panels")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject roomsPanel;
    [SerializeField] private GameObject experienceInfoPanel;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private GameObject dealPanel;
    #region Singleton
    public static MainManager Instance { get; private set; }
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
    #endregion

    #region Main
    private void OnEnable()
    {
        balanceText.text = MoneyFormatter.Format(GameState.Instance.userProfile.balance);
    }
    private void OnDisable()
    {
        
    }

    #endregion


    #region MethodsForButtons
    public void CloseAllPanels()
    {
        roomsPanel.SetActive(false);
        experienceInfoPanel.SetActive(false);
        shopPanel.SetActive(false);
    }
    public void OpenRoomsList()
    {
        CloseAllPanels();
        roomsPanel.SetActive(true);
        WebSocketManager.Instance.SendGetRooms();
    }
    public void OpenShop()
    {
        CloseAllPanels();
        shopPanel.SetActive(true);
    }
    public void OpenExperienceInfo()
    {
        CloseAllPanels();
        experienceInfoPanel.SetActive(true);
    }
    public void OpenDealPanel()
    {
        CloseAllPanels();
        dealPanel.SetActive(true);
    }

    #endregion

}
