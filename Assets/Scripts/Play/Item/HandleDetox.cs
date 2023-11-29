using System.Collections;
using UnityEngine;

public class HandleDetox : MonoBehaviour
{
    private GameObject lightOffObj;
    private GameObject lightOnObj;
    private GameObject timer3Obj;
    private GameObject twinkleEffectObj;
    private DetoxTimerUI detoxTimer;

    private bool isActive;
    private bool isUsing;
    private bool isMe;
    private int boothUser;
    

    private void Awake()
    {
        isActive = true;
        isUsing = false;
        isMe = false;
        boothUser = 0;

        lightOffObj = transform.GetChild(0).GetChild(0).gameObject;
        lightOnObj = transform.GetChild(0).GetChild(1).gameObject;

        timer3Obj = transform.GetChild(1).GetChild(0).gameObject;
        twinkleEffectObj = transform.GetChild(1).GetChild(1).gameObject;

        detoxTimer = timer3Obj.GetComponent<DetoxTimerUI>();

        ActivateBooth(true);
    }

    public void DetoxUsed()
    {
        if (isMe)
        {
            NetworkManager.Instance.PlaySceneManager.gamePlayer.GetComponent<HandleRPC>().ChangeStatus("Homes");
        }
        ActivateBooth(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (
            collision.gameObject.CompareTag("Player") || 
            collision.gameObject.CompareTag("Homes") || 
            collision.gameObject.CompareTag("Colloc")
            ) { 
            isMe = false; 
        }
        else if ( collision.gameObject.CompareTag("Infect") ) { isMe = true; }
        else { return; }

        if (isActive && !isUsing)
        {
            boothUser = collision.gameObject.GetInstanceID();
            UseBooth(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetInstanceID() == boothUser)
        {
            boothUser = 0;
            UseBooth(false);
        }
    }

    private void UseBooth(bool use)
    {
        isUsing = use;
        lightOnObj.SetActive(use);
        lightOffObj.SetActive(!use);
        timer3Obj.SetActive(use);
    }

    private void ActivateBooth(bool activate)
    {
        isActive = activate;
        UseBooth(false);
        twinkleEffectObj.SetActive(activate);
        if (!activate)
        {
            StartCoroutine(CountDeactivateTime());
        }
    }
    
    private readonly WaitForSecondsRealtime waitSec = new WaitForSecondsRealtime(StaticVars.DETOX_DEACTIVE_TIME);
    IEnumerator CountDeactivateTime()
    {
        yield return waitSec;
        ActivateBooth(true);
    }
}
