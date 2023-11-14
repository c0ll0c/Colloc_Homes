using UnityEngine;
using System.Collections;
using Photon.Pun;

public class HandleClue : MonoBehaviour
{
    public Clue clue;
    public GameObject ClueGetButton;
    public GameObject ClueHideButton;

    private void Start()
    {
        StaticFuncs.SpriteRendering(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Homes") && clue.ClueType != ClueType.FAKE)
        {
            if (collision.gameObject.GetComponent<PhotonView>().IsMine)
                ClueGetButton.SetActive(true);
        }

        if (collision.gameObject.CompareTag("Colloc") && clue.ClueType != ClueType.FAKE)
        {
            if (collision.gameObject.GetComponent<PhotonView>().IsMine)
                ClueHideButton.SetActive(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PhotonView>().IsMine)
        {
            ClueGetButton.SetActive(false);
            ClueHideButton.SetActive(false);
        }
    }

    public void MakeClue(ClueType _clueType, int _index, int _typeIndex, string _nickname, string _code)
    {
        clue = new Clue(_clueType, _index, _typeIndex, _nickname, _code);
    }

    public void GetClue()
    {
        if (!clue.IsHidden && !clue.IsGot)
        {
            if (clue.ClueType == ClueType.USER)
            {
                UIManager.Instance.ChangeUserClueUIText(clue.UserNickName,
                     clue.UserCode, clue.TypeIndex);

                StartCoroutine(UnactivePanel(0));
            }

            else if (clue.ClueType == ClueType.CODE)                // Colloc's code
            {
                string collocCode = PhotonNetwork.CurrentRoom.CustomProperties["CollocCode"].ToString();
                UIManager.Instance.ChangeCodeClueUIText(collocCode[clue.TypeIndex], clue.TypeIndex);

                StartCoroutine(UnactivePanel(1));
            }
        }

        if (clue.IsGot && !clue.IsHidden)
        {
            UIManager.Instance.ChangeClueStatusUIText("�̹� ȹ���� �ܼ�!");

            StartCoroutine(UnactivePanel(2));
        }
        else if (clue.IsHidden)
        {
            UIManager.Instance.ChangeClueStatusUIText("������ �ܼ�!");

            StartCoroutine(UnactivePanel(2));
        }
    }

    public void HideClue()
    {
        if (!clue.IsHidden)
        {
            NetworkManager.Instance.PV.RPC("SyncHiddenCode", RpcTarget.AllBuffered, clue.Index);

            StartCoroutine(UnactivePanel(2));
        }

        else
        {
            UIManager.Instance.ChangeClueStatusUIText("���� �ܼ�!");
            StartCoroutine(UnactivePanel(2));
        }
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
