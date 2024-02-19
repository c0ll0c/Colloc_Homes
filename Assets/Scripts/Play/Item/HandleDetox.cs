using Photon.Pun;
using System.Collections;
using UnityEngine;

public class HandleDetox : MonoBehaviour
{
    private GameObject lightOffObj;
    private GameObject lightOnObj;
    private GameObject timer3Obj;
    private GameObject twinkleEffectObj;

    private bool isActive;
    private bool isUsing;
    private bool localNeedsDetox;
    private int boothUser;

    private void Awake()
    {
        isActive = true;
        isUsing = false;
        localNeedsDetox = false;
        boothUser = 0;

        lightOffObj = transform.GetChild(0).GetChild(0).gameObject;
        lightOnObj = transform.GetChild(0).GetChild(1).gameObject;

        timer3Obj = transform.GetChild(1).GetChild(0).gameObject;
        twinkleEffectObj = transform.GetChild(1).GetChild(1).gameObject;

        ActivateBooth(true);
    }

    public void DetoxUsed()
    {
        if (localNeedsDetox)
        {
            NetworkManager.Instance.PlaySceneManager.gamePlayer.GetComponent<HandleRPC>().ChangeStatus("Homes");
            AudioManager.Instance.PauseEffect(EffectAudioType.COOLTIME);
            AudioManager.Instance.PlayEffect(EffectAudioType.DETOX);
            localNeedsDetox = false;
        }
        else
        {
            AudioManager.Instance.PauseEffect(EffectAudioType.COOLTIME);
            AudioManager.Instance.PlayEffect(EffectAudioType.UNDETOX);
        }
        ActivateBooth(false);
    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (!isActive || isUsing) return; 
        if (isUsing) return;

        Debug.Log(_collision.gameObject.name);
        if (_collision.gameObject.GetComponent<PhotonView>().IsMine)
        {
            AudioManager.Instance.PlayEffect(EffectAudioType.COOLTIME);

            if (_collision.gameObject.CompareTag("Infect")) localNeedsDetox = true;
        }

        boothUser = _collision.gameObject.GetInstanceID();
        UseBooth(true);
    }

    private void OnTriggerExit2D(Collider2D _collision)
    {
        if (Equals(_collision.gameObject.GetInstanceID(), boothUser))
        {
            boothUser = 0;
            if (_collision.gameObject.GetComponent<PhotonView>().IsMine)
            {
                AudioManager.Instance.PauseEffect(EffectAudioType.COOLTIME);
                localNeedsDetox = false;
            }
            UseBooth(false);
        }
    }

    private void UseBooth(bool _use)
    {
        isUsing = _use;
        lightOnObj.SetActive(_use);
        lightOffObj.SetActive(!_use);
        timer3Obj.SetActive(_use);
    }

    private void ActivateBooth(bool _activate)
    {
        isActive = _activate;
        UseBooth(false);
        twinkleEffectObj.SetActive(_activate);
        if (!_activate)
        {
            StartCoroutine(CountDeactivateTime());
        }
    }

    IEnumerator CountDeactivateTime()
    {
        yield return StaticFuncs.WaitForSeconds(StaticVars.DETOX_DEACTIVE_TIME);
        AudioManager.Instance.PlayEffect(EffectAudioType.ACTIVE);
        ActivateBooth(true);
    }
}
