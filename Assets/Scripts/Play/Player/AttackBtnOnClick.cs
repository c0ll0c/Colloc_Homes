using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class AttackBtnOnClick : MonoBehaviour
{
    public PhotonView pv;
    private List<HandleCollider> colliderList;

    private void Start()
    {
        colliderList = NetworkManager.Instance.PlaySceneManager.ColliderList;
    }

    public void onAttack()
    {
        if (colliderList.Count < 1) return;

        if (!NetworkManager.Instance.PlaySceneManager.TryAttack()) return;

        StartCoroutine(StaticFuncs.SetEffect(colliderList[0].collider.GetComponent<HandleRPC>().AttackEffect));
        Photon.Realtime.Player targetPlayer = colliderList[0].collider.GetComponent<PhotonView>().Owner;
        AudioManager.Instance.PlayEffect(EffectAudioType.ATTACK);
        pv.RPC("Attack", targetPlayer);
    }

    public void onInfect()
    {
        if (colliderList.Count < 1) return;

        if (!NetworkManager.Instance.PlaySceneManager.TryAttack()) return;

        StartCoroutine(StaticFuncs.SetEffect(colliderList[0].collider.GetComponent<HandleRPC>().InfectEffect));
        Photon.Realtime.Player targetPlayer = colliderList[0].collider.GetComponent<PhotonView>().Owner;
        AudioManager.Instance.PlayEffect(EffectAudioType.ATTACK);
        pv.RPC("ChangeStatus", targetPlayer, "Infect");
    }
}
