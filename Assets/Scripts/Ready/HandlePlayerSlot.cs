using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.U2D.Animation;

public class HandlePlayerSlot : MonoBehaviour
{
    public TMP_Text PlayerNameText;
    public GameObject IsLeaderObj;
    public GameObject IsReadyObj;
    
    public GameObject PlayerImage;
    private Image playerImg;
    private SpriteLibraryAsset sprites;

    public int PlayerColor;

    private void Start()
    {
        playerImg = PlayerImage.GetComponent<Image>();
        sprites = PlayerImage.GetComponent<SpriteLibrary>().spriteLibraryAsset;
    }

    public void SetSlot(PlayerData _player)
    {
        gameObject.SetActive(true);
        PlayerNameText.text = _player.Name;

        IsLeaderObj.SetActive(_player.IsMaster);
        IsReadyObj.SetActive(!_player.IsMaster & _player.IsReady);

        SetPlayerColor(_player.Color);
    }

    public void SetEmptySlot()
    {
        gameObject.SetActive(false);
    }

    public void SetPlayerColor(int _color)
    {
        string color = StaticFuncs.GetColorName(_color);
        playerImg.sprite = sprites.GetSprite("Color", color);
    }
}
