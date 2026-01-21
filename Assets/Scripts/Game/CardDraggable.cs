using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDraggable : MonoBehaviour, 
    //IPointerClickHandler, 
    IBeginDragHandler, IDragHandler 
    //IEndDragHandler
{
    private Camera mainCamera;
    private Vector3 startPosition;
    private Quaternion startRotation;
    private bool isDragging;
    private GameManager gameManager;

    public string cardId;
    [HideInInspector] public SpriteRenderer cardSprite;
    public CardManager cardManager;

    private void Awake()
    {
        mainCamera = Camera.main;
        gameManager = FindFirstObjectByType<GameManager>();
        cardSprite = GetComponentInChildren<SpriteRenderer>();
    }

    public void Init(string id)
    {
        cardId = id;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        isDragging = true;

        // Немного поднимаем карту визуально (по Z)
        transform.position += Vector3.forward * -0.5f;

        // Увеличиваем для визуального эффекта
        transform.localScale = Vector3.one * 1.1f;
        transform.DORotate(new Vector3(0, 0, 0), 0.15f);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        // Переводим позицию курсора в мировые координаты
        Vector3 screenPos = new Vector3(eventData.position.x, eventData.position.y, -mainCamera.transform.position.z);
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(screenPos);

        // Двигаем карту по X/Y
        transform.position = new Vector3(worldPos.x, worldPos.y, transform.position.z);
    }

    //public void OnEndDrag(PointerEventData eventData)
    //{
    //    isDragging = false;

    //    // Возвращаем карту на место, если нельзя сбросить
    //    bool canDrop = false;

    //    // Проверяем, навели ли на drop-зону
    //    //if (gameManager.IsValiableCard(this) && gameManager.myTurn && transform.position.y > -1.5f)
    //    {
    //        canDrop = true;
    //        //gameManager.OnCardDropped(this);
    //    }
    //    else
    //    {
    //        //Debug.Log(gameManager.IsValiableCard(this));
    //    }

    //    if (!canDrop)
    //    {
    //        transform.position = startPosition;
    //    }

    //    // Возвращаем масштаб и порядок
    //    transform.localScale = Vector3.one;
    //    transform.position = new Vector3(transform.position.x, transform.position.y, startPosition.z);
    //    transform.DORotate(startRotation.eulerAngles, 0.2f);
    //}

    //public void OnPointerClick(PointerEventData eventData)
    //{
    //    //if (gameManager != null && gameManager.isChoosingDiscard)
    //    {
    //        //gameManager.DiscardCard(this);

    //    }
    //}
}
