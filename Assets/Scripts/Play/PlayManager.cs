using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// game manage & photon communication script
public class PlayManager : MonoSingleton<PlayManager>
{
    public Tilemap[] layerGrass;
    public Tilemap[] layerWall;
    public GameObject CluePrefab;

    private int randomDropTime;
    private Vector3 randomDropPos;
    private float time = 0f;
    private int dropNum = 0;
    private int currentPlayer = 4;
    private int index;
    private Vector2[] cluePosition_layer1 = {    
        new Vector2(4.0f, 4.0f),
        new Vector2(-0.2f, -3.0f),
        new Vector2(4.0f, -10.0f),
        new Vector2(15.0f, 4.0f),
        new Vector2(-2.5f, 16.0f),
        new Vector2(-3.4f, -3.0f),
        new Vector2(-1.2f, 4.0f),
        new Vector2(12.0f, -9.0f),
        new Vector2(-8.4f, -6.0f),
        new Vector2(7.0f, -1.1f),
        new Vector2(1.8f, 1.3f),
    };
    private Vector2[] cluePosition_layer2 = { 
        new Vector2(-6.0f, 8.0f),
        new Vector2(-4.0f, 6.0f),
        new Vector2(-4.5f, -4.6f),
        new Vector2(9.3f, 7.2f),
        new Vector2(13.3f, 4.4f),
        new Vector2(6.2f, 18.4f),
        new Vector2(-7.1f, -3.1f),
        new Vector2(5.0f, 7.7f),
    };    
    
    private void Start()
    {
        randomDropTime = Random.Range(0, 10);
        SetDropPos();

        ShufflePosition(cluePosition_layer1);
        ShufflePosition(cluePosition_layer2);

        // layer 1
        index = 0;
        for (int i = 0; i < 2; i++)             // layer1�� �ִ� FAKE 2��
        {
            GameObject myInstance = Instantiate(CluePrefab);
            myInstance.transform.position = cluePosition_layer1[index];
            ClueManager cm = myInstance.GetComponent<ClueManager>();
            cm.MakeClue(ClueType.FAKE);
               // Clue = new Clue(ClueType.FAKE);
            index++;
        }

        for (int i = 0; i < 3; i++)             // layer1�� �ִ� CODE 3��
        {
            GameObject myInstance = Instantiate(CluePrefab);
            myInstance.transform.position = cluePosition_layer1[index];
            ClueManager cm = myInstance.GetComponent<ClueManager>();
            cm.MakeClue(ClueType.CODE);
            index++;
        }

        for (int i = 0; i < currentPlayer / 2; i++)             // ���� ���� �ִ� �÷��̾��� ������ ��
        {
            GameObject myInstance = Instantiate(CluePrefab);
            myInstance.transform.position = cluePosition_layer1[index];
            ClueManager cm = myInstance.GetComponent<ClueManager>();
            cm.MakeClue(ClueType.USER);
            index++;
        }

        // layer 2
        index = 0;
        for (int i = 0; i < 2; i++)             // layer2�� �ִ� FAKE 2��
        {
            GameObject myInstance = Instantiate(CluePrefab);
            myInstance.transform.position = cluePosition_layer2[index];
            myInstance.layer = LayerMask.NameToLayer("Layer 2");
            myInstance.GetComponent<SpriteRenderer>().sortingLayerName = "Layer 2";
            ClueManager cm = myInstance.GetComponent<ClueManager>();
            cm.MakeClue(ClueType.FAKE);
            index++;
        }

        for (int i = 0; i < 2; i++)             // layer2�� �ִ� CODE 2��
        {
            GameObject myInstance = Instantiate(CluePrefab);
            myInstance.transform.position = cluePosition_layer2[index];
            myInstance.layer = LayerMask.NameToLayer("Layer 2");
            myInstance.GetComponent<SpriteRenderer>().sortingLayerName = "Layer 2";
            ClueManager cm = myInstance.GetComponent<ClueManager>();
            cm.MakeClue(ClueType.CODE);
            index++;
        }

        for (int i = 0; i < currentPlayer - currentPlayer / 2; i++)             // ���� ���� �ִ� �÷��̾��� ������ ��
        {
            GameObject myInstance = Instantiate(CluePrefab);
            myInstance.transform.position = cluePosition_layer2[index];
            myInstance.layer = LayerMask.NameToLayer("Layer 2");
            myInstance.GetComponent<SpriteRenderer>().sortingLayerName = "Layer 2";
            ClueManager cm = myInstance.GetComponent<ClueManager>();
            cm.MakeClue(ClueType.USER);
            index++;
        }
    }

    private void Update()
    {
        time += Time.deltaTime;

        if(dropNum < 3 && time > randomDropTime)
        {
            var plane = ObjectPoolManager.Instance.GetObject("Plane");
            plane.transform.position = new Vector3(25.0f, randomDropPos.y, 0f);
            plane.GetComponent<Plane>().dropPos = randomDropPos;

            dropNum++;
            randomDropTime += 60;
            SetDropPos();
        }
    }

    private void SetDropPos()
    {
        randomDropPos = new Vector3(Random.Range(-3f, 10f), Random.Range(-8f, 17f), 0f);
        Debug.Log(randomDropPos);
        if (StaticFuncs.CheckOnWall(randomDropPos))
        {
            SetDropPos();
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
}

