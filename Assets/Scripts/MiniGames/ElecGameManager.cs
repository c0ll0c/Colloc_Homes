using TMPro;
using UnityEngine;

public class ElecManager : MonoBehaviour
{
    private Vector3 gameProgress = Vector3.zero;

    public GameObject MiniGameManagerObj;
    private MiniGameManager gameManager;

    public GameObject ProgressBar;
    private RectTransform p_GageImg;
    private TMP_Text p_GageText;

    private void Start()
    {
        gameManager = MiniGameManagerObj.GetComponent<MiniGameManager>();

        p_GageImg = ProgressBar.transform.GetChild(0).GetComponent<RectTransform>();
        p_GageText = ProgressBar.transform.GetChild(1).GetComponent<TMP_Text>();
        p_GageImg.localScale = Vector3.zero;
        p_GageText.text = "0%";
    }
    private void Update()
    {
        if (gameManager.Solved) return;
        if (gameProgress.x >= 1)
        {
            gameManager.Solve();
            p_GageImg.localScale = Vector3.one;
            p_GageText.text = "DONE!";
        }
        else if (gameProgress.x > 0)
        {
            gameProgress.x -= 0.01f;
            p_GageImg.localScale = gameProgress;
            p_GageText.text = $"{(int)(gameProgress.x*100)}%";
        }
    }
}
