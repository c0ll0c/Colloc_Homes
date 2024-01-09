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
    public int color;

    public GameObject ColorToggleDeactivatePanelObj;

    private void Awake()
    { 
        playerSlots = PlayerSlotsObj.transform.GetComponentsInChildren<HandlePlayerSlot>();
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

    // PhotonView Master
    /// 1. StartBtn Enable
    /// 2. IsMaster Badge Enable
    /// 3. Can Toggle EmptySlots
    /// 4. Can Kick Players out

    // Other Players
    /// 1. ReadyBtn Enable
    /// 2. Can Change Color
    /// 3. Can SetReady to True
    
    // All Players
    /// 1. Set Title, RoomCode

    public void SetUI(List<PlayerData> _players, int _availableSlots, bool _isMaster)
    {
        int index = 0;
        foreach(PlayerData player in _players){
            while ((_availableSlots & (1 << index)) == 0)
            {
                playerSlots[index].SetEmptySlot(true);
                index++;
            }
            playerSlots[index].SetSlot(player);
            if (player.IsLocal)
            {
                SetColorToggle(player.Color);
                localSlotIndex = index;
            }
            index++;
        }
        for (; index < StaticVars.MAX_PLAYERS_PER_ROOM; index++)
        {
            playerSlots[index].SetEmptySlot(false);
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

    public void DisactivateColorToggle(bool _isReady)
    {
        ColorToggleDeactivatePanelObj.SetActive(_isReady);
    }

    public void SetSlotAble(int _availableSlots)
    {
        for (int i=0; i<StaticVars.MAX_PLAYERS_PER_ROOM; i++)
        {
            if (playerSlots[i].hasPlayer) continue;
            playerSlots[i].SetNoPlayerImg((_availableSlots & (1 << i)) == 0);
        }
    }
}
