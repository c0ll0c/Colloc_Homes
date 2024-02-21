using Photon.Pun;
using UnityEngine;

// vaccine function script
public class HandleVaccine : PoolAble
{
    private GameObject effect;

    private void Start()
    {
        StaticFuncs.SpriteRendering(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Untagged")) return;

        if (collision.collider.GetComponent<PhotonView>().IsMine && collision.collider.gameObject.CompareTag(StaticVars.TAG_HOLMES))
        {
            NetworkManager.Instance.PlaySceneManager.StartVaccine();
        }
        else
        {
            AudioManager.Instance.PlayEffect(EffectAudioType.DROP);
        }

        ReleaseObject();
   
    }
}
