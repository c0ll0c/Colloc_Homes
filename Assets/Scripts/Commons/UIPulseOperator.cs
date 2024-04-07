using System.Collections;
using UnityEngine;

public class UIPulseOperator : MonoBehaviour
{
    private RectTransform uiToHandle;
    [SerializeField] float growthBound = 1.1f;
    [SerializeField] float shrinkBound = 0.9f;
    [SerializeField] float pulseSpeed = 0.01f;

    private Vector3 originalScale;
    private void OnEnable()
    {
        uiToHandle = GetComponent<RectTransform>();
        StopAllCoroutines();
        originalScale = uiToHandle.localScale;
        StartCoroutine(Pulse());
    }

    IEnumerator Pulse()
    {
        float currentRatio = 1;
        while (true)
        {
            while (currentRatio < growthBound)
            {
                currentRatio = Mathf.MoveTowards(currentRatio, growthBound, pulseSpeed);

                uiToHandle.localScale = originalScale * currentRatio;
                yield return StaticFuncs.WaitForSeconds(0.05f);
            }
            while (currentRatio > shrinkBound)
            {
                currentRatio = Mathf.MoveTowards(currentRatio, shrinkBound, pulseSpeed);
                uiToHandle.localScale = originalScale * currentRatio;
                yield return StaticFuncs.WaitForSeconds(0.05f);
            }
        }
    }
}
