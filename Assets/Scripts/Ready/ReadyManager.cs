using System.Collections.Generic;
using UnityEngine;

public class ReadyManager : MonoBehaviour
{
    public GameObject PlayerSlotsObj;
    private HandlePlayerSlot[] playerSlots = new HandlePlayerSlot[6];

    public GameObject StartBtnObj;
    public GameObject ReadyBtnObj;

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

    public void SetUI(List<PlayerData> _players, int _playerNum, bool _isMaster)
    {
        for (int i=0; i<_playerNum; i++)
        {
            playerSlots[i].SetSlot(true, _players[i].Name, _players[i].IsMaster, _players[i].IsReady);
        }
        for (int i = _playerNum; i < 6; i++)
        {
            playerSlots[i].SetSlot(false, string.Empty, false, false);
        }

        StartBtnObj.SetActive(_isMaster);
        ReadyBtnObj.SetActive(!_isMaster);
    }
}
