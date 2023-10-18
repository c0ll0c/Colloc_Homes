using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StartBtnController : MonoBehaviour
{
    public void OnClickStartBtn()
    {
        NetworkManager.Instance.Connect();
        GetComponent<Button>().interactable = false;
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);
    }
}
