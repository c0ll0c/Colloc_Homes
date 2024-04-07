using UnityEngine;
using UnityEngine.EventSystems;

public class HungryGameManager : MonoBehaviour, IDragHandler
{
    private float gameProgress;
    private float progressIncrement = 1f / 5;

    public GameObject MiniGameManagerObj;
    private MiniGameManager gameManager;

    public GameObject ProgressBar;
    private ProgressBarUI progressBarUI;

    private void Awake()
    {
        gameManager = MiniGameManagerObj.GetComponent<MiniGameManager>();

        progressBarUI = ProgressBar.GetComponent<ProgressBarUI>();
    }

    private void OnEnable()
    {
        gameProgress = 0f;
    }

    void Update()
    {
        if (gameManager.Solved) return;
        if (gameProgress >= 1)
        {
            gameManager.Solve();
            progressBarUI.ProgressDone("DONE!");
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // vertical movement 만 읽어서 홈즈 움직이기
        throw new System.NotImplementedException();
    }

    public void EatFood()
    {
        if (gameProgress + progressIncrement < 1)
        {
            gameProgress += progressIncrement;
            progressBarUI.ChangeGage(progressIncrement);
        }
        else
        {
            progressBarUI.ProgressDone("DONE!");
            gameManager.Solve();
        }
    }

    public void EatTrash()
    {
        gameProgress -= progressIncrement;
        if (gameProgress < 0) gameProgress = 0;
        progressBarUI.ChangeGage(-progressIncrement);
    }
}
