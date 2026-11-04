using Enums;
using ObjectsPool;
using UnityEngine;

namespace PowerUps
{
    public class PowerUpObject : PooledObject
    {
        public static event System.Action PowerUpPickedUp;

        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private Animator animator;
        [SerializeField] private float outOfBoundsY = -4.25f;

        private EPowerUp powerUpType;

        public void Setup(Vector3 position, EPowerUp powerUpType)
        {
            animator.SetFloat("PowerUpID", (int)powerUpType);
            rb.linearVelocity = Vector2.down;
            transform.position = position;
            this.powerUpType = powerUpType;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.transform.CompareTag("Paddle"))
            {
                PowerUpPickedUp?.Invoke();

                GameManager.Instance.PowerUpsManager.ApplyPowerUp(powerUpType);

                ReturnToPool();
            }
        }

        public override void ReturnToPool()
        {
            rb.linearVelocity = Vector2.zero;
            base.ReturnToPool();
        }

        private void FixedUpdate()
        {
            //Zebezpieczenie jak coœ wyjdzie poza mapê
            if (transform.position.y < outOfBoundsY)
            {
                ReturnToPool();
            }
        }
    }
}