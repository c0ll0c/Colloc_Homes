using UnityEngine;
using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class HandleRPC : MonoBehaviour
{
    public GameObject AttackEffect;
    public GameObject InfectEffect;
    public GameObject VaccineEffect;

    [SerializeField] private TextMeshProUGUI nickname;
    private SpriteRenderer spriter;
    private PhotonView pv;
    private HomesController homesController;
    private Button infectBtn;
    private Button attackBtn;

    private void Awake()
    {
        spriter = transform.GetComponent<SpriteRenderer>();
        pv = transform.GetComponent<PhotonView>();
    }

    private void Start()
    {
        infectBtn = NetworkManager.Instance.PlaySceneManager.InfectBtn;
        attackBtn = NetworkManager.Instance.PlaySceneManager.AttackBtn;

        pv.RPC("SetNickname", RpcTarget.AllBuffered, pv.Owner.NickName);
        AttackEffect.SetActive(false);
        InfectEffect.SetActive(false);
        VaccineEffect.SetActive(false);

        homesController = NetworkManager.Instance.PlaySceneManager.GetLocalController();
    }

    // sync player direction
    [PunRPC]
    public void FlipX(float _axis)
    {
        spriter.flipX = _axis < 0;
    }

    // set player nickname
    [PunRPC]
    public void SetNickname(string _nickname)
    {
        nickname.text = _nickname;
    }

    // change player status (infect or detox)
    [PunRPC]
    public void ChangeStatus(string _status)
    {
        if (NetworkManager.Instance.PlaySceneManager.CompareLocalTag(_status)) return;
        if (NetworkManager.Instance.PlaySceneManager.CompareLocalTag(StaticVars.TAG_COLLOC)) return;

        if (Equals(string.Compare(_status, StaticVars.TAG_HOLMES, true), 0))
        {
            attackBtn.gameObject.SetActive(true);
            infectBtn.gameObject.SetActive(false);
            NetworkManager.Instance.PlaySceneManager.ChangePlayerTag(StaticVars.TAG_HOLMES);
            UIManager.Instance.SetGameUI(StaticVars.TAG_HOLMES);
            StaticFuncs.StopEffect(NetworkManager.Instance.PlaySceneManager.LocalRPC.InfectEffect);
            pv.RPC("UpdateInfectProgress", RpcTarget.All, false);
        }
        else
        {
            StartCoroutine(DelayInfect());
        }
    }

    private IEnumerator DelayInfect()
    {
        yield return StaticFuncs.WaitForSeconds(StaticVars.INFECT_DELAY_TIME);

        if (NetworkManager.Instance.PlaySceneManager.isVaccinated)
        {
            NetworkManager.Instance.PlaySceneManager.LocalRPC.VaccineEffect.SetActive(false);
            NetworkManager.Instance.PlaySceneManager.isVaccinated = false;
            AudioManager.Instance.PlayEffect(EffectAudioType.VACCINE);
        }
        else
        {
            infectBtn.gameObject.SetActive(true);
            attackBtn.gameObject.SetActive(false);
            NetworkManager.Instance.PlaySceneManager.ChangePlayerTag(StaticVars.TAG_INFECT);
            UIManager.Instance.SetGameUI(StaticVars.TAG_INFECT);
            AudioManager.Instance.PlayEffect(EffectAudioType.INFECT);
            StartCoroutine(StaticFuncs.SetEffect(NetworkManager.Instance.PlaySceneManager.LocalRPC.InfectEffect));
            pv.RPC("UpdateInfectProgress", RpcTarget.All, true);
        }
    }

    // change player speed (attack)
    [PunRPC]
    public void Attack()
    {
        homesController.SetSpeed(0);
        attackBtn.interactable = false;
        infectBtn.interactable = false;
        AudioManager.Instance.PlayEffect(EffectAudioType.ATTACKED);
        StartCoroutine(StaticFuncs.SetEffect(NetworkManager.Instance.PlaySceneManager.LocalRPC.AttackEffect));
        StartCoroutine(ResetSpeed());
    }

    private IEnumerator ResetSpeed()
    {
        yield return StaticFuncs.WaitForSeconds(StaticVars.DELAY_TIME);
        attackBtn.interactable = true;
        infectBtn.interactable = true;
        homesController.SetSpeed(StaticVars.HOMES_SPEED);
    }

    [PunRPC]
    public void UpdateInfectProgress(bool infect)
    {
        NetworkManager.Instance.PlaySceneManager.InfectProgressUI.UpdateProgress(infect);
    }
}
