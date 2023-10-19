using UnityEngine;
using UnityEngine.UI;

public class StartBtnOnClick : MonoBehaviour
{
    // Intro Scene���� ���� ��ư ���� �� Loading Text Ȱ��ȭ ��Ű��
    // ��ư�� ��� Ŭ���� ���ϵ��� ���´�.
    public void OnClickStartBtn()
    {
        NetworkManager.Instance.Connect();
        GetComponent<Button>().interactable = false;
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);
    }
}
