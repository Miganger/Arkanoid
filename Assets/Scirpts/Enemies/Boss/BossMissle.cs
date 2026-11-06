using ObjectsPool;
using Paddle;
using System;
using UnityEngine;

namespace Enemies.Boss
{
    public class BossMissle : PooledObject
    {
        public static event System.Action Hit;

        [SerializeField] private float missleSpeed = 3f;
        [SerializeField] private float outOfBoundsY = -6f;

        private Animator animator;
        private Vector3 direction;

        private bool isHitted = false;

        private void Awake()
        {
            if (!animator)
                animator = GetComponent<Animator>();
        }

        internal void SetDirection(Vector3 spawnPoint, Vector3 direction)
        {
            transform.position = spawnPoint;
            this.direction = direction;
            isHitted = false;
            gameObject.SetActive(true);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Paddle"))
            {
                PaddleController controller = collision.GetComponentInParent<PaddleController>();
                if (controller)
                {
                    controller.OnHit();
                }
                Hit?.Invoke();
                animator.SetTrigger("Hit");
                isHitted = true;
            }
        }

        private void FixedUpdate()
        {
            if (isHitted)
                return;

            transform.position += direction * (Time.deltaTime * missleSpeed);

            if (transform.position.y < outOfBoundsY)
            {
                ReturnToPool();
            }
        }
    }

}