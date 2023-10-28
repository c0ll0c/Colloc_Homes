using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    public GameObject CluePanelCanvas;
    public GameObject ClueUIButton;

    private void Start()
    {
        ClueUIButton.SetActive(false);
    }
}
