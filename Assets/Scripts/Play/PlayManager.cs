using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ܼ� �ν��Ͻ� ���� (CODE: layer1 - 3�� / layer2 - 2��, USER: �ִ� 6, FAKE: layer1 - 2�� / layer2 - 2��)

// �ܼ� ���� ������ �ֱ�
// �ܼ� Ÿ�� -> �ܼ� ����
// �ܼ� ��ġ (���� ������)

// �� 15���� �����Ǿ�� ��

public class PlayManager : MonoBehaviour
{
    public GameObject CluePrefab;
    private Vector2[] cluePosition_layer1 = {       // �ּ� 8��
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
    };          // �ּ� 7��

    private int currentPlayer = 4;
    private int index;


    private void Start()
    {
        ShufflePosition(cluePosition_layer1);
        ShufflePosition(cluePosition_layer2);

        // layer 1
        index = 0;
        for (int i = 0; i < 2; i++)             // layer1�� �ִ� FAKE 2��
        {
            GameObject myInstance = Instantiate(CluePrefab);
            myInstance.transform.position = cluePosition_layer1[index];
            ClueManager cm = myInstance.GetComponent<ClueManager>();
            cm.Clue = new Clue(ClueType.FAKE);
            index++;
        }

        for (int i = 0; i < 3; i++)             // layer1�� �ִ� CODE 3��
        {
            GameObject myInstance = Instantiate(CluePrefab);
            myInstance.transform.position = cluePosition_layer1[index];
            ClueManager cm = myInstance.GetComponent<ClueManager>();
            cm.Clue = new Clue(ClueType.CODE);
            index++;
        }

        for (int i = 0; i < currentPlayer / 2; i++)             // ���� ���� �ִ� �÷��̾��� ������ ��
        {
            GameObject myInstance = Instantiate(CluePrefab);
            myInstance.transform.position = cluePosition_layer1[index];
            ClueManager cm = myInstance.GetComponent<ClueManager>();
            cm.Clue = new Clue(ClueType.USER);
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
            cm.Clue = new Clue(ClueType.FAKE);
            index++;
        }

        for (int i = 0; i < 2; i++)             // layer2�� �ִ� CODE 2��
        {
            GameObject myInstance = Instantiate(CluePrefab);
            myInstance.transform.position = cluePosition_layer2[index];
            myInstance.layer = LayerMask.NameToLayer("Layer 2");
            myInstance.GetComponent<SpriteRenderer>().sortingLayerName = "Layer 2";
            ClueManager cm = myInstance.GetComponent<ClueManager>();
            cm.Clue = new Clue(ClueType.CODE);
            index++;
        }

        for (int i = 0; i < currentPlayer - currentPlayer / 2; i++)             // ���� ���� �ִ� �÷��̾��� ������ ��
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

