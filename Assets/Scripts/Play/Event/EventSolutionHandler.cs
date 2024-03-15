using UnityEngine;
using UnityEngine.UI;

public class EventSolutionHandler : MonoBehaviour
{
    private bool isTargetEvent;

    public Button MiniGameButton;
    public Image SolutionImg;
    public Sprite EventActive;
    public Sprite EventDeactive;
    
    public void SetClickable()
    {
        SolutionImg.sprite = EventActive;
        isTargetEvent = true;
        MiniGameButton.onClick.AddListener(delegate {
            NetworkManager.Instance.EventToPlay.MiniGame();
        });
    }

    public void SolutionHungry()
    {

    }
    public void SolutionFog()
    {

    }
    public void SolutionElec()
    {

    }
    public void SolutionSaveNPC()
    {

    }

    public void CloseGame()
    {
        NetworkManager.Instance.EventToPlay.CloseMiniGame(false);
    }
    public void EventSolved()
    {
        SolutionImg.sprite = EventDeactive;
        NetworkManager.Instance.EventToPlay.CloseMiniGame(true);
    }
}
