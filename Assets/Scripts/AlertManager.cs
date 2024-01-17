using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AlertManager : MonoSingleton<AlertManager>
{
    public GameObject AlertModal;
    private TMP_Text alertTitle;
    private TMP_Text alertContent;

    // Start is called before the first frame update
    void Start()
    {
        AlertModal.SetActive(false);
        alertTitle = AlertModal.transform.GetChild(1).GetComponent<TMP_Text>();
        alertContent = AlertModal.transform.GetChild(2).GetComponent<TMP_Text>();
    }

    public void CloseBtnOnClick()
    {
        AlertModal.SetActive(false);
    }

    public void ShowAlert(string _alertTitle, string _alertContent)
    {
        alertTitle.text = _alertTitle;
        alertContent.text = _alertContent;

        AlertModal.SetActive(true);
    }
}
