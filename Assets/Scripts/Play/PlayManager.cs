using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

// game manage & photon communication script
public class PlayManager : MonoSingleton<PlayManager>
{
    public GameObject ObjectManager;
    public GameObject gamePlayer;
    public Tilemap[] LayerGrass;
    public Tilemap[] LayerWall;
    public GameObject CluePrefab;
    public GameObject[] ClueInstances = new GameObject[16];

    public GameObject DetoxObj;
    private HandleDetox[] detoxHandlers = new HandleDetox[2];

    public TimeManager TimeManager;

    public Vector3[] RandomDropPos;
    public bool isVaccinated = false;

    //private int currentPlayer = 4;
    private GameObject[] CodeClueInstances = new GameObject[5];
    private GameObject[] UserClueInstances = new GameObject[6];
    private GameObject[] FakeClueInstances = new GameObject[4];
    private int index = 0;
    private int posIndex = 0;
    private int codeIndex = 0;
    private int userIndex = 0;
    private int fakeIndex = 0;

    // private double gameEndTime;

    protected override void Awake()
    {
        base.Awake();

        NetworkManager.Instance.PlaySceneManager = this;
        GameManager.Instance.EnterGame();
        
        if (PhotonNetwork.IsMasterClient)
        {
            NetworkManager.Instance.GameSetting();
        }
    }

    // [TODO] Syncronize Make Clue Instace exclude ShufflePosition : Move to GameSetting
    private void Start()
    {
        detoxHandlers = DetoxObj.GetComponentsInChildren<HandleDetox>();
    }

    public void SpawnHomes(bool _isColloc, Vector2 _spawnPos, string _color)
    {
        gamePlayer = PhotonNetwork.Instantiate("Homes_" + _color, _spawnPos, Quaternion.identity) as GameObject;
        if (_isColloc)
        {
            gamePlayer.tag = "Colloc";
            UIManager.Instance.SetGameUI("Colloc");
        }
        else
        {
            gamePlayer.tag = "Homes";
            UIManager.Instance.SetGameUI("Homes");
        }
    }

    public void SetGame(int _dropTime, Vector3[] _dropPos, double _endTime)
    {
        RandomDropPos = _dropPos;
        TimeManager.SetPlayTime(_endTime, _dropTime);
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
    }
    
    private void OnDestroy()
    {
        NetworkManager.Instance.PlaySceneManager = null;
    }
}
