using UnityEngine;
using DG.Tweening;

public class CardToCenterAnimator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Canvas canvas;

    [Header("Animation Settings")]
    [SerializeField] private float duration = 0.6f;
    [SerializeField] private Vector3 endRotation = Vector3.zero;
    [SerializeField] private Ease ease = Ease.OutCubic;

    public void Animate(RectTransform target)
    {
        if (target == null || canvas == null)
            return;

        RectTransform canvasRect = canvas.transform as RectTransform;

        // Центр канваса в мировых координатах
        Vector3 canvasCenterWorld =
            canvasRect.TransformPoint(canvasRect.rect.center);

        Sequence seq = DOTween.Sequence();

        // Движение
        seq.Join(
            target.DOMove(canvasCenterWorld, duration)
                  .SetEase(ease)
        );

        // Поворот одновременно
        seq.Join(
            target.DORotate(endRotation, duration)
                  .SetEase(ease)
        );
    }
}
