using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HandleClue : MonoBehaviour
{
    private Clue clue;
    public GameObject ClueGetButton;
    public GameObject ClueHideButton;

    private Transform cluePanelCanvas;
    private Transform clueUIButton;

    private int indexOfClueInstance;

    private void Start()
    {
        cluePanelCanvas = UIManager.Instance.CluePanelCanvas.transform;
        clueUIButton = UIManager.Instance.ClueUIButton.transform;

        indexOfClueInstance = System.Array.IndexOf(PlayManager.Instance.ClueInstances, gameObject);

        for (int i = 0; i < 4; i++)
        {
            cluePanelCanvas.GetChild(i).gameObject.SetActive(false);
        }
    }

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

    public void MakeClue(ClueType _clueType)
    {
        clue = new Clue(_clueType);
    }

    public void GetClue()
    {
        if (!clue.IsHidden && !clue.IsGot)
        {
            if (clue.ClueType == ClueType.USER)
            {
                cluePanelCanvas.GetChild(0).GetChild(0).GetComponent<Text>().text =
                    "User Name: " + System.Array.IndexOf(PlayManager.Instance.UserClueInstances, gameObject).ToString();
                cluePanelCanvas.GetChild(0).GetChild(1).GetComponent<Text>().text =
                    "User Code: " + System.Array.IndexOf(PlayManager.Instance.UserClueInstances, gameObject).ToString();

                clueUIButton.GetChild(0).GetChild(System.Array.IndexOf(PlayManager.Instance.UserClueInstances, gameObject)).GetChild(0).GetComponent<Text>().text =
                    "UserName: " + System.Array.IndexOf(PlayManager.Instance.UserClueInstances, gameObject).ToString();
                clueUIButton.GetChild(0).GetChild(System.Array.IndexOf(PlayManager.Instance.UserClueInstances, gameObject)).GetChild(1).GetComponent<Text>().text =
                    "UserCode: " + System.Array.IndexOf(PlayManager.Instance.UserClueInstances, gameObject).ToString();

                cluePanelCanvas.GetChild(0).gameObject.SetActive(true);

                StartCoroutine(UnactivePanel(0));
            }

            else if (clue.ClueType == ClueType.CODE)
            {
                cluePanelCanvas.GetChild(1).GetChild(0).GetComponent<Text>().text =
                    "Code " + System.Array.IndexOf(PlayManager.Instance.CodeClueInstances, gameObject).ToString();

                clueUIButton.GetChild(1).GetChild(System.Array.IndexOf(PlayManager.Instance.CodeClueInstances, gameObject)).GetChild(0).GetComponent<Text>().text =
                    System.Array.IndexOf(PlayManager.Instance.CodeClueInstances, gameObject).ToString();

                cluePanelCanvas.GetChild(1).gameObject.SetActive(true);

                StartCoroutine(UnactivePanel(1));
            }
        }

        if (clue.IsGot && !clue.IsHidden)
        {
            cluePanelCanvas.GetChild(3).GetChild(0).GetComponent<Text>().text = "¿ÃπÃ »πµÊ«— ¥‹º≠!";
            cluePanelCanvas.GetChild(3).gameObject.SetActive(true);
            StartCoroutine(UnactivePanel(3));
        }
        else if (clue.IsHidden)
        {
            cluePanelCanvas.GetChild(3).GetChild(0).GetComponent<Text>().text = "º˚∞‹¡¯ ¥‹º≠!";
            cluePanelCanvas.GetChild(3).gameObject.SetActive(true);
            StartCoroutine(UnactivePanel(3));
        }
    }

    public void HideClue()
    {
        cluePanelCanvas.GetChild(2).gameObject.SetActive(true);
        
        Debug.Log(indexOfClueInstance + " π¯¬∞ ¥‹º≠ º˚±Ë");


        // [TODO] connect NetworkManager.cs in Runtime -> ¡÷ºÆ «ÿ¡¶
        // NetworkManager.Instance.PV.RPC("syncHiddenCode", Photon.Pun.RpcTarget.Other, PlayManager.Instance.ClueInstances[indexOfClueInstance].GetComponent<HandleClue>().clue.IsHidden, true);
        clue.IsHidden = true;

        StartCoroutine(UnactivePanel(2));
    }

    IEnumerator UnactivePanel(int index)
    {
        yield return new WaitForSeconds(1.0f);

        cluePanelCanvas.GetChild(index).gameObject.SetActive(false);

        if (!clue.IsHidden)
            clue.IsGot = true;
    }
}
