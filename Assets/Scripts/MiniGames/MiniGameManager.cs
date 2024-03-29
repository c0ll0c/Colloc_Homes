using System.Collections;
using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    public bool Solved = false;
    public GameObject SolvedPanelObj;

    public void OnCloseBtnClick()
    {
        if (Solved) NetworkManager.Instance.PlaySceneManager.EventToPlay.Solution.EventSolved();
        else NetworkManager.Instance.PlaySceneManager.EventToPlay.Solution.CloseGame();
    }

    public void Solve()
    {
        Solved = true;
        SolvedPanelObj.SetActive(true);
        StartCoroutine(CloseIn3Secs());
    }

    IEnumerator CloseIn3Secs()
    {
        yield return StaticFuncs.WaitForSeconds(3f);
        OnCloseBtnClick();
    }
}
