using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AlertManager : MonoSingleton<AlertManager>
{
    public GameObject AlertModal;
    private GameObject warningIcon;
    private TMP_Text alertTitle;
    private TMP_Text alertContent;

    // Start is called before the first frame update
    void Start()
    {
        AlertModal.SetActive(false);
        warningIcon = AlertModal.transform.GetChild(0).GetChild(1).gameObject;
        alertTitle = AlertModal.transform.GetChild(0).GetChild(3).GetComponent<TMP_Text>();
        alertContent = AlertModal.transform.GetChild(0).GetChild(4).GetComponent<TMP_Text>();
    }

    public void CloseBtnOnClick()
    {
        AlertModal.SetActive(false);
    }

    public void ShowAlert(string _alertTitle, string _alertContent)
    {
        warningIcon.SetActive(false);
        alertTitle.gameObject.SetActive(true);
        alertTitle.text = _alertTitle;
        alertContent.text = _alertContent;

        AlertModal.SetActive(true);
    }

    public void WarnAlert(string _alertContent)
    {
        warningIcon.SetActive(true);
        alertTitle.gameObject.SetActive(false);
        alertContent.text = _alertContent;

        AlertModal.SetActive(true);
    }
}
