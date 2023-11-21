using System.Collections;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public GameObject TimeCanvasObj;
    private TimeCanvasUI timeCanvas;

    [SerializeField] private GameObject coolTimeUI;
    private bool attackActivated = false;

    private double gameLeftTime = 0;
    private double vaccineDropTime;
    private int vaccineNum = 0;

    private void Start()
    {
        timeCanvas = TimeCanvasObj.GetComponent<TimeCanvasUI>();
    }

    private void Update()
    {
        if (gameLeftTime <= 0) return;
        gameLeftTime -= Time.deltaTime;
        timeCanvas.SetTime(gameLeftTime);
        vaccineDropTime -= Time.deltaTime;
        if (vaccineNum < 3 && vaccineDropTime < 0)
        {
            DropVaccine();
            vaccineDropTime += StaticVars.VACCINE_DROP_INTERVAL;
        }
    }

    public void SetPlayTime(double _time, double _dropTime)
    {
        vaccineDropTime = _dropTime;

        gameLeftTime = _time - NetworkManager.Instance.GetServerTime();
    }

    private void DropVaccine()
    {
        // drop vaccine
        GameObject plane = ObjectPoolManager.Instance.GetObject("Plane");
        plane.GetComponent<Plane>().InitiateDrop(vaccineNum);

        vaccineNum++;
    }

    public bool IsAttackActivated()
    {
        if (attackActivated) return true;
        attackActivated = true;
        StartCoroutine(AttackCoolTime());
        coolTimeUI.GetComponent<CoolTimeUI>().Active = true;
        return false;
    }

    private IEnumerator AttackCoolTime()
    {
        yield return StaticFuncs.WaitForSeconds(StaticVars.ATTACK_TIME);
        attackActivated = false;
    }
}
