using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 단서 인스턴스 생성 (CODE: layer1 - 3개 / layer2 - 2개, USER: 최대 6, FAKE: layer1 - 2개 / layer2 - 2개)

// 단서 상태 정의해 주기
// 단서 타입 -> 단서 내용
// 단서 위치 (랜덤 포지션)

// 총 15개가 생성되어야 함

public class PlayManager : MonoBehaviour
{
    public GameObject CluePrefab;
    private Vector2[] cluePosition_layer1 = {       // 최소 8개
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
    };          // 최소 7개

    private int currentPlayer = 4;
    private int index;


    private void Start()
    {
        ShufflePosition(cluePosition_layer1);
        ShufflePosition(cluePosition_layer2);

        // layer 1
        index = 0;
        for (int i = 0; i < 2; i++)             // layer1에 있는 FAKE 2개
        {
            GameObject myInstance = Instantiate(CluePrefab);
            myInstance.transform.position = cluePosition_layer1[index];
            ClueManager cm = myInstance.GetComponent<ClueManager>();
            cm.Clue = new Clue(ClueType.FAKE);
            index++;
        }

        for (int i = 0; i < 3; i++)             // layer1에 있는 CODE 3개
        {
            GameObject myInstance = Instantiate(CluePrefab);
            myInstance.transform.position = cluePosition_layer1[index];
            ClueManager cm = myInstance.GetComponent<ClueManager>();
            cm.Clue = new Clue(ClueType.CODE);
            index++;
        }

        for (int i = 0; i < currentPlayer / 2; i++)             // 현재 들어와 있는 플레이어의 숫자의 반
        {
            GameObject myInstance = Instantiate(CluePrefab);
            myInstance.transform.position = cluePosition_layer1[index];
            ClueManager cm = myInstance.GetComponent<ClueManager>();
            cm.Clue = new Clue(ClueType.USER);
            index++;
        }

        // layer 2
        index = 0;
        for (int i = 0; i < 2; i++)             // layer2에 있는 FAKE 2개
        {
            GameObject myInstance = Instantiate(CluePrefab);
            myInstance.transform.position = cluePosition_layer2[index];
            myInstance.layer = LayerMask.NameToLayer("Layer 2");
            myInstance.GetComponent<SpriteRenderer>().sortingLayerName = "Layer 2";
            ClueManager cm = myInstance.GetComponent<ClueManager>();
            cm.Clue = new Clue(ClueType.FAKE);
            index++;
        }

        for (int i = 0; i < 2; i++)             // layer2에 있는 CODE 2개
        {
            GameObject myInstance = Instantiate(CluePrefab);
            myInstance.transform.position = cluePosition_layer2[index];
            myInstance.layer = LayerMask.NameToLayer("Layer 2");
            myInstance.GetComponent<SpriteRenderer>().sortingLayerName = "Layer 2";
            ClueManager cm = myInstance.GetComponent<ClueManager>();
            cm.Clue = new Clue(ClueType.CODE);
            index++;
        }

        for (int i = 0; i < currentPlayer - currentPlayer / 2; i++)             // 현재 들어와 있는 플레이어의 숫자의 반
        {
            GameObject myInstance = Instantiate(CluePrefab);
            myInstance.transform.position = cluePosition_layer2[index];
            myInstance.layer = LayerMask.NameToLayer("Layer 2");
            myInstance.GetComponent<SpriteRenderer>().sortingLayerName = "Layer 2";
            ClueManager cm = myInstance.GetComponent<ClueManager>();
            cm.Clue = new Clue(ClueType.USER);
            index++;
        }

    }

    void ShufflePosition(Vector2[] position)
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

