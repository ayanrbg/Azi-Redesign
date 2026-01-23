using UnityEngine;
using DG.Tweening;

public class RotateObject : MonoBehaviour
{
    [Tooltip("Ось и скорость вращения (градусов за полный оборот)")]
    public Vector3 rotation = new Vector3(0, 0, 360);

    [Tooltip("Время одного полного оборота (сек)")]
    public float duration = 1f;

    [Tooltip("Вращение в локальных координатах")]
    public bool localRotation = true;

    private Tween rotateTween;

    void OnEnable()
    {
        StartRotation();
    }

    void OnDisable()
    {
        rotateTween?.Kill();
    }

    public void StartRotation()
    {
        rotateTween?.Kill();

        rotateTween = transform
            .DOLocalRotate(new Vector3(0, 0, 360), duration, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1);

    }

    public void StopRotation()
    {
        rotateTween?.Kill();
        rotateTween = null;
    }
}
