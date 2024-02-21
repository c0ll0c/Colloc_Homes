using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;

public class InfectProgressUI : MonoBehaviour
{
    public bool CollocWinAble;

    private int playerCount;
    private int currentInfected;
    private GameObject timer;
    private Image progressBar;

    void Awake()
    {
        CollocWinAble = false;
        progressBar = transform.GetChild(1).gameObject.GetComponent<Image>();
        progressBar.fillAmount = 0;
        timer = transform.GetChild(2).gameObject;
        timer.SetActive(false);
        playerCount = PhotonNetwork.CurrentRoom.PlayerCount - 1;
        currentInfected = 0;
    }

    public void UpdateProgress(bool infect)
    {
        if (infect)
        {
            currentInfected++;
        }
        else
        {
            currentInfected--;
        }
        progressBar.fillAmount = (float)currentInfected / playerCount;

        StartCollocTimer();
    }

    public void StartCollocTimer()
    {
        if (Equals(currentInfected, playerCount) && CollocWinAble)
        {
            timer.SetActive(true);
        }
        else
        {
            timer.SetActive(false);
        }
    }
}
