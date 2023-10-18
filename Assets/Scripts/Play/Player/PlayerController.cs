using System;
using System.Collections;
using UnityEngine;
using Photon.Pun;

// player movement control script
 public class PlayerController : MonoBehaviour
{
    private Vector2 inputVec;
    private Vector2 nextVec;
    private float speed;
    private float moveY, moveX;

    private Rigidbody2D rigid;
    private SpriteRenderer spriter;
    private Animator anim;

    void Awake()
    {       
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        speed = 3.0f;

        // camera setting (focus on player)
        Camera cam = Camera.main;
        cam.transform.SetParent(transform);
        cam.transform.localPosition = new Vector3(0f, 0f, -5f);
    }

    // moving function
    void FixedUpdate()
    {
        moveY = Input.GetAxis("Vertical");
        moveX = Input.GetAxis("Horizontal");
        inputVec = new Vector2(moveX, moveY);
        nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);

        anim.SetFloat("Speed", inputVec.magnitude);
        if (inputVec.x != 0) spriter.flipX = inputVec.x < 0;
    }
}