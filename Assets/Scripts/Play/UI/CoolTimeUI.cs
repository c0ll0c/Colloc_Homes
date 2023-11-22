using UnityEngine;
using UnityEngine.UI;

public class CoolTimeUI : MonoBehaviour
{
    private Image cooltimeBar;

    private void Start()
    {
        cooltimeBar = transform.GetChild(1).GetComponent<Image>();
        cooltimeBar.fillAmount = 1;
    }

    public void SetCoolTimeBar(float _fill)
    {
        cooltimeBar.fillAmount = _fill;
    }
}
