using System.Collections;
using TMPro;
using UnityEngine;

public class CollocWinTimeUI : MonoBehaviour
{
    private TextMeshProUGUI timer;
    private int time;

    private void Awake()
    {
        timer = GetComponent<TextMeshProUGUI>();
        time = StaticVars.COLLOC_WIN_TIME;
    }

    private void OnEnable()
    {
        timer.text = string.Empty;
        time = StaticVars.COLLOC_WIN_TIME;
        StartCoroutine(CountCollocWinTime());
    }

    private readonly WaitForSecondsRealtime waitSec = new WaitForSecondsRealtime(1.0f);
    IEnumerator CountCollocWinTime()
    {
        while (time >= 0)
        {
            timer.text = time.ToString();
            yield return waitSec;
            time--;
        }
        NetworkManager.Instance.EndingManager.ShowResult(EndingType.TimeOver, true, string.Empty);
    }
}
