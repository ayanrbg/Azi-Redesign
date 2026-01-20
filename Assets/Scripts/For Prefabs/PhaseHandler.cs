using TMPro;
using UnityEngine;

public class PhaseHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dayTimerText;
    [SerializeField] private TextMeshProUGUI nightTimerText;
    [SerializeField] private TextMeshProUGUI voteTimerText;

    [SerializeField] private GameObject dayIcon;
    [SerializeField] private GameObject nightIcon;
    [SerializeField] private GameObject voteIcon;
    public void UpdateTimer(int seconds, string phase)
    { 
        switch (phase)
        {
            case "day":
                Timer.Instance.StartTimer(seconds, dayTimerText);
                break;
            case "night":
                Timer.Instance.StartTimer(seconds, nightTimerText);
                break;
            case "vote":
                Timer.Instance.StartTimer(seconds, voteTimerText);
                break;
        }
    }
    public void SetDayTimer(int seconds)
    {
        dayTimerText.gameObject.SetActive(true);
        nightTimerText.gameObject.SetActive(false);
        voteTimerText.gameObject.SetActive(false);

        nightIcon.SetActive(false);
        voteIcon.SetActive(false);
        dayIcon.SetActive(true);
        Timer.Instance.StartTimer(seconds, dayTimerText);
    }
    public void SetNightTimer(int seconds)
    {
        dayTimerText.gameObject.SetActive(false);
        nightTimerText.gameObject.SetActive(true);
        voteTimerText.gameObject.SetActive(false);

        dayIcon.SetActive(false);
        voteIcon.SetActive(false);
        nightIcon.SetActive(true);

        Timer.Instance.StartTimer(seconds, nightTimerText);
    }
    public void SetVoteTimer(int seconds)
    {
        dayTimerText.gameObject.SetActive(false);
        nightTimerText.gameObject.SetActive(false);
        voteTimerText.gameObject.SetActive(true);

        nightIcon.SetActive(false);
        dayIcon.SetActive(false);
        voteIcon.SetActive(true);

        Timer.Instance.StartTimer(seconds, voteTimerText);
    }
}
