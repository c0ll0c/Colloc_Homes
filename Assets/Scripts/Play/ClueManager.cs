using UnityEngine;

// 단서 프리팹에 붙어 있음
// 이 스크립트에서 모든 함수를 만들고 PlayManager는 호출하기만...

// 단서 숨기기, 단서 획득하기
// 각각의 인스턴스에 붙어 있는 Clue를 받아옴

public class ClueManager : MonoBehaviour
{
    private Clue clue;
    public GameObject ClueButton;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Homes") && clue.ClueType != ClueType.FAKE)
        {
            ClueButton.SetActive(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        ClueButton.SetActive(false);
    }

    public void MakeClue(ClueType _clueType)
    {
        clue = new Clue(_clueType);
    }

}
