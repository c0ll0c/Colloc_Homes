using Photon.Pun;
using UnityEngine;

public class InfectController : MonoBehaviour
{
    private PhotonView pv;
    private SpriteRenderer spriter;
    private Camera cam;

    private void Start()
    {
        pv = transform.parent.GetComponent<PhotonView>();
        spriter = transform.GetChild(0).GetComponent<SpriteRenderer>();
        cam = Camera.main;
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.name == "PlayerTrigger" && !PlayManager.Instance.gamePlayer.CompareTag("Homes"))
        {
            if (!pv.IsMine) spriter.enabled = true;
            if (Input.GetMouseButtonDown(0) && pv.IsMine)
            {
                onInfect(collider);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.name == "PlayerTrigger")
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    private void onInfect(Collider2D _collider)
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = cam.ScreenToWorldPoint(mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, transform.forward, 15f);

        if (hit.collider == null) return;
        if (hit.collider != _collider) return;

        //attackActivated = true;
        Photon.Realtime.Player targetPlayer = hit.collider.GetComponentInParent<PhotonView>().Owner;
        pv.RPC("ChangeStatus", targetPlayer, "Infect");
    }
}
