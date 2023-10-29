using UnityEngine;
using System.Collections;

public class HandleClue : MonoBehaviour
{
    private Clue clue;
    public GameObject ClueGetButton;
    public GameObject ClueHideButton;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Homes") && clue.ClueType != ClueType.FAKE)
        {
            ClueGetButton.SetActive(true);
        }

        if (collision.gameObject.CompareTag("Colloc") && clue.ClueType != ClueType.FAKE)
        {
            ClueHideButton.SetActive(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        ClueGetButton.SetActive(false);
        ClueHideButton.SetActive(false);
    }

    public void MakeClue(ClueType _clueType, int _index, int _typeIndex)
    {
        clue = new Clue(_clueType, _index, _typeIndex);
    }

    public void GetClue()
    {
        if (!clue.IsHidden && !clue.IsGot)
        {
            if (clue.ClueType == ClueType.USER)
            {
                UIManager.Instance.ChangeUserClueUIText("User Name: " + clue.TypeIndex,
                     "User Code: " + clue.TypeIndex, clue.TypeIndex);
                
                StartCoroutine(UnactivePanel(0));
            }

            else if (clue.ClueType == ClueType.CODE)
            {
                UIManager.Instance.ChangeCodeClueUIText("Code " + clue.TypeIndex, clue.TypeIndex);

                StartCoroutine(UnactivePanel(1));
            }
        }

        if (clue.IsGot && !clue.IsHidden)
        {
            UIManager.Instance.ChangeClueStatusUIText("�̹� ȹ���� �ܼ�!");

            StartCoroutine(UnactivePanel(3));
        }
        else if (clue.IsHidden)
        {
            UIManager.Instance.ChangeClueStatusUIText("������ �ܼ�!");

            StartCoroutine(UnactivePanel(3));
        }
    }

    public void HideClue()
    {
        UIManager.Instance.ChangeClueStatusUIText("�ܼ� ����!");

        // [TODO] connect NetworkManager.cs in Runtime -> �ּ� ����
        // NetworkManager.Instance.PV.RPC("SyncHiddenCode", Photon.Pun.RpcTarget.Other, PlayManager.Instance.ClueInstances[indexOfClueInstance].GetComponent<HandleClue>().clue.IsHidden, true);
        clue.IsHidden = true;

        StartCoroutine(UnactivePanel(2));
    }

    private WaitForSeconds waitFor1Sec = new WaitForSeconds(1.0f);
    IEnumerator UnactivePanel(int index)
    {
        yield return waitFor1Sec;

        UIManager.Instance.UnactivePanel(index);

        if (!clue.IsHidden)
            clue.IsGot = true;
    }
}
