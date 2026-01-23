using TMPro;
using UnityEngine;

public class ExperienceManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI balanceText;
    private void OnEnable()
    {
        balanceText.text = GameState.Instance.userProfile.balance.ToString();
    }
}
