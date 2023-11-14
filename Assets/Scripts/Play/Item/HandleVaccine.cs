using Photon.Pun;
using UnityEngine;

// vaccine function script
public class HandleVaccine : PoolAble
{
    private Rigidbody2D rigid;
    private int num = 0;
    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        StaticFuncs.SpriteRendering(gameObject);
    }

    private void FixedUpdate()
    {
        if (num < 50)
        {
            num++;
            rigid.MovePosition(rigid.position + new Vector2(0, -0.02f));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Untagged")) return;

        if (collision.collider.GetComponent<PhotonView>().IsMine)
        {
            PlayManager.Instance.isVaccinated = true;
        }

        ReleaseObject();
   
    }
}
