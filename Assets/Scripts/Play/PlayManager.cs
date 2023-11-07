using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// game manage & photon communication script
public class PlayManager : MonoSingleton<PlayManager>
{
    public GameObject ObjectManager;
    public Tilemap[] LayerGrass;
    public Tilemap[] LayerWall;
    public GameObject CluePrefab;
    public GameObject[] ClueInstances = new GameObject[16];
    public GameObject[] CodeClueInstances = new GameObject[5];
    public GameObject[] UserClueInstances = new GameObject[4];
    public GameObject[] FakeClueInstances = new GameObject[4];

    private bool gameReady = false;
    private int randomDropTime;
    private Vector3[] randomDropPos;
    private int dropNum = 0;
    private float time = 0f;
    private int index;
    private int posIndex = 0;
    private int codeIndex = 0;
    private int userIndex = 0;
    private int fakeIndex = 0;

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

    private void Update()
    {
        if (!gameReady) return;

        time += Time.deltaTime;

        if(dropNum < 3 && time > randomDropTime)
        {
            var plane = ObjectPoolManager.Instance.GetObject("Plane");
            plane.transform.position = new Vector3(25.0f, randomDropPos[dropNum].y, 0f);
            plane.GetComponent<Plane>().dropPos = randomDropPos[dropNum];

            dropNum++;
            randomDropTime += 60;
        }
    }

    public void SpawnHomes(bool _isColloc, int _idx)
    {
        GameObject gamePlayer = PhotonNetwork.Instantiate("PhotonHomes", StaticVars.SpawnPosition[_idx], Quaternion.identity) as GameObject;
        if (_isColloc) gamePlayer.tag = "Colloc";
    }

    public void SetGame(Dictionary<string, string> _codes, int _dropTime, Vector3[] _dropPos)
    {
        randomDropTime = _dropTime;
        randomDropPos = _dropPos;
        gameReady = true;
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
                hc.MakeClue(clueType, index, codeIndex, " ", " ");

                CodeClueInstances[codeIndex] = myInstance;
                codeIndex++;
            }

            else if (clueType == ClueType.FAKE)
            {
                hc.MakeClue(clueType, index, fakeIndex, " ", " ");

                FakeClueInstances[fakeIndex] = myInstance;
                fakeIndex++;
            }

            ClueInstances[index] = myInstance;
            index++;
        }
    }

    public void MakeUserClueInstance(Vector2[] position, ClueType clueType, int N, string _nickname, string _code)
    {
        for (int i = 0; i < N; i++)
        {
            GameObject myInstance = Instantiate(CluePrefab);     
            myInstance.transform.position = position[posIndex];           
            myInstance.transform.SetParent(ObjectManager.transform);              
            HandleClue hc = myInstance.GetComponent<HandleClue>();
            posIndex++;

            if (clueType == ClueType.USER)
            {
                hc.MakeClue(clueType, index, userIndex, _nickname, _code);

                CodeClueInstances[userIndex] = myInstance;
                userIndex++;
            }

            ClueInstances[index] = myInstance;
            index++;
        }
    }
    
    private void OnDestroy()
    {
        NetworkManager.Instance.PlaySceneManager = null;
    }
}
