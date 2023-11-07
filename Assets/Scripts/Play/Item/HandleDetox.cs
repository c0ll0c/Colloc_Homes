using UnityEngine;

public class HandleDetox : MonoBehaviour
{
    private GameObject deactivatedObj;
    private GameObject activatedObj;
    private GameObject timerObj;
    private DetoxTimer detoxTimer;
    private GameObject buttonObj;
    private ItemBtnsOnClick btnHandler;

    public bool isActivated;

    private void Awake()
    {
        isActivated = true;

        deactivatedObj = transform.GetChild(0).GetChild(0).gameObject;
        activatedObj = transform.GetChild(0).GetChild(1).gameObject;

        timerObj = transform.GetChild(1).GetChild(0).gameObject;
        detoxTimer = timerObj.GetComponent<DetoxTimer>();
        detoxTimer.DetoxHandler = this;

        buttonObj = transform.GetChild(1).GetChild(1).gameObject;
        btnHandler = buttonObj.GetComponent<ItemBtnsOnClick>();

        ActivateBooth(true);
        buttonObj.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isActivated)
        {
            // [TODO] collision �Ͼ player�� ������ play ���� ������� Ȯ���ؾ� ��!
            if (collision.collider.CompareTag("Homes"))
            {
                btnHandler.ChangeBtnText("����ϱ�");
            }
            else if (collision.collider.CompareTag("Colloc"))
            {
                btnHandler.ChangeBtnText("�ı��ϱ�");
            }
            buttonObj.SetActive(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        buttonObj.SetActive(false);
    }

    public void ActivateBooth(bool activate)
    {
        isActivated = activate;
        activatedObj.SetActive(activate);

        timerObj.SetActive(!activate);
        deactivatedObj.SetActive(!activate);
    }

    // Called When
    // 1) Homes uses detox booth
    // 2) Colloc deactivates booth
    public void UseOrDeactivate()
    {
        if (!isActivated) return;

        ActivateBooth(false);
    }
}
