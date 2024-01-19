using UnityEngine;
using UnityEngine.UI;

public class ColorHandler : MonoBehaviour
{
    public int SelectionColor;
    private Toggle colorToggle;
    private Image toggleImg;
    private GameObject checkObj;

    private void Start()
    {
        colorToggle = GetComponent<Toggle>();
        colorToggle.onValueChanged.AddListener(delegate
        {
            OnToggle(colorToggle.isOn);
        });

        toggleImg = GetComponent<Image>();

        checkObj = transform.GetChild(0).gameObject;
        checkObj.SetActive(false);
    }

    private void OnToggle(bool _isOn)
    {
        if (!NetworkManager.Instance.ReadySceneManager.FirstRendering)
        {
            return;
        }
        if (_isOn)
        {
            NetworkManager.Instance.ReadySceneManager.ChangeLocalColor(SelectionColor);
            toggleImg.color = Color.gray;
            checkObj.SetActive(true);
        }
        else
        {
            toggleImg.color = Color.white;
            checkObj.SetActive(false);

        }
    }
}
