using TMPro;
using UnityEngine;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private float initProgress = 0f;
    [SerializeField] private bool showText = true;
    private float gageSpeed = 0.001f;

    private RectTransform gageBar;
    private TMP_Text gageText;
    private Vector3 progress = Vector3.up;
    private float nextProgress = 0f;

    void Awake()
    {
        gageBar = transform.GetChild(0).GetComponent<RectTransform>();
        gageText = transform.GetChild(1).GetComponent<TMP_Text>();
        if (!showText) transform.GetChild(1).gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        progress.x = initProgress;
        nextProgress = initProgress;
        gageBar.localScale = progress;
        gageText.text = $"{(int)(progress.x * 100)}%";
    }

    private void Update()
    {
        if (progress.x == nextProgress) return;

        progress.x = Mathf.MoveTowards(progress.x, nextProgress, gageSpeed);
        gageBar.localScale = progress;
        gageText.text = $"{(int)(progress.x * 100)}%";
    }

    public void ChangeGage(float _amount)
    {
        nextProgress += _amount;
        if (nextProgress > 1) nextProgress = 1f;
        else if (nextProgress < 0) nextProgress = 0f;
    }

    public void ProgressDone(string _doneMessage)
    {
        nextProgress = 1;
        gageText.text = _doneMessage;
    }
}
