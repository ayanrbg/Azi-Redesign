using UnityEngine;

public class RoleToggle : MonoBehaviour
{
    [Header("Role")]
    [SerializeField] private Role role;

    [Header("Visuals")]
    [SerializeField] private GameObject checkImage;
    [SerializeField] private GameObject hoverImage;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Controller")]
    [SerializeField] private RoomRulesController rulesController;

    public Role Role => role;
    public bool IsSelected { get; private set; }
    public bool IsAvailable { get; private set; }

    private void Awake()
    {
        ForceDeselect();
        hoverImage.SetActive(false);
    }

    public void SetAvailable(bool available)
    {
        IsAvailable = available;

        canvasGroup.alpha = available ? 1f : 0.35f;
        canvasGroup.blocksRaycasts = available;

        if (!available)
            ForceDeselect();
    }

    public void Toggle()
    {
        if (!IsAvailable) return;

        IsSelected = !IsSelected;
        checkImage.SetActive(IsSelected);
        hoverImage.SetActive(IsSelected);

        rulesController.OnRoleToggled(this);
    }

    public void ForceDeselect()
    {
        IsSelected = false;
        checkImage.SetActive(false);
        hoverImage.SetActive(false);
    }
}
