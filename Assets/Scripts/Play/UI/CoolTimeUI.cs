using UnityEngine;
using UnityEngine.UI;

public class CoolTimeUI : MonoBehaviour
{
    public Image cooltimeBar;
    private GameObject cooltimeText;
    private Text text;

    private void Start()
    {
        cooltimeBar = transform.GetChild(0).GetComponent<Image>();
        cooltimeText = UIManager.Instance.CoolTimeText;
        text = cooltimeText.transform.GetComponent<Text>();

        Init_UI();
        cooltimeBar.fillAmount = 0;
    }

    private void Init_UI()
    {
        cooltimeBar.type = Image.Type.Filled;
        cooltimeBar.fillMethod = Image.FillMethod.Radial360;
        cooltimeBar.fillOrigin = (int)Image.Origin360.Top;
        cooltimeBar.fillClockwise = false;
    }

    public void SetCoolTimeBar(float _fill)
    {
        int sec = 15 - (int)(_fill*15);
        if (cooltimeBar != null)
        {
            cooltimeBar.fillAmount = 1 - _fill;
            if (sec > 0)
            {
                cooltimeText.SetActive(true);
                text.text = sec.ToString();
            }
            else
            {
                cooltimeText.SetActive(false);
            }
        }
    }
}
