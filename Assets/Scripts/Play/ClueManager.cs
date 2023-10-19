using UnityEngine;

// �ܼ� �����տ� �پ� ����
// �� ��ũ��Ʈ���� ��� �Լ��� ����� PlayManager�� ȣ���ϱ⸸...

// �ܼ� �����, �ܼ� ȹ���ϱ�
// ������ �ν��Ͻ��� �پ� �ִ� Clue�� �޾ƿ�

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
