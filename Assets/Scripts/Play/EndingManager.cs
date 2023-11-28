using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
        gameObject.SetActive(false);
        resultText = transform.GetChild(1).GetComponent<TMP_Text>();
        resultImg = transform.GetChild(2).GetComponent<Image>();
    }

    public void OnEnable()
    {
        Time.timeScale = 0;
    }

    public void OnDisable()
    {
        Time.timeScale = 1;
    }

    public void ShowResult(EndingType _endType, bool invoker)
    {
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
                // homes 일 수 밖에 없다!
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
                break;
        }

        resultText.text = (result) ? "YOU WIN!" : "YOU LOSE...";
        gameObject.SetActive(true);
    }
}
