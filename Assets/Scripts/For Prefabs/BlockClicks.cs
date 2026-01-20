using UnityEngine;
using UnityEngine.EventSystems;

public class BlockClicks : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        eventData.Use(); // ⬅ останавливает всплытие
    }
}
