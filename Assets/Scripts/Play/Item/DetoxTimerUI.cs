using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DetoxTimerUI : MonoBehaviour
{
    private HandleDetox detoxHandler;
    private Image timerProgressBar;
    private float progress;
    private float progressIncrement;

    private void Awake()
    {
        detoxHandler = transform.parent.parent.GetComponent<HandleDetox>();
        timerProgressBar = transform.GetChild(1).GetComponent<Image>();
        progressIncrement = StaticVars.ENUM_TIME / StaticVars.DETOX_USE_TIME;
    }

    private void OnEnable()
    {
        StartCoroutine(CountDetoxTime());
    }

    private readonly WaitForSecondsRealtime waitSec = new WaitForSecondsRealtime(StaticVars.ENUM_TIME);
    IEnumerator CountDetoxTime()
    {
        progress = 0f;
        timerProgressBar.fillAmount = progress;
        while (progress < 1)
        {
            progress += progressIncrement;
            timerProgressBar.fillAmount = progress;
            yield return waitSec;
        }
        detoxHandler.DetoxUsed();
    }
}
