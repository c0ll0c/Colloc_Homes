using UnityEngine;
using Photon.Pun;
using System.Collections;
using TMPro;

public class HandleRPC : MonoBehaviour
{
    public GameObject AttackEffect;
    public GameObject InfectEffect;
    public GameObject VaccineEffect;

    [SerializeField] private TextMeshProUGUI nickname;
    private SpriteRenderer spriter;
    private PhotonView pv;
    private AttackController attackController;
    private HomesController homesController;

    private void Awake()
    {
        spriter = transform.GetComponent<SpriteRenderer>();
        pv = transform.GetComponent<PhotonView>();
    }

    private void Start()
    {
        pv.RPC("SetNickname", RpcTarget.AllBuffered, pv.Owner.NickName);
        AttackEffect.SetActive(false);
        InfectEffect.SetActive(false);
        VaccineEffect.SetActive(false);

        if (gameObject.CompareTag("Colloc"))
        {
            transform.GetChild(2).GetComponent<AttackController>().enabled = false;
        }

        StartCoroutine(DelayStart());
    }

    private IEnumerator DelayStart()
    {
        yield return StaticFuncs.WaitForSeconds(3.0f);
        attackController = PlayManager.Instance.gamePlayer.transform.GetChild(2).GetComponent<AttackController>();
        homesController = PlayManager.Instance.gamePlayer.transform.GetChild(2).GetComponent<HomesController>();
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
        if (PlayManager.Instance.gamePlayer.CompareTag(_status)) return;
        if (PlayManager.Instance.gamePlayer.CompareTag("Colloc")) return;

        if (_status == "Homes")
        {
            attackController.enabled = true;
            PlayManager.Instance.gamePlayer.tag = "Homes";
            UIManager.Instance.SetGameUI("Homes");
        }
        else
        {
            StartCoroutine(DelayInfect());
        }
    }

    private IEnumerator DelayInfect()
    {
        yield return StaticFuncs.WaitForSeconds(3.0f);
        attackController.enabled = false;
        PlayManager.Instance.gamePlayer.tag = "Infect";
        UIManager.Instance.SetGameUI("Infect");
        StartCoroutine(StaticFuncs.SetEffect(PlayManager.Instance.gamePlayer.GetComponent<HandleRPC>().InfectEffect));
    }

    // change player speed (attack)
    [PunRPC]
    public void Attack()
    {
        homesController.SetSpeed(0);
        StartCoroutine(StaticFuncs.SetEffect(PlayManager.Instance.gamePlayer.GetComponent<HandleRPC>().AttackEffect));
        StartCoroutine(ResetSpeed());
    }

    private IEnumerator ResetSpeed()
    {
        yield return StaticFuncs.WaitForSeconds(3.0f);
        homesController.SetSpeed(3.0f);
    }
}
