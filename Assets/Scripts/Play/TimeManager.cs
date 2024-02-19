using Photon.Pun;
using System.Collections;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public GameObject TimerObj;
    public GameObject InfectCooltimeObj;
    public GameObject AttackCooltimeObj;
    private TimeCanvasUI timerUI;
    private CoolTimeUI InfectCoolTimeUI;
    private CoolTimeUI AttackCoolTimeUI;

    private double gameLeftTime = 0;
    private double vaccineDropTime;
    private int vaccineNum = 0;
    private bool notice = false;

    public static bool gameStart = false;
    public static bool NPCTime = false;
    public GameObject EndingCanvasObj;
    private EndingManager endingManager;

    private void Start()
    {
        timerUI = TimerObj.GetComponent<TimeCanvasUI>();

        InfectCoolTimeUI = InfectCooltimeObj.GetComponent<CoolTimeUI>();
        AttackCoolTimeUI = AttackCooltimeObj.GetComponent<CoolTimeUI>(); 

        endingManager = EndingCanvasObj.GetComponent<EndingManager>();
    }

    private void Update()
    {
        if (gameLeftTime <= 0)
        {
            if (!EndingCanvasObj.activeSelf && gameStart)
            {
                endingManager.ShowResult(EndingType.TimeOver, true);
            }
            return;
        }

        if (gameLeftTime < StaticVars.NPC_TIME)
            NPCTime = true;

        if(NPCTime && !notice)
        {
            StartCoroutine(NpcTime());
        }

        gameLeftTime -= Time.deltaTime;
        timerUI.SetTime(gameLeftTime);

        vaccineDropTime -= Time.deltaTime;
        if (vaccineNum < 3 && vaccineDropTime < 0)
        {
            DropVaccine();
            vaccineDropTime += StaticVars.VACCINE_DROP_INTERVAL;
        }
    }

    IEnumerator NpcTime()
    {
        notice = true;
        UIManager.Instance.NoticeText.text = "지금부터 고발이 가능합니다!";
        UIManager.Instance.NoticePanelObj.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        UIManager.Instance.NoticePanelObj.SetActive(false);
    }

    public void SetDropTime(double _dropTime)
    {
        vaccineDropTime = _dropTime;
    }

    public void SetEndTime(double _endTime)
    {
        gameLeftTime = _endTime - NetworkManager.Instance.GetServerTime();
        gameStart = true;
    }

    private void DropVaccine()
    {
        // drop vaccine
        GameObject plane = ObjectPoolManager.Instance.GetObject("Plane");
        plane.GetComponent<Plane>().InitiateDrop(vaccineNum);

        vaccineNum++;
    }

    public void AttackCooltime()
    {
        StartCoroutine(AttackCooltimeBar());
    }

    static float incrementTime = 0.25f;
    static float incrementProg = incrementTime / StaticVars.ATTACK_TIME;

    private IEnumerator AttackCooltimeBar()
    {
        float prog = 0;
        while (prog < 1)
        {
            yield return StaticFuncs.WaitForSeconds(incrementTime);
            prog += incrementProg;
            InfectCoolTimeUI.SetCoolTimeBar(prog);
            AttackCoolTimeUI.SetCoolTimeBar(prog);
        }

        NetworkManager.Instance.PlaySceneManager.ActivateAttack();
    }
}
