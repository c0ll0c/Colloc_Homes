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
    public GameObject falseEnding;

    public void Awake()
    {
        NetworkManager.Instance.EndingManager = GetComponent<EndingManager>();
        resultText = transform.GetChild(1).GetComponent<TMP_Text>();
        resultImg = transform.GetChild(2).GetComponent<Image>();
        gameObject.SetActive(false);
        falseEnding.SetActive(false);
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
                        AudioManager.Instance.ChangeBGM(GameState.WIN);
                    }
                    else             
                    {
                        // lose _ other homes
                        result = false;
                        AudioManager.Instance.ChangeBGM(GameState.LOSE);
                    }
                }
                else                
                {
                    // lose _ colloc
                    result = false;
                    AudioManager.Instance.ChangeBGM(GameState.LOSE);
                }
                break;
            case EndingType.FalseAlarm:
                // homes �� ���ۿ� ����!
                // lose _ false alarm
                result = false;
                AudioManager.Instance.ChangeBGM(GameState.LOSE);
                falseEnding.SetActive(true);
                break;
            case EndingType.TimeOver:
                if (isHomes)
                {
                    // lose
                    result = false;
                    AudioManager.Instance.ChangeBGM(GameState.LOSE);
                }
                else
                {
                    // win 
                    result = true;
                    AudioManager.Instance.ChangeBGM(GameState.WIN);
                }
                break;
            case EndingType.Error:
                // TODO Error type ����ȭ
                // �� ȥ�� ������ ���
                // �ݷ��� ������ ���
                result = true;
                AudioManager.Instance.ChangeBGM(GameState.WIN);
                break;
        }

        resultText.text = (result) ? "YOU WIN!" : "YOU LOSE...";
        gameObject.SetActive(true);

        // 2�� �ڿ� end
        StartCoroutine(goTo(_endType));
    }

    public IEnumerator goTo(EndingType _endType)
    {
        AudioManager.Instance.PauseEffect(EffectAudioType.COOLTIME);
        AudioManager.Instance.PauseEffect(EffectAudioType.PLANE);

        yield return new WaitForSecondsRealtime(4f);

        switch (_endType)
        {
            case EndingType.CatchColloc: 
                endGame();
                break;
/*            case EndingType.FalseAlarm:            // ���� or �κ� -> ��ư �߱�
                                                   // TODO: ��ư �ߴ� �ɷ� �ٲ�� ��, �����ϱ� ����
                leaveRoom();
                break;*/
            case EndingType.TimeOver:  
                endGame();
                break;
            case EndingType.Error:
                // TODO Error type ����ȭ
                endGame();
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
        GameManager.Instance.ChangeScene(GameState.READY);
    }
}
