using UnityEngine;
using UnityEngine.UI;

public class EventSolutionHandler : MonoBehaviour
{
    private ShowButtonOnCollision showBtnOnCollisionScript;
    public SpriteRenderer SolutionSprite;
    public Sprite EventActive;
    public Sprite EventDeactive;
    
    public void SetClickable()
    {
        SolutionSprite.sprite = EventActive;

        showBtnOnCollisionScript = GetComponent<ShowButtonOnCollision>();
        showBtnOnCollisionScript.enabled = true;
        showBtnOnCollisionScript.ButtonToShow.GetComponentInChildren<Button>().onClick.RemoveAllListeners();
        showBtnOnCollisionScript.ButtonToShow.GetComponentInChildren<Button>().onClick.AddListener(delegate {
            NetworkManager.Instance.EventToPlay.MiniGame();
        });
    }

    public void CloseGame()
    {
        NetworkManager.Instance.EventToPlay.CloseMiniGame(false);
        //showBtnOnCollisionScript.CanvasToShow.SetActive(false);
    }
    public void EventSolved()
    {
        NetworkManager.Instance.EventToPlay.CloseMiniGame(true);
        showBtnOnCollisionScript.ButtonToShow.SetActive(false);
        //showBtnOnCollisionScript.CanvasToShow.SetActive(false);
        SolutionSprite.sprite = EventDeactive;
        showBtnOnCollisionScript.enabled = false;
    }
}
