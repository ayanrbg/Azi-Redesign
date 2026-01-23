using TMPro;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance { get; private set; }

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
    
    public GameUIController gameUIController;
    [SerializeField] private RectTransform card1;
    public void PlayAnimationCard()
    {
        card1.anchoredPosition = new Vector2(-800, 0);

        card1.DOAnchorPos(Vector2.zero, 0.6f)
            .SetEase(Ease.OutBack);
    }
    private void OnEnable()
    {
        gameUIController.LoadJoinRoom(GameState.Instance.joinedRoomData);
        EventBus.OnJoinedRoom += gameUIController.LoadJoinRoom;
        //EventBus.OnPlayedCard += gameUIController.ShowPlayedCard;
    }
    private void OnDisable()
    {
        EventBus.OnJoinedRoom -= gameUIController.LoadJoinRoom;
        //EventBus.OnPlayedCard += gameUIController.ShowPlayedCard;
    }
    
    public void SendReady()
    {
        WebSocketManager.Instance.SendReady();
    }
   
}
