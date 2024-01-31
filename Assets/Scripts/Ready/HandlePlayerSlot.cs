using TMPro;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

public class HandlePlayerSlot : MonoBehaviour
{
    public bool IsFilled;
    public bool IsBlocked;
    public int PlayerNum;

    Toggle btnActiveToggle;
    // Child 0
    private GameObject playerObj;
    private TMP_Text playerName;
    private GameObject isLeaderObj;
    private GameObject isLocalObj;
    private GameObject isReadyObj;
    private Image homesImg;
    private SpriteLibraryAsset sprites;
    // Child 1
    private GameObject noPlayerObj;
    // Child 2
    private GameObject btnObj;
    private PlayerSlotBtnOnClick btnHandler;

    private void Awake()
    {
        IsFilled = false;
        IsBlocked = false;

        btnActiveToggle = GetComponent<Toggle>();
        btnActiveToggle.interactable = false;

        // Child 0
        playerObj = transform.GetChild(0).gameObject;
        playerName = playerObj.transform.GetChild(1).GetComponent<TMP_Text>();
        isLeaderObj = playerObj.transform.GetChild(2).gameObject;
        isLocalObj = playerObj.transform.GetChild(3).gameObject;
        isReadyObj = playerObj.transform.GetChild(4).gameObject;
        homesImg = playerObj.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        sprites = playerObj.transform.GetChild(0).GetChild(0).GetComponent<SpriteLibrary>().spriteLibraryAsset;

        // Child 1
        noPlayerObj = transform.GetChild(1).gameObject;

        // Child 2
        btnObj = transform.GetChild(2).gameObject;
        btnHandler = btnObj.GetComponent<PlayerSlotBtnOnClick>();
    }

    private void SetSlotState(bool _hasPlayer)
    {
        IsFilled = _hasPlayer;
        playerObj.SetActive(_hasPlayer);
        noPlayerObj.SetActive(!_hasPlayer);
    }

    public void SetSlot(PlayerData _player)
    {
        SetSlotState(true); ;
        PlayerNum = _player.Id;
        playerName.text = _player.Name;

        isLeaderObj.SetActive(_player.IsMaster);
        isLocalObj.SetActive(_player.IsLocal);
        isReadyObj.SetActive(!_player.IsMaster & _player.IsReady);

        SetPlayerColor(_player.Color);
    }

    public void SetEmptySlot(bool _blocked)
    {
        SetSlotState(false);
        PlayerNum = -1;

        IsBlocked = _blocked;

        // [TO MODIFY]
        noPlayerObj.transform.GetChild(0).gameObject.SetActive(!_blocked);
        noPlayerObj.transform.GetChild(1).gameObject.SetActive(_blocked);
    }

    public void SetPlayerColor(int _color)
    {
        homesImg.sprite = sprites.GetSprite("Color", StaticFuncs.GetColorName(_color));
    }

    public void EnableToggle()
    {
        btnActiveToggle.interactable = true;
        btnActiveToggle.onValueChanged.AddListener(delegate
        {
            OnToggle(btnActiveToggle.isOn);
        });
    }

    private void OnToggle(bool _isOn)
    {
        if (_isOn)
        {
            btnHandler.ChangeType(!IsFilled, IsBlocked);
        }
        btnObj.SetActive(_isOn);
    }
}
