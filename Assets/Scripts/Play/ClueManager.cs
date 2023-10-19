using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 단서 프리팹에 붙어 있음
// 게임 중에서 변경 발생

// 단서 숨기기, 단서 획득하기
// 각각의 인스턴스에 붙어 있는 Clue를 받아옴

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
