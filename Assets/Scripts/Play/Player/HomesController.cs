using UnityEngine;
using Photon.Pun;

// Photon player movement control script
public class HomesController : MonoBehaviour
{
    private Joystick joystick;
    private Vector2 inputVec;
    private Vector2 nextVec;
    private float speed;
    private float moveY, moveX;

    private PhotonView pv;
    private Rigidbody2D rigid;
    private Animator anim;

    private void Awake()
    {
        pv = transform.parent.GetComponent<PhotonView>();
        rigid = transform.parent.GetComponent<Rigidbody2D>();
        anim = transform.parent.GetComponent<Animator>();
        speed = 3.5f;

        if (!pv.IsMine) { gameObject.SetActive(false); return; }

        // camera setting (focus on my player)
        Camera cam = Camera.main;
        GameObject playerDot;

        playerDot = GameObject.Find("Panel_MinimapDot");
        playerDot.transform.SetParent(transform.parent.GetChild(4));
        playerDot.transform.localPosition = new Vector3(0f, 0f, -1f);

        cam.transform.SetParent(transform.parent);
        cam.transform.localPosition = new Vector3(0f, 0f, -5f);

        joystick = NetworkManager.Instance.PlaySceneManager.Joystick;
    }

    // moving & animation function
    private void FixedUpdate()
    {
        moveY = joystick.Vertical;
        moveX = joystick.Horizontal;

        inputVec = new Vector2(moveX, moveY);
        nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);

        anim.SetFloat("Speed", inputVec.magnitude);
        if (!Equals(speed, 0))
        {
            pv.RPC("FlipX", RpcTarget.All, inputVec.x);
        }

        if (pv.IsMine)
        {
            if (inputVec.x != 0 || inputVec.y != 0)
            {
                AudioManager.Instance.PlayerFootSound();
            }
            else
            {
                AudioManager.Instance.PauseFootSound();
            }
        }
    }

    // change speed after attack
    public void SetSpeed(float _speed)
    {
        speed = _speed;
    }
}
