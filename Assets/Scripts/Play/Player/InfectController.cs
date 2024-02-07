using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfectController : MonoBehaviour
{
    private PhotonView pv;
    private SpriteRenderer spriter;
    private Camera cam;
    private Button infectBtn;
    private List<HandleCollider> colliderList;

    private void Start()
    {
        pv = transform.parent.GetComponent<PhotonView>();
        spriter = transform.GetChild(0).GetComponent<SpriteRenderer>();
        cam = Camera.main;
        colliderList = NetworkManager.Instance.PlaySceneManager.ColliderList;
        spriter.color = Color.gray;

        if (!pv.IsMine) { gameObject.SetActive(false); return; }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            spriter.color = Color.red;
            colliderList.Add(new HandleCollider(collider.gameObject.name, collider.gameObject));
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            spriter.color = Color.gray;
            int index = colliderList.FindIndex(x => x.name == collider.gameObject.name);
            if (index != -1)
            {
                colliderList.RemoveAt(index);
            }
        }
    }

    private void onInfect(Collider2D _collider)
    {
        /*
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = cam.ScreenToWorldPoint(mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, transform.forward, 15f);

        if (hit.collider == null) return;
        if (hit.collider != _collider) return;
        */
        if (!NetworkManager.Instance.PlaySceneManager.TryAttack()) return; 

        StartCoroutine(StaticFuncs.SetEffect(_collider.GetComponent<HandleRPC>().InfectEffect));
        Photon.Realtime.Player targetPlayer = _collider.GetComponent<PhotonView>().Owner;
        AudioManager.Instance.PlayEffect(EffectAudioType.ATTACK);
        pv.RPC("ChangeStatus", targetPlayer, "Infect");
    }
}
