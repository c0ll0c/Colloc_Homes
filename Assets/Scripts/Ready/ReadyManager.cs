using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReadyManager : MonoBehaviour
{
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

    public bool FirstRendering = false;

    public TMP_Text RoomTitle;
    public TMP_Text RoomCode;

    public GameObject PlayerSlotsObj;
    private HandlePlayerSlot[] playerSlots = new HandlePlayerSlot[6];

    private int localSlotIndex = 0;

    public GameObject StartBtnObj;
    public GameObject ReadyBtnObj;

    public GameObject ColorToggleGroupObj;
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

    public void SetSlotBlock(int _availableSlots)
    {
        for (int i=0; i<StaticVars.MAX_PLAYERS_PER_ROOM; i++)
        {
            if ((_availableSlots & (1 << i)) == 0)
            {
                playerSlots[i].SetEmptySlot(true);
            }
            else
            {
                if (!playerSlots[i].IsFilled)
                {
                    playerSlots[i].SetEmptySlot(false);
                }
            }
        }
    }

    public void SetUI(string _title, string _code, List<PlayerData> _players, int _availableSlots)
    {
        bool isMaster = false;

        // Title, RoomCode UI
        RoomTitle.text = _title;
        RoomCode.text = "ROOM CODE : " + _code;

        // PlayerSlots UI
        int index = 0;
        foreach (PlayerData player in _players)
        {
            while ((_availableSlots & (1 << index)) == 0)
            {
                playerSlots[index].SetEmptySlot(true);
                index++;
            }
            playerSlots[index].SetSlot(player);
            if (player.IsLocal)
            {
                localSlotIndex = index;
                isMaster = player.IsMaster;
                FirstRendering = true;
                SetColorToggle(player.Color);
            }
            index++;
        }
        for (; index < StaticVars.MAX_PLAYERS_PER_ROOM; index++)
        {
            playerSlots[index].SetEmptySlot((_availableSlots & (1 << index)) == 0);
        }

        // Start/Ready Btn UI
        StartBtnObj.SetActive(isMaster);
        ReadyBtnObj.SetActive(!isMaster);

        // Master UI
        if (isMaster) SetMasterUI();

        // [TODO]
        // if (loadingpanel.isactive) loadingpanel 끄기
    }

    private void SetMasterUI()
    {
        for(int index=0; index<StaticVars.MAX_PLAYERS_PER_ROOM; index++)
        {
            // 자신의 슬롯은 toggle 키지 않기
            if (index == localSlotIndex) continue;

            playerSlots[index].EnableToggle();
        }

        // toggle group enable
        ToggleGroup playerSlotToggleGroup = PlayerSlotsObj.GetComponent<ToggleGroup>();
        playerSlotToggleGroup.enabled = true;
        playerSlotToggleGroup.SetAllTogglesOff();

        ColorToggleDeactivatePanelObj.SetActive(false);
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
    
    public void DisactivateColorToggle(bool _isReady)
    {
        ColorToggleDeactivatePanelObj.SetActive(_isReady);
    }

    public int GetPlayerId(int _index)
    {
        if (!playerSlots[_index].IsFilled) return -1;
        else
        {
            return playerSlots[_index].PlayerNum;
        }
    }
}
