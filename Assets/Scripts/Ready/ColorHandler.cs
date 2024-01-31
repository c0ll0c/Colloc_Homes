using UnityEngine;
using UnityEngine.UI;

public class ColorHandler : MonoBehaviour
{
    public int SelectionColor;
    private Toggle colorToggle;
    private Image toggleImg;

    private void Start()
    {
        colorToggle = GetComponent<Toggle>();
        colorToggle.onValueChanged.AddListener(delegate
        {
            OnToggle(colorToggle.isOn);
        });

        toggleImg = GetComponent<Image>();
    }

    private void OnToggle(bool _isOn)
    {
        if (!NetworkManager.Instance.ReadySceneManager.FirstRendering)
        {
            return;
        }
        if (_isOn)
        {
            NetworkManager.Instance.ChangeLocalColor(SelectionColor);
            toggleImg.color = Color.gray;
        }
        else
        {
            toggleImg.color = Color.white;

        }
    }
}
