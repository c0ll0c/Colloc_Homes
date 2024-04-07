using UnityEngine;
using UnityEngine.EventSystems;

public class ElecManager : MonoBehaviour, IPointerClickHandler
{
    private float gameProgress;

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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject.CompareTag("Touch"))
        {
            gameProgress += 0.03f;
            progressBarUI.ChangeGage(0.03f);

            if (gameProgress >= 1)
            {
                gameManager.Solve();
                progressBarUI.ProgressDone("DONE!");
            }
        }
    }
}
