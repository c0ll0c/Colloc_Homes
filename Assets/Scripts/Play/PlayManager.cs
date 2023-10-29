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
    private int currentPlayer = 4;
    private int index;
    private int layerNum;
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

    // [TODO] Syncronize Make Clue Instace exclude ShufflePosition : Move to GameSetting
    private void Start()
    {
        ShufflePosition(StaticVars.cluePosition_layer1);
        ShufflePosition(StaticVars.cluePosition_layer2);

        // layer 1
        layerNum = 0;
        MakeClueInstance(StaticVars.cluePosition_layer1, ClueType.FAKE, 2, "Layer 1");
        MakeClueInstance(StaticVars.cluePosition_layer1, ClueType.CODE, 3, "Layer 1");
        MakeClueInstance(StaticVars.cluePosition_layer1, ClueType.USER, currentPlayer/2, "Layer 1");

        // layer 2
        layerNum = 0;
        MakeClueInstance(StaticVars.cluePosition_layer2, ClueType.FAKE, 2, "Layer 2");
        MakeClueInstance(StaticVars.cluePosition_layer2, ClueType.CODE, 2, "Layer 2");
        MakeClueInstance(StaticVars.cluePosition_layer2, ClueType.USER, currentPlayer/2, "Layer 2");
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

    private void ShufflePosition(Vector2[] _position)
    {
        System.Random rand = new System.Random();

        for (int i = _position.Length - 1; i > 0; i--)
        {
            int index = rand.Next(i + 1);

            Vector2 temp = _position[index];
            _position[index] = _position[i];
            _position[i] = temp;
        }
    }

    void MakeClueInstance(Vector2[] position, ClueType clueType, int N, string Layer)
    {
        for (int i = 0; i < N; i++)
        {
            GameObject myInstance = Instantiate(CluePrefab);     
            myInstance.transform.position = position[layerNum];           
            myInstance.layer = LayerMask.NameToLayer(Layer);     
            myInstance.GetComponent<SpriteRenderer>().sortingLayerName = Layer;
            myInstance.transform.SetParent(ObjectManager.transform);              
            HandleClue hc = myInstance.GetComponent<HandleClue>(); 
            layerNum++;

            if (clueType == ClueType.CODE)
            {
                hc.MakeClue(clueType, index, codeIndex);

                CodeClueInstances[codeIndex] = myInstance;
                codeIndex++;
                Debug.Log("code" + codeIndex);
            }

            else if (clueType == ClueType.USER)
            {
                hc.MakeClue(clueType, index, userIndex);

                UserClueInstances[userIndex] = myInstance;
                userIndex++;
                Debug.Log("user" + userIndex);
            }

            else if (clueType == ClueType.FAKE)
            {
                hc.MakeClue(clueType, index, fakeIndex);

                FakeClueInstances[fakeIndex] = myInstance;
                fakeIndex++;
                Debug.Log("user" + userIndex);
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
