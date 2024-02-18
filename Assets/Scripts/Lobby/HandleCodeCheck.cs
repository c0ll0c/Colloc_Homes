using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HandleCodeCheck : MonoBehaviour
{
    public GameObject RoomButton;
    public TMP_InputField CodeField;

    private void OnEnable()
    {
        CodeField.text = "";
        RoomButton.GetComponentInChildren<Button>().interactable = false;
    }

    public void Close()
    {
        RoomButton.GetComponentInChildren<Button>().interactable = true;
        gameObject.SetActive(false);
    }

    public void CheckCode()
    {
        RoomButton.GetComponent<HandleRoomList>().CheckCode(CodeField.text);
    }
}
