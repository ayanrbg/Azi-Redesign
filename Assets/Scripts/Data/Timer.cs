using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public static Timer Instance;

    private Coroutine timerCoroutine;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void StartTimer(int seconds, TMP_Text tmpText)
    {
        StartInternal(seconds, tmpText, null, null);
    }
    public void StartTimer(int seconds, TMP_Text tmpText, string textBefore, string textAfter)
    {
        StartInternal(seconds, tmpText, textBefore, textAfter);
    }
    public void StopTimer()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }
    }

    private void StartInternal(int seconds, TMP_Text tmpText, string textBefore, string textAfter)
    {
        StopTimer();
        timerCoroutine = StartCoroutine(TimerCoroutine(seconds,  tmpText, textBefore, textAfter));
    }
    private IEnumerator TimerCoroutine(int seconds, TMP_Text tmpText, string textBefore, string textAfter)
    {
        int timeLeft = seconds;

        while (timeLeft >= 0)
        {
            string value = timeLeft.ToString();

            tmpText.text = textBefore + value + textAfter;

            yield return new WaitForSeconds(1f);
            timeLeft--;
        }
    }
}
