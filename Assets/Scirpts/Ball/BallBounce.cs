
using UnityEngine;

namespace Ball
{
    [RequireComponent(typeof(Rigidbody2D), typeof(BallController))]
    public class BallBounce : MonoBehaviour
    {
        public static event System.Action Bounce;

        public Vector2 NewDirection { get; set; }

        [Header("Paddle")]
        [SerializeField, Range(30f, 80f)]
        private float maxPaddleAngle = 65f;

        [Header("Ball direction limits")]
        [SerializeField, Range(0f, 20f)]
        private float minAngleFromVertical = 5f;

        [SerializeField, Range(45f, 85f)]
        private float maxAngleFromVertical = 70f;

        private Rigidbody2D rb;
        private BallController controller;

        private Vector2 velocityBeforeStep;
        private Vector2 incomingVelocity;
        private Vector2 combinedNormal;

        private bool bounceQueued;
        private bool paddleBounceQueued;
        private float paddleHit;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            controller = GetComponent<BallController>();
        }
        private void FixedUpdate()
        {
            // Rozstrzygamy wszystkie kontakty z poprzedniego
            // kroku fizyki jako jedno odbicie.
            if (bounceQueued && controller.Launched)
            {
                NewDirection = paddleBounceQueued
                    ? GetPaddleDirection()
                    : GetSurfaceDirection();

                controller.SetDirection(NewDirection);
            }

            ClearPendingBounce();

            // Prêdkoœæ przed nastêpn¹ symulacj¹ fizyki.
            velocityBeforeStep = rb.linearVelocity;
        }
        public Vector2 CalculateBounce()
        {
            return GetPaddleDirection();
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            Bounce?.Invoke();

            if (!controller.Launched ||
                collision.contactCount == 0 ||
                velocityBeforeStep.sqrMagnitude < 0.0001f)
                return;

            if (collision.collider.CompareTag("Paddle"))
            {
                ContactPoint2D contact = collision.GetContact(0);
                Bounds bounds = collision.collider.bounds;

                if (velocityBeforeStep.y < 0f &&
                    contact.point.y >= bounds.center.y)
                {
                    paddleHit = Mathf.InverseLerp(
                        bounds.min.x,
                        bounds.max.x,
                        contact.point.x) * 2f - 1f;

                    if (controller.IsCatchActive)
                    {
                        ClearPendingBounce();
                        controller.CatchOnPaddle();
                        return;
                    }

                    paddleBounceQueued = true;
                    bounceQueued = true;
                    return;
                }
            }

            // Odbicie od œcian i klocków.
            // Paletka ma pierwszeñstwo, jeœli dotknêliœmy
            // kilku obiektów w tym samym kroku fizyki.
            if (paddleBounceQueued)
                return;

            bool foundContact = false;

            for (int i = 0; i < collision.contactCount; i++)
            {
                Vector2 normal = collision.GetContact(i).normal;

                if (Vector2.Dot(velocityBeforeStep, normal) < 0f)
                {
                    if (!bounceQueued)
                        incomingVelocity = velocityBeforeStep;

                    combinedNormal += normal;
                    foundContact = true;
                }
            }

            if (foundContact)
                bounceQueued = true;
        }
        private void OnDisable()
        {
            ClearPendingBounce();
            velocityBeforeStep = Vector2.zero;
        }

        private Vector2 GetPaddleDirection()
        {
            float angle = paddleHit * maxPaddleAngle;
            float radians = angle * Mathf.Deg2Rad;

            return new Vector2(
                Mathf.Sin(radians),
                Mathf.Cos(radians)
            );
        }
        private Vector2 GetSurfaceDirection()
        {
            Vector2 direction = incomingVelocity.normalized;

            float x = Mathf.Abs(combinedNormal.x);
            float y = Mathf.Abs(combinedNormal.y);

            bool flipX;

            // Niejasny naro¿nik: wybieramy oœ na podstawie
            // dominuj¹cego kierunku nadlatuj¹cej pi³ki.
            if (Mathf.Abs(x - y) < 0.01f)
                flipX = Mathf.Abs(direction.x) > Mathf.Abs(direction.y);
            else
                flipX = x > y;

            if (flipX)
                direction.x = -direction.x;
            else
                direction.y = -direction.y;

            return LimitAngle(direction);
        }
        private Vector2 LimitAngle(Vector2 direction)
        {
            direction.Normalize();

            float minX = Mathf.Sin(
                minAngleFromVertical * Mathf.Deg2Rad
            );

            float maxX = Mathf.Sin(
                maxAngleFromVertical * Mathf.Deg2Rad
            );

            float absX = Mathf.Clamp(
                Mathf.Abs(direction.x),
                minX,
                maxX
            );

            float signX = direction.x < 0f ? -1f : 1f;
            float signY = direction.y < 0f ? -1f : 1f;

            float absY = Mathf.Sqrt(1f - absX * absX);

            return new Vector2(
                signX * absX,
                signY * absY
            );
        }
        private void ClearPendingBounce()
        {
            bounceQueued = false;
            paddleBounceQueued = false;
            combinedNormal = Vector2.zero;
        }
    }
}