using UnityEngine;
using UnityEngine.Tilemaps;

// game manage & photon communication script
public class PlayManager : MonoSingleton<PlayManager>
{
    public static PlayManager Instance;
    public GameObject ClueManager;
    public Tilemap[] layer;
    public GameObject CluePrefab;

    private Vector2[] cluePosition_layer1 = StaticVars.cluePosition_layer1;
    private Vector2[] cluePosition_layer2 = StaticVars.cluePosition_layer2;

    public GameObject[] ClueInstances = new GameObject[16];
    public GameObject[] CodeClueInstances = new GameObject[5];
    public GameObject[] UserClueInstances = new GameObject[4];

    private int[] randomDropTime = new int[3];
    private float time = 0f;
    private int layerIndex = 0;
    private int currentPlayer = 4;
    private int layerNum;  
    
    private int index;             
    private int codeIndex = 0;              
    private int userIndex = 0;              

    // [TODO] Syncronize Make Clue Instace exclude ShufflePosition : Move to GameSetting
    private void Start()
    {
        ShufflePosition(cluePosition_layer1);
        ShufflePosition(cluePosition_layer2);

        // layer 1
        layerNum = 0;
        MakeClueInstance(cluePosition_layer1, ClueType.FAKE, 2, "Layer 1");
        MakeClueInstance(cluePosition_layer1, ClueType.CODE, 3, "Layer 1");
        MakeClueInstance(cluePosition_layer1, ClueType.USER, currentPlayer/2, "Layer 1");

        // layer 2
        layerNum = 0;
        MakeClueInstance(cluePosition_layer2, ClueType.FAKE, 2, "Layer 2");
        MakeClueInstance(cluePosition_layer2, ClueType.CODE, 2, "Layer 2");
        MakeClueInstance(cluePosition_layer2, ClueType.USER, currentPlayer/2, "Layer 2");
    }

    private void Init()
    {
        for (int i = 0; i < 3; i++)
        {
            randomDropTime[i] = Random.Range(0, 10);
        }
    }

    private void Update()
    {
        time += Time.deltaTime;
        if(layerIndex < 3 && time > randomDropTime[layerIndex])
        {
            var plane = ObjectPoolManager.Instance.GetObject("Plane");
            plane.transform.position = new Vector3(25.0f, Random.Range(-8.0f, 17.0f), 0f);
            layerIndex++;
        }
    }

    private void ShufflePosition(Vector2[] position)
    {
        System.Random rand = new System.Random();

        for (int i = position.Length - 1; i > 0; i--)
        {
            int index = rand.Next(i + 1);

            Vector2 temp = position[index];
            position[index] = position[i];
            position[i] = temp;
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
            myInstance.transform.SetParent(ClueManager.transform);              
            HandleClue hc = myInstance.GetComponent<HandleClue>(); 
            hc.MakeClue(clueType);
            layerNum++;

            ClueInstances[index] = myInstance;
            index++;

            if (clueType == ClueType.CODE)
            {
                CodeClueInstances[codeIndex] = myInstance;
                codeIndex++;
                Debug.Log("code" + codeIndex);
            }

            else if (clueType == ClueType.USER)
            {
                UserClueInstances[userIndex] = myInstance;
                userIndex++;
                Debug.Log("user" + userIndex);
            }
        }
    }
}
