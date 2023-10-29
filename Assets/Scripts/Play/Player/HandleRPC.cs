using UnityEngine;
using Photon.Pun;

public class HandleRPC : MonoBehaviour
{
    private SpriteRenderer spriter;

    private void Awake()
    {
        spriter = transform.GetComponent<SpriteRenderer>();
    }

    // sync player direction
    [PunRPC]
    void FlipX(float _axis)
    {
        spriter.flipX = _axis < 0;
    }
}
