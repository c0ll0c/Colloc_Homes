using UnityEngine;
using TMPro;

public class TimeCanvasUI : MonoBehaviour
{
    public TMP_Text MainTimer;

    public void SetTime(double _time)
    {
        MainTimer.text = FormatTime((int)_time);
    }

    public string FormatTime(int _time)
    {
        return string.Format("{0:0}:{1:00}", _time / 60, _time % 60);
    }
}
