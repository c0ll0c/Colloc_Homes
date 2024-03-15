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
        showBtnOnCollisionScript.ButtonToShow.GetComponentInChildren<Button>().onClick.AddListener(delegate {
            // Opening the minigame panel is done by ShowButtonOnCollision.cs
            NetworkManager.Instance.EventToPlay.MiniGame();
        });
    }

    public void SolutionHungry()
    {
        // 음식 배급 받기
        // 홈즈 좌우로 움직여 떨어지는 음식 5개 먹기?
    }
    public void SolutionFog()
    {
        // 선풍기 돌리기
        // 미니 선풍기 화면에서 움직이며 구름 오브젝트 치우기?
    }
    public void SolutionElec()
    {
        // 발전기 30번 때리기
    }
    public void SolutionSaveNPC()
    {
        // 돌 전부 세 번씩 두드려 깨기?
        // EventSolved() Call
    }

    public void CloseGame()
    {
        NetworkManager.Instance.EventToPlay.CloseMiniGame(false);
        showBtnOnCollisionScript.CanvasToShow.SetActive(false);
    }
    public void EventSolved()
    {
        NetworkManager.Instance.EventToPlay.CloseMiniGame(true);
        showBtnOnCollisionScript.ButtonToShow.SetActive(false);
        showBtnOnCollisionScript.CanvasToShow.SetActive(false);
        SolutionSprite.sprite = EventDeactive;
        showBtnOnCollisionScript.enabled = false;
    }
}
