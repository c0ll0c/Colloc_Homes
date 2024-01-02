using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using System.Collections;

public enum EndingType
{
    // A Homes finds Colloc in time.
    CatchColloc = 0,    // Founder Homes, Other Homes, Colloc
    // A Homes tries to catch Colloc, but fails
    FalseAlarm = 1,     // Failed Homes
    // No Homes founds Colloc in time
    TimeOver = 2,       // All Homes, Colloc
    // Errornous Ending
    Error = 3,
}

public class EndingManager : MonoBehaviour
{
    private bool result;
    private TMP_Text resultText;
    private Image resultImg;

    public void Awake()
    {
        NetworkManager.Instance.EndingManager = GetComponent<EndingManager>();
        resultText = transform.GetChild(1).GetComponent<TMP_Text>();
        resultImg = transform.GetChild(2).GetComponent<Image>();
        gameObject.SetActive(false);
    }

    public void OnDisable()
    {
        Time.timeScale = 1;
    }

    public void ShowResult(EndingType _endType, bool invoker)
    {
        Time.timeScale = 0;
        bool isHomes = NetworkManager.Instance.PlaySceneManager.gamePlayer.tag != "Colloc";
        switch (_endType)
        {
            case EndingType.CatchColloc:
                if (isHomes)
                {
                    if (invoker)
                    {
                        // win _ founder homes
                        result = true;
                    }
                    else             
                    {
                        // lose _ other homes
                        result = false;
                    }
                }
                else                
                {
                    // lose _ colloc
                    result = false;
                }
                break;
            case EndingType.FalseAlarm:
                // homes 일 수밖에 없다!
                // lose _ false alarm
                result = false;
                break;
            case EndingType.TimeOver:
                if (isHomes)
                {
                    // lose
                    result = false;
                }
                else
                {
                    // win 
                    result = true;
                }
                break;
            case EndingType.Error:
                // TODO Error type 세분화
                // 나 혼자 남았을 경우
                // 콜록이 나갔을 경우
                break;
        }

        resultText.text = (result) ? "YOU WIN!" : "YOU LOSE...";
        gameObject.SetActive(true);

        // 2초 뒤에 end
        StartCoroutine(goTo(_endType));
    }

    public IEnumerator goTo(EndingType _endType)
    {
        Debug.Log(_endType);

        yield return new WaitForSecondsRealtime(2f);

        switch (_endType)
        {
            case EndingType.CatchColloc: 
                endGame();
                break;
            case EndingType.FalseAlarm:            // 관전 or 로비 -> 버튼 뜨기
                                                   // TODO: 버튼 뜨는 걸로 바꿔야 함, 관전하기 구현
                leaveRoom();
                break;
            case EndingType.TimeOver:  
                endGame();
                break;
            case EndingType.Error:
                // TODO Error type 세분화
                break;
        }
    }

    void leaveRoom()
    {
        Time.timeScale = 1;
        PhotonNetwork.LeaveRoom();
    }
    
    void endGame()
    {
        Time.timeScale = 1;
        PhotonNetwork.LoadLevel("ReadyScene");
    }
}
