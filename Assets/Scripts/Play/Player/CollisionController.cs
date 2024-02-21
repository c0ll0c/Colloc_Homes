using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
    private GameObject player;
    private PhotonView pv;
    private SpriteRenderer spriter;
    private List<HandleCollider> colliderList;
    private int index;

    private void Awake()
    {
        player = transform.parent.gameObject;
        pv = player.GetComponent<PhotonView>();
        spriter = transform.GetChild(0).GetComponent<SpriteRenderer>();
        colliderList = NetworkManager.Instance.PlaySceneManager.ColliderList;
        spriter.color = Color.white;

        if (!pv.IsMine) { gameObject.SetActive(false); return; }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.gameObject.CompareTag("Player")) return;

        if (player.CompareTag("Homes"))
        {
            spriter.color = Color.blue;
        }
        else
        {
            spriter.color = Color.red;
        }
        colliderList.Add(new HandleCollider(collider.gameObject.name, collider.gameObject));
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (!collider.gameObject.CompareTag("Player")) return;

        if (colliderList.Count == 1)
        {
            spriter.color = Color.white;
            colliderList.Clear();
        }

        index = colliderList.FindIndex(x => x.name == collider.gameObject.name);
        if (index != -1)
        {
            colliderList.RemoveAt(index);
        }
    }
}
