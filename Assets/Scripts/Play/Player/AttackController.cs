using Photon.Pun;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    private bool attackActivated = false;

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
        if (attackActivated) return;

        Vector3 mousePosition = Input.mousePosition;
        mousePosition = cam.ScreenToWorldPoint(mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, transform.forward, 15f);

        if (hit.collider == null) return;
        if (hit.collider.name != "PlayerTrigger" || hit.collider.GetComponentInParent<PhotonView>().IsMine) return;

        //attackActivated = true;
        Photon.Realtime.Player targetPlayer = hit.collider.GetComponentInParent<PhotonView>().Owner;
        pv.RPC("Attack", targetPlayer);
    }
}
