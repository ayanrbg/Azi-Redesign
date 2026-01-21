using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Splines;
using UnityEngine;

public class HandView : MonoBehaviour
{
    [SerializeField] private SplineContainer splineContainer;
    public List<CardDraggable> cards = new();

    public IEnumerator AddCard(CardDraggable cardView, string id, Sprite sprite)
    {
        cardView.Init(id);
        cards.Add(cardView);
        yield return UpdateCardPositions(0.15f);
    }

    public void UpdateCards(float duration)
    {
        StartCoroutine(UpdateCardPositions(duration));
    }
    public IEnumerator UpdateCardPositions(float duration)
    {
        if (cards.Count == 0) yield break;
        float cardSpacing = 1f / 4f;
        float firstCardPosition = 0.5f - (cards.Count - 1) * cardSpacing / 2;

        Spline spline = splineContainer.Spline;
        for (int i = 0; i < cards.Count; i++)
        {
            float p = firstCardPosition + i * cardSpacing;
            Vector3 splinePosition = splineContainer.EvaluatePosition(p);
            Vector3 forward = splineContainer.EvaluateTangent(p);
            Vector3 up = splineContainer.EvaluateUpVector(p);
            Quaternion rotation = Quaternion.LookRotation(-up, Vector3.Cross(-up, forward).normalized);
            cards[i].transform.DOMove(splinePosition + transform.position + 0.1f * i * Vector3.back, duration);
            cards[i].transform.DORotate(rotation.eulerAngles, duration);
        }
        yield return new WaitForSeconds(duration);
    }
}
