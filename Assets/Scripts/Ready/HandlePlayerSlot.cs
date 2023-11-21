using UnityEngine;
using TMPro;

public class HandlePlayerSlot : MonoBehaviour
{
    public TMP_Text PlayerNameText;
    public GameObject IsLeaderObj;
    public GameObject IsReadyObj;

    public void SetSlot(bool hasPlayer, string _nickname, bool _isLead, bool _isReady)
    {
        if (!hasPlayer)
        {
            gameObject.SetActive(false);
            return;
        }
        gameObject.SetActive(true);
        PlayerNameText.text = _nickname;

        IsLeaderObj.SetActive(_isLead);
        if (!_isLead)
        {
            IsReadyObj.SetActive(_isReady);
        }
    }
}
