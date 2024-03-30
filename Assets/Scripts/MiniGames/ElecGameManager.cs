using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ElecManager : MonoBehaviour, IPointerClickHandler
{
    private Vector3 gameProgress;

    public GameObject MiniGameManagerObj;
    private MiniGameManager gameManager;

    public GameObject ProgressBar;
    private RectTransform p_GageImg;
    private TMP_Text p_GageText;

    private void Awake()
    {
        gameManager = MiniGameManagerObj.GetComponent<MiniGameManager>();

        p_GageImg = ProgressBar.transform.GetChild(0).GetComponent<RectTransform>();
        p_GageText = ProgressBar.transform.GetChild(1).GetComponent<TMP_Text>();
        
        StartCoroutine(GageDown());
    }

    private void OnEnable()
    {
        gameProgress = Vector3.up;
        p_GageImg.localScale = gameProgress;
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
            StopAllCoroutines();
        }
        else if (gameProgress.x > 0)
        {
            p_GageImg.localScale = gameProgress;
            p_GageText.text = $"{(int)(gameProgress.x*100)}%";
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject.CompareTag("Touch"))
        {
            gameProgress.x += 0.03f;
        }
    }

    IEnumerator GageDown()
    {
        while (true)
        {
            while (gameProgress.x > 0)
            {
                gameProgress.x -= 0.01f;
                yield return StaticFuncs.WaitForSeconds(1f);
            }
        }
    }
}
