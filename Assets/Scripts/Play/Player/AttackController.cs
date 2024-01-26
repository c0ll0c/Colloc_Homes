using Photon.Pun;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    private Camera cam;
    private GameObject player;
    private PhotonView pv;

    private void Start()
    {
        cam = Camera.main;
        player = transform.parent.gameObject;
        pv = player.GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            onAttack();
        }
    }

    private void onAttack()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = cam.ScreenToWorldPoint(mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, transform.forward, 15f);

        if (hit.collider == null) return;
        if (hit.collider.name != "PlayerTrigger") return;
        if (hit.collider.GetComponentInParent<PhotonView>().IsMine) return;

        if (!NetworkManager.Instance.PlaySceneManager.TryAttack()) return;
        StartCoroutine(StaticFuncs.SetEffect(hit.collider.GetComponentInParent<HandleRPC>().AttackEffect));
        Photon.Realtime.Player targetPlayer = hit.collider.GetComponentInParent<PhotonView>().Owner;
        AudioManager.Instance.PlayEffect(EffectAudioType.ATTACK);
        pv.RPC("Attack", targetPlayer);
    }
}
