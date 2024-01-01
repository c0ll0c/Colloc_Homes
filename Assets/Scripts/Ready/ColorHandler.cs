using UnityEngine;
using UnityEngine.UI;

public class ColorHandler : MonoBehaviour
{
    public int Color;
    private Toggle colorToggle;

    private void Start()
    {
        colorToggle = GetComponent<Toggle>();
        colorToggle.onValueChanged.AddListener(delegate
        {
            OnToggle(colorToggle);
        });
    }

    private void OnToggle(Toggle change)
    {
        if (change.isOn)
        {
            NetworkManager.Instance.ReadySceneManager.ChangeLocalColor(Color);
        }
    }
}
