using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Splines;
using UnityEngine;

public class HandView : MonoBehaviour
{
    [SerializeField] private SplineContainer splineContainer;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private RectTransform spawnPoint;
    public List<CardDraggable> cards = new();

    private CardDraggable selectedCard;
    [SerializeField] private float selectOffsetY = 40f;
    [SerializeField] private float selectAnimDuration = 0.2f;

    public IEnumerator AddCard(string code, string cardCode)
    {
        CardDraggable card = Instantiate(cardPrefab, spawnPoint.transform)
            .GetComponent<CardDraggable>();

        cards.Add(card);
        card.Init(cards.Count-1, cardCode);

        card.transform.localScale = Vector3.one;
        card.transform.DOScale(1f, 0.25f).SetEase(Ease.OutBack);

        yield return UpdateCardPositions(0.25f);
    }
    public void UpdateCards(float duration)
    {
        StartCoroutine(UpdateCardPositions(duration));
    }
    public IEnumerator UpdateCardPositions(float duration)
    {
        if (cards.Count == 0)
            yield break;

        float spacing = cards.Count > 1
            ? 1f / (cards.Count - 1)
            : 0.5f;

        float maxFanAngle = 20f;

        for (int i = 0; i < cards.Count; i++)
        {
            float t = Mathf.Clamp01(i * spacing);

            Vector3 pos = splineContainer.EvaluatePosition(t);

            float angle = Mathf.Lerp(
                maxFanAngle,
                -maxFanAngle,
                cards.Count > 1 ? (float)i / (cards.Count - 1) : 0.5f
            );

            Transform tr = cards[i].transform;
            tr.DOKill();

            tr.DOMove(pos + Vector3.back * 0.05f * i, duration)
              .SetEase(Ease.OutCubic);

            tr.DORotateQuaternion(
                Quaternion.Euler(0f, 0f, angle),
                duration
            ).SetEase(Ease.OutCubic);
        }

        yield return new WaitForSeconds(duration);
    }
    public IEnumerator ClearHand(float duration = 0.25f)
    {
        if (cards.Count == 0)
            yield break;
        foreach (var card in cards)
        {
            if (card == null) continue;

            Transform tr = card.transform;
            tr.DOKill();

            // Улетает вниз + уменьшается
            tr.DOMoveY(tr.position.y - 200f, duration)
              .SetEase(Ease.InCubic);

            tr.DOScale(0f, duration)
              .SetEase(Ease.InCubic);
        }

        yield return new WaitForSeconds(duration);

        foreach (var card in cards)
        {
            if (card == null) continue;
            Destroy(card.gameObject);
        }

        cards.Clear();
    }
    public void RecalculateCardIndex(CardDTO[] serverCards)
    {
        for(int i = 0; i < serverCards.Length; i++)
        {
            foreach(var card in cards)
            {
                if (serverCards[i].code == card.cardCode)
                {
                    card.cardOrderId = i;
                }
            }
        }
    }
    public void RemoveCardAt(int index)
    {
        if (index < 0 || index >= cards.Count)
            return;

        RemoveCard(cards[index]);
    }
    public void RemoveCard(CardDraggable card, float animDuration = 0.25f)
    {
        if (card == null || !cards.Contains(card))
            return;

        cards.Remove(card);

        Transform tr = card.transform;
        tr.DOKill();

        // Анимация удаления (вверх / вниз — выбери)
        tr.DOMoveY(tr.position.y + 150f, animDuration)
          .SetEase(Ease.InCubic);

        tr.DOScale(0f, animDuration)
          .SetEase(Ease.InCubic)
          .OnComplete(() =>
          {
              Destroy(card.gameObject);
          });

        // Пересчёт позиций оставшихся карт
        StartCoroutine(UpdateCardPositions(animDuration));
    }
    public void SelectCard(CardDraggable card)
    {
        if (card == null)
            return;

        // Если нажали ту же карту — ничего не делаем
        if (selectedCard == card)
            return;

        // Вернуть предыдущую карту
        if (selectedCard != null)
        {
            MoveCardBack(selectedCard);
        }

        // Поднять новую
        selectedCard = card;
        LiftCard(card);
    }
    private void LiftCard(CardDraggable card)
    {
        Transform tr = card.transform;
        tr.DOKill();

        tr.DOMoveY(tr.position.y + selectOffsetY, selectAnimDuration)
          .SetEase(Ease.OutCubic);
    }
    private void MoveCardBack(CardDraggable card)
    {
        selectedCard = null;
        StartCoroutine(UpdateCardPositions(selectAnimDuration));
    }
    public void ClearSelection()
    {
        if (selectedCard != null)
        {
            MoveCardBack(selectedCard);
            selectedCard = null;
        }
    }

}
