using Blocks;
using ObjectsPool;
using System;
using UnityEngine;

public class Missle : PooledObject
{
    [SerializeField] private float baseSpeed = 3f;

    private Rigidbody2D rb;
    private Animator animator;

    private void Awake()
    {
        if (!rb)
            rb = GetComponent<Rigidbody2D>();

        if(!animator)
            animator = GetComponent<Animator>();
    }

    public void Fire()
    {
        rb.linearVelocity = Vector3.up * baseSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.TryGetComponent(out BlockController block))
        {
            if (!block.gameObject.activeSelf)
            {
                rb.linearVelocity = Vector3.up * baseSpeed;
                return;
            }

            block.Hit();
        }
        rb.linearVelocity = Vector3.zero;
        animator.SetTrigger("Hit");
    }
}
