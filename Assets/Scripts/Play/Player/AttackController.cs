using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    private Camera cam;
    private GameObject player;
    private PhotonView pv;
    private List<HandleCollider> colliderList;

    private void Start()
    {
        cam = Camera.main;
        player = NetworkManager.Instance.PlaySceneManager.gamePlayer;
        pv = player.GetComponent<PhotonView>();
        colliderList = NetworkManager.Instance.PlaySceneManager.ColliderList;
    }

    private void onAttack()
    {
        if (colliderList.Count < 1) return;

        if (!NetworkManager.Instance.PlaySceneManager.TryAttack()) return;

        StartCoroutine(StaticFuncs.SetEffect(colliderList[0].collider.GetComponent<HandleRPC>().AttackEffect));
        Photon.Realtime.Player targetPlayer = colliderList[0].collider.GetComponent<PhotonView>().Owner;
        AudioManager.Instance.PlayEffect(EffectAudioType.ATTACK);
        pv.RPC("Attack", targetPlayer);
    }

    private void onInfect()
    {
        if (colliderList.Count < 1) return;

        if (!NetworkManager.Instance.PlaySceneManager.TryAttack()) return;

        StartCoroutine(StaticFuncs.SetEffect(colliderList[0].collider.GetComponent<HandleRPC>().InfectEffect));
        Photon.Realtime.Player targetPlayer = colliderList[0].collider.GetComponent<PhotonView>().Owner;
        AudioManager.Instance.PlayEffect(EffectAudioType.ATTACK);
        pv.RPC("ChangeStatus", targetPlayer, "Infect");
    }
}
