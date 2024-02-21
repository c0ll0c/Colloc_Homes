using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using System.Collections;
using System.Text;

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
    private Image winLoseImg;
    private Image resultImg;
    private TMP_Text resultTxt;
    public Sprite[] EndingImage;
    public Sprite[] ResultImage;
    public GameObject falseEnding;

    public void Awake()
    {
        NetworkManager.Instance.EndingManager = this;
        winLoseImg = transform.GetChild(1).GetComponent<Image>();
        resultImg = transform.GetChild(2).GetComponent<Image>();
        resultTxt = transform.GetChild(3).GetComponent<TMP_Text>();
        gameObject.SetActive(false);
        falseEnding.SetActive(false);
    }
    public void OnDisable()
    {
        Time.timeScale = 1;
    }

    private void OnDestroy()
    {
        NetworkManager.Instance.EndingManager = null;
    }

    public void ShowResult(EndingType _endType, bool _invoker, string _winnerHolmes)
    {
        Time.timeScale = 0;
        bool isHomes = !NetworkManager.Instance.PlaySceneManager.CompareLocalTag(StaticVars.TAG_COLLOC);

        // format colloc name and color
        string color = StaticFuncs.GetColorName((int)PhotonNetwork.CurrentRoom.CustomProperties[StaticCodes.PHOTON_R_CCOLR]);
        string collocString = StaticFuncs.WrapNameWithColor(
            color, 
            (string)PhotonNetwork.CurrentRoom.CustomProperties[StaticCodes.PHOTON_R_CNAME]
        );

        StringBuilder sb = new StringBuilder();
        switch (_endType)
        {
            case EndingType.CatchColloc:
                if (isHomes)
                {
                    if (_invoker)
                    {
                        // win _ founder homes
                        result = true;
                        resultImg.sprite = EndingImage[1];
                        AudioManager.Instance.ChangeBGM(GameState.WIN);
                    }
                    else             
                    {
                        // lose _ other homes
                        result = false;
                        resultImg.sprite = EndingImage[4];
                        AudioManager.Instance.ChangeBGM(GameState.LOSE);
                    }
                }
                else                
                {
                    // lose _ colloc
                    result = false;
                    resultImg.sprite = EndingImage[3];
                    AudioManager.Instance.ChangeBGM(GameState.LOSE);
                }
                sb.Append(_winnerHolmes);
                sb.Append(" Ȩ� ");
                sb.Append(collocString);
                sb.Append(" �ݷ��� �����½��ϴ�!");
                resultTxt.text = sb.ToString();

                break;
            case EndingType.FalseAlarm:
                // homes �� ���ۿ� ����!
                // lose _ false alarm
                result = false;
                resultImg.sprite = EndingImage[4];
                AudioManager.Instance.ChangeBGM(GameState.LOSE);
                // falseEnding.SetActive(true);

                sb.Append("����� ������ Ȩ� ����߾��.. �ݷ��� ��ü�� ");
                sb.Append(collocString);
                sb.Append("�̾����ϴ�!");
                resultTxt.text = sb.ToString();

                break;
            case EndingType.TimeOver:
                if (isHomes)
                {
                    // lose
                    result = false;
                    resultImg.sprite = EndingImage[2];
                    AudioManager.Instance.ChangeBGM(GameState.LOSE);
                }
                else
                {
                    // win 
                    result = true;
                    resultImg.sprite = EndingImage[0];
                    AudioManager.Instance.ChangeBGM(GameState.WIN);
                }

                sb.Append(collocString);
                sb.Append(" �ݷ��� ���̷����� ��ΰ� �����Ǿ����ϴ�");
                resultTxt.text = sb.ToString();

                break;
            case EndingType.Error:
                // TODO Error type ����ȭ
                // �� ȥ�� ������ ���
                // �ݷ��� ������ ���
                result = true;
                resultImg.sprite = EndingImage[1];
                AudioManager.Instance.ChangeBGM(GameState.WIN);
                resultTxt.text = "ȥ�� ���� �÷��̾ �Ǿ� �¸��մϴ�!";
                break;
        }

        winLoseImg.sprite = (result) ? ResultImage[0] : ResultImage[1];
        gameObject.SetActive(true);

        // 2�� �ڿ� end
        StartCoroutine(GoTo(_endType));
    }

    public IEnumerator GoTo(EndingType _endType)
    {
        AudioManager.Instance.PauseEffect(EffectAudioType.COOLTIME);
        AudioManager.Instance.PauseEffect(EffectAudioType.PLANE);

        yield return new WaitForSecondsRealtime(StaticVars.Ending_TIME);

        switch (_endType)
        {
            case EndingType.CatchColloc: 
                EndGame();
                break;
            case EndingType.FalseAlarm:            // ���� or �κ� -> ��ư �߱�
                                                   // TODO: ��ư �ߴ� �ɷ� �ٲ�� ��, �����ϱ� ����
                LeaveRoom();
                break;
            case EndingType.TimeOver:  
                EndGame();
                break;
            case EndingType.Error:
                // TODO Error type ����ȭ
                EndGame();
                break;
        }
    }

    void LeaveRoom()
    {
        Time.timeScale = 1;
        NetworkManager.Instance.LeaveRoom();
    }

    void EndGame()
    {
        Time.timeScale = 1;
        NetworkManager.Instance.EndGame();
    }
}
