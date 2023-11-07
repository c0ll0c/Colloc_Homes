using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DetoxTimer : MonoBehaviour
{
    public HandleDetox DetoxHandler;
    private Image timerProgressBar;

    private static readonly float enumTime = 0.5f;
    private float progress;
    private float progressIncrement;

    private void Awake()
    {
        timerProgressBar = transform.GetChild(1).GetComponent<Image>();
        progressIncrement = enumTime / StaticVars.DETOX_TIME;
    }

    private void OnEnable()
    {
        StartCoroutine(CountDetoxTime());
    }

    private readonly WaitForSecondsRealtime halfSec = new WaitForSecondsRealtime(enumTime);
    IEnumerator CountDetoxTime()
    {
        progress = 1f;
        timerProgressBar.fillAmount = progress;
        while (progress > 0)
        {
            progress -= progressIncrement;
            timerProgressBar.fillAmount = progress;
            yield return halfSec;
        }
        DetoxHandler.ActivateBooth(true);
    }
}
