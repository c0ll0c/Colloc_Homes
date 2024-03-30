using UnityEngine;
using UnityEngine.UI;

public class TutorlaManager : MonoBehaviour
{
    public Text dialogText;
    public GameObject clueCanvas;
    private string[] dialogBody;
    int i = 0;

    // Start is called before the first frame update
    void Start()
    {
        dialogBody = StaticVars.tutorialMessage;
        clueCanvas.gameObject.SetActive(false);
        dialogText.text = dialogBody[i];
    }

    // next ��ư ������ ��, �������� �Ѿ��
    public void OnClickNextButton()
    {
        if (i >= 0 && i <= 18)
        {
            if (i == 4 || i == 9 || i == 13)
            {
                simulationOn();
            }

            i++;
            dialogText.text = fetchText(i);
        }
        else if (i == 19)
        {
            Debug.Log("�κ��");
            // GameManager.Instance.ChangeScene(GameState.LOBBY);
        }
    }

    // before ��ư ������ ��, �������� ���ư���
    public void OnClickBeforeButton()
    {
        if (i > 0 && i <= 19)
        {
            i--;
            dialogText.text = fetchText(i);
        }
    }

    private string fetchText(int i)
    {
        return dialogBody[i];
    }

    private void simulationOn()
    {
        gameObject.SetActive(false);
    }

    public void simulationOff()
    {
        gameObject.SetActive(true);
    }

    public void clueSimul()
    {
        clueCanvas.gameObject.SetActive(true);
    }
}
