using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

// game manage & photon communication script
public class PlayManager : MonoBehaviour
{
    public Joystick Joystick;
    public Button AttackBtn;
    public Button InfectBtn;
    public GameObject ObjectManager;
    public Tilemap[] LayerGrass;
    public Tilemap[] LayerWall;
    public GameObject CluePrefab;
    public GameObject[] ClueInstances = new GameObject[16];
    public InfectProgressUI InfectProgressUI;

    private GameObject vaccineEffect;

    public Vector3[] RandomDropPos;
    public bool isVaccinated = false;
    public List<HandleCollider> ColliderList = new List<HandleCollider>();

    //private int currentPlayer = 4;
    private GameObject[] CodeClueInstances = new GameObject[5];
    private GameObject[] UserClueInstances = new GameObject[6];
    private GameObject[] FakeClueInstances = new GameObject[4];
    private int index = 0;
    private int posIndex = 0;
    private int codeIndex = 0;
    private int userIndex = 0;
    private int fakeIndex = 0;

    private TimeManager timeManager;

    // player info
    private GameObject localPlayer;
    public HandleRPC LocalRPC;
    public PhotonView LocalPV;
    public string LocalColor;

    // GameSetting
    public enum GameSettings
    {
        READY_PLAYER = 1,
        READY_USERCLUE = 2,
        READY_CLUENOTE = 4,
        READY_OTHERCLUE_FAKE = 8,
        READY_OTHERCLUE_CODE = 16,
        READY_ITEM = 32,
    }
    private int readyStatus = 0;

    private void Awake()
    {
        NetworkManager.Instance.PlaySceneManager = this;
        GameManager.Instance.EnterGame();

        timeManager = transform.GetComponent<TimeManager>();

        readyStatus = 0;
        if (PhotonNetwork.IsMasterClient)
        {
            NetworkManager.Instance.GameSetting();
        }
    }

    private void OnDestroy()
    {
        NetworkManager.Instance.PlaySceneManager = null;
    }

    public void CheckReady(GameSettings _type)
    {
        readyStatus |= (int)_type;
        if (readyStatus == 63) NetworkManager.Instance.SetPlayerSettingDone();
    }

    public void StartGame(double _endTime)
    {
        timeManager.SetEndTime(_endTime);
        UIManager.Instance.DeactivateStartPanel();
    }

    private bool canAttack = true;
    private bool canInfect = true;
    public bool TryAttack()
    {
        if (!canAttack) return false;
        // attack success -> attack cooltime activate
        canAttack = false;
        timeManager.AttackCooltime();
        return true;
    }

    public bool TryInfect()
    {
        if (!canInfect) return false;
        // attack success -> attack cooltime activate
        canInfect = false;
        timeManager.InfectCooltime();
        return true;
    }

    public void ActivateAttack() { canAttack = true; }
    public void ActivateInfect() { canInfect = true; }

    public void SpawnHomes(bool _isColloc, Vector2 _spawnPos, string _color)
    {
        localPlayer = PhotonNetwork.Instantiate("Homes_" + _color, _spawnPos, Quaternion.identity);
        LocalColor = _color;
        if (_isColloc)
        {
            localPlayer.tag = StaticVars.TAG_COLLOC;
            AttackBtn.GetComponent<RectTransform>().anchoredPosition = new Vector3(-120, 280, 0);
            UIManager.Instance.SetGameUI(StaticVars.TAG_COLLOC);
        }
        else
        {
            localPlayer.tag = StaticVars.TAG_HOLMES;
            InfectBtn.gameObject.SetActive(false);
            UIManager.Instance.SetGameUI(StaticVars.TAG_HOLMES);
        }
        LocalPV = localPlayer.GetComponent<PhotonView>();
        LocalRPC = localPlayer.GetComponent<HandleRPC>();
        vaccineEffect = gamePlayer.GetComponent<HandleRPC>().VaccineEffect;
        AttackBtn.GetComponent<AttackBtnOnClick>().PV = LocalPV;
        InfectBtn.GetComponent<AttackBtnOnClick>().PV = LocalPV;

        UIManager.Instance.LoadStartPanel(_isColloc);
        CheckReady(GameSettings.READY_PLAYER);
    }

    public void SetGame(int _dropTime, Vector3[] _dropPos)
    {
        RandomDropPos = _dropPos;
        timeManager.SetDropTime(_dropTime);
        CheckReady(GameSettings.READY_ITEM);
    }

    public void MakeOtherClueInstance(Vector2[] position, ClueType clueType, int N)
    {
        for (int i = 0; i < N; i++)
        {
            GameObject myInstance = Instantiate(CluePrefab);     
            myInstance.transform.position = position[posIndex];           
            myInstance.transform.SetParent(ObjectManager.transform);              
            HandleClue hc = myInstance.GetComponent<HandleClue>();
            posIndex++;

            if (clueType == ClueType.CODE)
            {
                hc.MakeClue(clueType, index, codeIndex, " ", " ", " ");

                CodeClueInstances[codeIndex] = myInstance;
                codeIndex++;
            }

            else if (clueType == ClueType.FAKE)
            {
                hc.MakeClue(clueType, index, fakeIndex, " ", " ", " ");

                FakeClueInstances[fakeIndex] = myInstance;
                fakeIndex++;
            }

            ClueInstances[index] = myInstance;
            index++;
        }

        if (clueType == ClueType.CODE) CheckReady(GameSettings.READY_OTHERCLUE_CODE);
        else CheckReady(GameSettings.READY_OTHERCLUE_FAKE);
    }

    public void MakeUserClueInstance(Vector2[] position, ClueType clueType, string _nickname, string _code, string _color)
    {
        GameObject myInstance = Instantiate(CluePrefab);     
        myInstance.transform.position = position[posIndex];           
        myInstance.transform.SetParent(ObjectManager.transform);              
        HandleClue hc = myInstance.GetComponent<HandleClue>();
        posIndex++;

        if (clueType == ClueType.USER)
        {
            hc.MakeClue(clueType, index, userIndex, _nickname, _code, _color);
            UserClueInstances[userIndex] = myInstance;
            userIndex++;
        }

        ClueInstances[index] = myInstance;
        index++;

        CheckReady(GameSettings.READY_USERCLUE);
    }

    public void StartVaccine()
    {
        isVaccinated = true;
        LocalRPC.VaccineEffect.SetActive(true);
        LocalRPC.VaccineEffect.GetComponent<SpriteRenderer>().sortingLayerID = localPlayer.GetComponent<SpriteRenderer>().sortingLayerID;
        AudioManager.Instance.PlayEffect(EffectAudioType.VACCINE);
        StartCoroutine(EndVaccine());
    }

    private readonly WaitForSecondsRealtime waitSec = new WaitForSecondsRealtime(StaticVars.VACCINE_TIME);
    IEnumerator EndVaccine()
    {
        yield return waitSec;
        isVaccinated = false;
        LocalRPC.VaccineEffect.SetActive(false);
        AudioManager.Instance.PlayEffect(EffectAudioType.VACCINE);
    }

    #region Manage Local Player
    public void ChangePlayerTag(string _tag)
    {
        localPlayer.tag = _tag;
    }
    public HomesController GetLocalController()
    {
        return localPlayer.transform.GetChild(2).GetComponent<HomesController>();
    }
    public bool CompareLocalTag(string _tag)
    {
        return localPlayer.CompareTag(_tag);
    }
    #endregion
}
