using UnityEngine;
using System.Collections;
using Photon.Pun;

public class HandleClue : MonoBehaviour
{
    public Clue clue;
    public GameObject ClueGetButton;
    public GameObject ClueHideButton;
    private bool isMine;

    private void Start()
    {
        StaticFuncs.SpriteRendering(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isMine = collision.gameObject.GetComponent<PhotonView>().IsMine;

        if (collision.gameObject.CompareTag(StaticVars.TAG_HOLMES) && clue.ClueType != ClueType.FAKE)
        {
            if (isMine)
            {
                AudioManager.Instance.PlayEffect(EffectAudioType.ENTER);
                ClueGetButton.SetActive(true);
            }
        }

        if (collision.gameObject.CompareTag(StaticVars.TAG_COLLOC) && clue.ClueType != ClueType.FAKE)
        {
            if (isMine)
            {
                AudioManager.Instance.PlayEffect(EffectAudioType.ENTER);
                ClueHideButton.SetActive(true);         
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isMine = collision.gameObject.GetComponent<PhotonView>().IsMine;

        if (isMine)
        {
            ClueGetButton.SetActive(false);
            ClueHideButton.SetActive(false);
        }
    }

    public void MakeClue(ClueType _clueType, int _index, int _typeIndex, string _nickname, string _code, string _color)
    {
        clue = new Clue(_clueType, _index, _typeIndex, _nickname, _code, _color);
    }

    public void GetClue()
    {
        AudioManager.Instance.PlayEffect(EffectAudioType.PAPER);

        if (!clue.IsHidden && !clue.IsGot)
        {
            if (clue.ClueType == ClueType.USER)
            {
                UIManager.Instance.ChangeUserClueUIText(clue.UserNickName, clue.UserCode, clue.TypeIndex, clue.color);
            }

            else if (clue.ClueType == ClueType.CODE)                // Colloc's code
            {
                string collocCode = PhotonNetwork.CurrentRoom.CustomProperties[StaticCodes.PHOTON_R_CCODE].ToString();
                UIManager.Instance.ChangeCodeClueUIText(collocCode[clue.TypeIndex]);
            }

            NetworkManager.Instance.PV.RPC("SyncHiddenCode", RpcTarget.Others, clue.Index);
            StartCoroutine(IsGotTrue());
        }

        if (clue.IsGot && !clue.IsHidden)
        {
            UIManager.Instance.ChangeClueStatusUIText("이미 획득한 단서!");
        }
        else if (clue.IsHidden)
        {
            UIManager.Instance.ChangeClueStatusUIText(StaticFuncs.RandomIndex(StaticVars.hideClueMessage));
        }
    }

    public void HideClue()
    {
        if (!clue.IsHidden)
        {
            NetworkManager.Instance.PV.RPC("SyncHiddenCode", RpcTarget.AllBuffered, clue.Index);
            UIManager.Instance.ChangeClueStatusUIText("단서 숨김!");
        }

        else
        {
            UIManager.Instance.ChangeClueStatusUIText("숨긴 단서!");
        }
    }

    private WaitForSeconds waitFor1Sec = new WaitForSeconds(1.0f);

    IEnumerator IsGotTrue()
    {
        yield return waitFor1Sec;

        clue.IsGot = true;
    }
}
