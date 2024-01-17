using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum SlotBtnType
{
    KICK,
    BLOCK,
    ENABLE
}

public class PlayerSlotBtnOnClick : MonoBehaviour
{
    private SlotBtnType btnType = SlotBtnType.BLOCK;

    public TMP_Text btnText;

    private Toggle connectedToggle;
    private int slotIndex;

    private void Start()
    {
        connectedToggle = transform.parent.GetComponent<Toggle>();
        slotIndex = transform.parent.GetSiblingIndex();
    }

    public void ChangeType(bool _isEmpty, bool _isBlocked)
    {
        if (_isEmpty)
        {
            if (_isBlocked)
            {
                btnType = SlotBtnType.ENABLE;
                btnText.text = "Ȱ��ȭ";
            }
            else
            {
                btnType = SlotBtnType.BLOCK;
                btnText.text = "��Ȱ��ȭ";
            }
        }
        else
        {
            btnType = SlotBtnType.KICK;
            btnText.text = "�����ϱ�";
        }
    }
    public void OnClick()
    {
        switch (btnType)
        {
            case SlotBtnType.KICK:
                NetworkManager.Instance.KickPlayerOut(slotIndex);
                // ����
                break;
            case SlotBtnType.BLOCK:
                NetworkManager.Instance.SetSlotAble(slotIndex, false);
                break;
            case SlotBtnType.ENABLE:
                NetworkManager.Instance.SetSlotAble(slotIndex, true);
                break;
        }
        connectedToggle.isOn = false;
    }
}
