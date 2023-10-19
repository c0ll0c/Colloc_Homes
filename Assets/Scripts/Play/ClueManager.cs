using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ܼ� �����տ� �پ� ����
// ���� �߿��� ���� �߻�

// �ܼ� �����, �ܼ� ȹ���ϱ�
// ������ �ν��Ͻ��� �پ� �ִ� Clue�� �޾ƿ�

public class ClueManager : MonoBehaviour
{
    public Clue Clue;
    public GameObject ClueButton;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Homes") && Clue.ClueType != ClueType.FAKE)
        {
            ClueButton.SetActive(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        ClueButton.SetActive(false);
    }

}
