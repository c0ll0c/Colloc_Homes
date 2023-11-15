using UnityEngine;
using UnityEngine.UI;

public class CoolTimeUI : MonoBehaviour
{
    public bool Active = false; // cannot sync with timeManager variable

    private Image cooltimeBar;
    private float time = 0;
    private void Start()
    {
        cooltimeBar = transform.GetChild(1).GetComponent<Image>();
        cooltimeBar.fillAmount = 1;
    }

    private void Update()
    {
        if (Active) {
            cooltimeBar.fillAmount = time / StaticVars.ATTACK_TIME;
            time += Time.deltaTime;

            if (time > StaticVars.ATTACK_TIME)
            {
                Active = false;
                cooltimeBar.fillAmount = 1;
                time = 0;
            }
        }
    }
}
