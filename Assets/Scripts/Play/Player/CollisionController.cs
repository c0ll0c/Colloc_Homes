using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
    private PhotonView pv;
    private SpriteRenderer spriter;
    private List<HandleCollider> colliderList;

    private void Start()
    {
        pv = transform.parent.GetComponent<PhotonView>();
        spriter = transform.GetChild(0).GetComponent<SpriteRenderer>();
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
            if (colliderList.Count == 1)
            {
                spriter.color = Color.gray;
            }
            int index = colliderList.FindIndex(x => x.name == collider.gameObject.name);
            if (index != -1)
            {
                colliderList.RemoveAt(index);
            }
        }
    }
}
