using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class AttackBtnOnClick : MonoBehaviour
{
    public PhotonView PV;

    private List<HandleCollider> colliderList;
    private Photon.Realtime.Player targetPlayer;

    private void Start()
    {
        colliderList = NetworkManager.Instance.PlaySceneManager.ColliderList;
    }

    public void onAttack()
    {
        if (colliderList.Count < 1) return;

        if (!NetworkManager.Instance.PlaySceneManager.TryAttack()) return;

        StartCoroutine(StaticFuncs.SetEffect(colliderList[0].collider.GetComponent<HandleRPC>().AttackEffect));
        targetPlayer = colliderList[0].collider.GetComponent<PhotonView>().Owner;
        AudioManager.Instance.PlayEffect(EffectAudioType.ATTACK);
        PV.RPC("Attack", targetPlayer);
    }

    public void onInfect()
    {
        if (colliderList.Count < 1) return;

        if (!NetworkManager.Instance.PlaySceneManager.TryInfect()) return;

        StartCoroutine(StaticFuncs.SetEffect(colliderList[0].collider.GetComponent<HandleRPC>().InfectEffect));
        targetPlayer = colliderList[0].collider.GetComponent<PhotonView>().Owner;
        AudioManager.Instance.PlayEffect(EffectAudioType.ATTACK);
        PV.RPC("ChangeStatus", targetPlayer, StaticVars.TAG_INFECT);
    }
}
