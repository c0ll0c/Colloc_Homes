using System;
using System.Collections;
using UnityEngine;
using Photon.Pun;

// Photon player movement control script
public class HomesController : MonoBehaviour
{
    private Vector2 inputVec;
    private Vector2 nextVec;
    private float speed;
    private float moveY, moveX;
    private PhotonView pv;
    private Rigidbody2D rigid;
    private SpriteRenderer spriter;
    private Animator anim;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        speed = 3.0f;

        // camera setting (focus on my player)
        if (pv.IsMine)
        {
            Camera cam = Camera.main;
            cam.transform.SetParent(transform);
            cam.transform.localPosition = new Vector3(0f, 0f, -5f);
        }
    }

    // moving & animation function
    private void FixedUpdate()
    {
        if (!pv.IsMine || !PhotonNetwork.IsConnected) return; // can control only mine

        moveY = Input.GetAxis("Vertical");
        moveX = Input.GetAxis("Horizontal");
        inputVec = new Vector2(moveX, moveY);
        nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);

        anim.SetFloat("Speed", inputVec.magnitude);
        if (inputVec.x != 0) pv.RPC("FlipX", RpcTarget.All, inputVec.x);
    }

    // sync player direction
    [PunRPC]
    void FlipX(float axis)  
    {
        spriter.flipX = axis < 0;
    }

}