using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadingText : MonoBehaviour
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

    private readonly WaitForSeconds halfSec = new WaitForSeconds (0.5f);
    private readonly string[] loadingText = { "Loading", "Loading.", "Loading..", "Loading..." };
    private IEnumerator StartTextAnimation()
    {
        foreach(string _text in loadingText)
        {
            Loading.text = _text;
            yield return halfSec;
        }
    }
}
