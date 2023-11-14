using System.Collections;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public GameObject TimeCanvasObj;
    private TimeCanvasUI timeCanvas;

    private double gameLeftTime;
    private double vaccineDropTime;
    private int vaccineNum = 0;

    private void Start()
    {
        timeCanvas = TimeCanvasObj.GetComponent<TimeCanvasUI>();
    }

    public void SetPlayTime(double _time, double _dropTime)
    {
        gameLeftTime = _time - NetworkManager.Instance.GetServerTime();
        vaccineDropTime = _dropTime;
        StartCoroutine(ManageGameTime());
    }

    private static readonly float enumTime = 0.5f;
    private readonly WaitForSecondsRealtime halfSec = new WaitForSecondsRealtime(enumTime);
    IEnumerator ManageGameTime()
    {
        while (gameLeftTime > 0)
        {
            yield return halfSec;

            // Manage Total Game Time
            gameLeftTime -= enumTime;
            timeCanvas.SetTime(gameLeftTime);

            // Manage Vaccine Drop Time
            vaccineDropTime -= enumTime;
            if (vaccineNum < 3 && vaccineDropTime < 0)
            {
                DropVaccine();
                vaccineDropTime += StaticVars.VACCINE_DROP_INTERVAL;
            }
        }
    }

    private void DropVaccine()
    {
        // drop vaccine;
        GameObject plane = ObjectPoolManager.Instance.GetObject("Plane");
        plane.GetComponent<Plane>().InitiateDrop(vaccineNum);

        vaccineNum++;
    }
}
