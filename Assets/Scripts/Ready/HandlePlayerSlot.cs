using UnityEngine;
using TMPro;

public class HandlePlayerSlot : MonoBehaviour
{
    public TMP_Text PlayerNameText;
    public GameObject IsLeaderObj;

    public void SetSlot(bool hasPlayer, string _nickname, bool _isLead)
    {
        if (!hasPlayer)
        {
            gameObject.SetActive(false);
            return;
        }
        gameObject.SetActive(true);
        PlayerNameText.text = _nickname;
        IsLeaderObj.SetActive(_isLead);
    }
}
