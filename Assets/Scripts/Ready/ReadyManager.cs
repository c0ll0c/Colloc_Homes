using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadyManager : MonoBehaviour
{
    public GameObject PlayerSlotsObj;
    private HandlePlayerSlot[] playerSlots = new HandlePlayerSlot[6];
    private int localSlotIndex = 0;

    public GameObject StartBtnObj;
    public GameObject ReadyBtnObj;

    public GameObject ColorToggleGroupObj;
    private ToggleGroup colorToggleGroup;
    public int color;

    private void Awake()
    { 
        playerSlots = PlayerSlotsObj.transform.GetComponentsInChildren<HandlePlayerSlot>();
        colorToggleGroup = ColorToggleGroupObj.GetComponent<ToggleGroup>();
    }

    private void Start()
    {
        NetworkManager.Instance.ReadySceneManager = this;
        NetworkManager.Instance.SyncPlayersData();
    }

    private void OnDestroy()
    {
        NetworkManager.Instance.ReadySceneManager = null;
    }

    public void SetUI(List<PlayerData> _players, int _playerNum, bool _isMaster)
    {
        for (int i=0; i<_playerNum; i++)
        {
            playerSlots[i].SetSlot(_players[i]);
            if (_players[i].IsLocal) 
            {
                SetColorToggle(_players[i].Color); 
                localSlotIndex = i; 
            }
        }
        for (int i = _playerNum; i < 6; i++)
        {
            playerSlots[i].SetEmptySlot();
        }

        StartBtnObj.SetActive(_isMaster);
        ReadyBtnObj.SetActive(!_isMaster);
    }

    public void SetColorToggle(int _color)
    {
        int index= 0;
        while (_color > 1)
        {
            _color >>= 1;
            ++index;
        }

        Toggle toggle = ColorToggleGroupObj.transform.GetChild(index).GetComponent<Toggle>();
        toggle.isOn = true;
    }

    public void ChangeLocalColor(int _color)
    {
        color = _color;
        playerSlots[localSlotIndex].SetPlayerColor(_color);
    }
}
