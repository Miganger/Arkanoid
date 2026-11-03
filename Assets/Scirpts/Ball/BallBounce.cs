using Enums;
using UnityEngine;

namespace Ball
{
    [RequireComponent(typeof(Rigidbody2D), typeof(BallController))]
    public class BallBounce : MonoBehaviour
    {
        [Header("Paddle bounce")]
        [SerializeField, Range(0f, 0.5f)] private float leftZone = 0.4f;
        [SerializeField, Range(0.5f, 1f)] private float rightZone = 0.6f;
        [SerializeField, Range(0f, 45f)] private float minimumSideAngle = 10f;

        private Rigidbody2D rb;
        private BallController controller;
        private Vector2 velocityBeforeCollision;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            controller = GetComponent<BallController>();
        }

        private void FixedUpdate()
        {
            velocityBeforeCollision = rb.linearVelocity;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!controller.Launched || collision.contactCount == 0 ||
                velocityBeforeCollision.sqrMagnitude < 0.0001f)
                return;

            ContactPoint2D contact = collision.GetContact(0);
            Vector2 normal = contact.normal;

            if (Vector2.Dot(velocityBeforeCollision, normal) >= 0f)
                return;

            Vector2 direction = Vector2.Reflect(velocityBeforeCollision, normal).normalized;

            // Tylko gorna powierzchnia paletki daje specjalne odbicie.
            if (collision.collider.CompareTag("Paddle") && normal.y > 0.5f)
            {
                if (GameManager.Instance.PowerUpsManager.CurrentPowerUp == EPowerUp.Catch)
                {
                    controller.CatchOnPaddle();
                    return;
                }

                Bounds bounds = collision.collider.bounds;
                float hitPosition = Mathf.InverseLerp(bounds.min.x, bounds.max.x, contact.point.x);
                float minX = Mathf.Sin(minimumSideAngle * Mathf.Deg2Rad);

                if (hitPosition < leftZone)
                {
                    direction.x = -Mathf.Max(Mathf.Abs(direction.x), minX);
                    direction.y = Mathf.Sqrt(Mathf.Max(0f, 1f - direction.x * direction.x));
                }
                else if (hitPosition > rightZone)
                {
                    direction.x = Mathf.Max(Mathf.Abs(direction.x), minX);
                    direction.y = Mathf.Sqrt(Mathf.Max(0f, 1f - direction.x * direction.x));
                }
            }

            controller.SetDirection(direction);
        }
    }
}
