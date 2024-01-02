using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

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

    public void SetUI(List<PlayerData> _players, bool _isMaster)
    {
        for (int i=0; i<_players.Count; i++)
        {
            playerSlots[i].SetSlot(_players[i]);
            if (_players[i].IsLocal) 
            {
                SetColorToggle(_players[i].Color); 
                localSlotIndex = i; 
            }
        }
        for (int i = _players.Count; i < 6; i++)
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

    public void SetSlotAble(int _availableSlots)
    {
        for (int i=0; i<StaticVars.MAX_PLAYERS_PER_ROOM; i++)
        {
            if ((_availableSlots & (1 << i)) == 0)
            {
                playerSlots[i].SetNoPlayerImg(false);
            }
        }        
    }
}
