using System.Collections;
using UnityEngine;
using TMPro;

public class LoadingTextUI : MonoBehaviour
{
    public TMP_Text Loading;
    private void OnEnable()
    {
        StartCoroutine(StartTextAnimation());
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private readonly string[] loadingText = { "로딩 중", "로딩 중.", "로딩 중..", "로딩 중..." };
    private IEnumerator StartTextAnimation()
    {
        while (true)
        {
            foreach (string _text in loadingText)
            {
                Loading.text = _text;
                yield return StaticFuncs.WaitForSeconds(0.5f);
            }
        }
    }
}
