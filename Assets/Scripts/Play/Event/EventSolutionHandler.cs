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
        // ���� ��� �ޱ�
        // Ȩ�� �¿�� ������ �������� ���� 5�� �Ա�?
    }
    public void SolutionFog()
    {
        // ��ǳ�� ������
        // �̴� ��ǳ�� ȭ�鿡�� �����̸� ���� ������Ʈ ġ���?
    }
    public void SolutionElec()
    {
        // ������ 30�� ������
    }
    public void SolutionSaveNPC()
    {
        // �� ���� �� ���� �ε�� ����?
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
