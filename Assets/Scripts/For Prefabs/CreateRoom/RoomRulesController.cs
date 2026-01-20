using UnityEngine;
using TMPro;
using System.Linq;

public class RoomRulesController : MonoBehaviour
{
    [Header("Texts")]
    [SerializeField] private TMP_Text mafiaText;
    [SerializeField] private TMP_Text activeRolesText;

    [Header("UI")]
    [SerializeField] private RangePlayersSlide playersSlider;
    [SerializeField] private RoleToggle[] roleToggles;

    private RoomRules currentRules;

    private void OnEnable()
    {
        ApplyRules(playersSlider.MinPlayers);
    }

    // 🔹 вызывается из RangePlayersSlider
    public void OnPlayersCountChanged()
    {
        ApplyRules(playersSlider.MinPlayers);
    }

    // 🔹 вызывается из RoleToggle
    public void OnRoleToggled(RoleToggle toggledRole)
    {
        int selectedCount = roleToggles.Count(t => t.IsSelected);

        // ❗ превышен лимит активных ролей
        if (selectedCount > currentRules.maxActiveCitizens)
        {
            // снимаем САМУЮ ПЕРВУЮ выбранную роль (кроме текущей)
            RoleToggle toRemove = roleToggles
                .First(t => t != toggledRole && t.IsSelected);

            toRemove.ForceDeselect();
        }

        UpdateRolesText();
    }

    private void ApplyRules(int minPlayers)
    {
        if (minPlayers < 6)
            minPlayers = 6;

        currentRules = RoomRulesConfig.GetRules(minPlayers);

        mafiaText.text = $"Мафия: {currentRules.mafiaCount}";
        UpdateRolesText();

        foreach (var toggle in roleToggles)
        {
            bool available = currentRules.availableRoles.Contains(toggle.Role);
            toggle.SetAvailable(available);
        }
    }

    private void UpdateRolesText()
    {
        int selectedCount = roleToggles.Count(t => t.IsSelected);
        activeRolesText.text =
            $"Активные роли: {selectedCount}/{currentRules.maxActiveCitizens}";
    }

    // 🔹 ВСЕГДА АКТУАЛЬНО
    public string[] GetSelectedRoles()
    {
        return roleToggles
            .Where(t => t.IsSelected)
            .Select(t => t.Role.ToString())
            .ToArray();
    }

    public int GetMafiaCount()
    {
        return currentRules.mafiaCount;
    }
}
