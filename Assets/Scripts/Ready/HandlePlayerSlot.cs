using TMPro;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

public class HandlePlayerSlot : MonoBehaviour
{
    private GameObject existingPlayer;
    private GameObject noPlayer;
    private Toggle emptySlotToggle;

    public TMP_Text PlayerNameText;
    public GameObject IsLeaderObj;
    public GameObject IsReadyObj;
    
    public GameObject PlayerImage;
    private Image playerImg;
    private SpriteLibraryAsset sprites;

    public int PlayerColor;

    private void Awake()
    {
        existingPlayer = transform.GetChild(0).gameObject;
        noPlayer = transform.GetChild(1).gameObject;
        
        playerImg = PlayerImage.GetComponent<Image>();
        sprites = PlayerImage.GetComponent<SpriteLibrary>().spriteLibraryAsset;
    }

    private void SetSlotState(bool hasPlayer)
    {
        existingPlayer.SetActive(hasPlayer);
        noPlayer.SetActive(!hasPlayer);
        emptySlotToggle = (hasPlayer) ? null : noPlayer.GetComponent<Toggle>();
    }

    public void SetSlot(PlayerData _player)
    {
        SetSlotState(true);

        PlayerNameText.text = _player.Name;

        IsLeaderObj.SetActive(_player.IsMaster);
        IsReadyObj.SetActive(!_player.IsMaster & _player.IsReady);

        SetPlayerColor(_player.Color);
    }

    public void SetEmptySlot()
    {
        SetSlotState(false);

        SetNoPlayerImg(emptySlotToggle.isOn);

        emptySlotToggle.onValueChanged.AddListener(delegate
        {
            OnToggle(emptySlotToggle.isOn);
        });
    }

    private void OnToggle(bool canJoin)
    {
        NetworkManager.Instance.SetSlotAble(transform.GetSiblingIndex(),canJoin);
    }

    public void SetNoPlayerImg(bool canJoin)
    {
        noPlayer.transform.GetChild(0).gameObject.SetActive(canJoin);
        noPlayer.transform.GetChild(1).gameObject.SetActive(!canJoin);
    }

    public void SetPlayerColor(int _color)
    {
        string color = StaticFuncs.GetColorName(_color);
        playerImg.sprite = sprites.GetSprite("Color", color);
    }
}
