using UnityEngine;
using TMPro;

public class HandleNicknameText : MonoBehaviour
{
    private TMP_Text tmpNickname;
    void Start()
    {
        tmpNickname = transform.GetChild(0).GetComponent<TMP_Text>();
        tmpNickname.text = GameManager.Instance.PlayerName;
    }
}
