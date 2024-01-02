using TMPro;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

public class HandlePlayerSlot : MonoBehaviour
{
    public bool hasPlayer;
    public bool blocked;

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
        hasPlayer = false;
        blocked = false;

        existingPlayer = transform.GetChild(0).gameObject;
        noPlayer = transform.GetChild(1).gameObject;
        
        playerImg = PlayerImage.GetComponent<Image>();
        sprites = PlayerImage.GetComponent<SpriteLibrary>().spriteLibraryAsset;
    }

    private void SetSlotState(bool _hasPlayer)
    {
        hasPlayer = _hasPlayer;
        existingPlayer.SetActive(_hasPlayer);
        noPlayer.SetActive(!_hasPlayer);
        emptySlotToggle = (_hasPlayer) ? null : noPlayer.GetComponent<Toggle>();
    }

    public void SetSlot(PlayerData _player)
    {
        SetSlotState(true);

        PlayerNameText.text = _player.Name;

        IsLeaderObj.SetActive(_player.IsMaster);
        IsReadyObj.SetActive(!_player.IsMaster & _player.IsReady);

        SetPlayerColor(_player.Color);
    }

    public void SetEmptySlot(bool _blocked)
    {
        SetSlotState(false);
        SetNoPlayerImg(_blocked);
        emptySlotToggle.onValueChanged.AddListener(delegate
        {
            OnToggle();
        });
    }

    private void OnToggle()
    {
        bool res = NetworkManager.Instance.SetSlotAble(transform.GetSiblingIndex(), blocked);
        if (res) blocked = !blocked;
    }

    public void SetNoPlayerImg(bool _blocked)
    {
        noPlayer.transform.GetChild(0).gameObject.SetActive(!_blocked);
        noPlayer.transform.GetChild(1).gameObject.SetActive(_blocked);
    }

    public void SetPlayerColor(int _color)
    {
        string color = StaticFuncs.GetColorName(_color);
        playerImg.sprite = sprites.GetSprite("Color", color);
    }
}
