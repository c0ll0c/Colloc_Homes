using UnityEngine;
using UnityEngine.UI;

public class ColorHandler : MonoBehaviour
{
    //public int Color;
    public int SelectionColor;
    private Toggle colorToggle;
    private Image toggleImg;

    private void Start()
    {
        colorToggle = GetComponent<Toggle>();
        colorToggle.onValueChanged.AddListener(delegate
        {
            OnToggle(colorToggle);
        });

        toggleImg = GetComponent<Image>();
    }

    private Color deepGray = new Color(0.2f, 0.2f, 0.2f);
    private void OnToggle(Toggle change)
    {
        if (change.isOn)
        {
            NetworkManager.Instance.ReadySceneManager.ChangeLocalColor(SelectionColor);
            toggleImg.color = Color.cyan;
        }
        else
        {
            toggleImg.color = deepGray;
        }
    }
}
