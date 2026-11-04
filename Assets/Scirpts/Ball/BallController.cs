using System;
using Enums;
using ObjectsPool;
using UnityEngine;

namespace Ball
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class BallController : PooledObject
    {
        public static event System.Action<BallController> OnBallDestroy;

        public bool Launched { get; private set; }
        public Vector2 Direction => rb.linearVelocity.normalized; 
        public bool IsCatchActive => powerUpsManager.CurrentPowerUp == EPowerUp.Catch;

        [Header("References")]
        [SerializeField] private Rigidbody2D rb;

        [Header("Gameplay")]
        [SerializeField] private float outOfBoundsY = -6f;
        [SerializeField, Range(0f, 1f)] private float launchHorizontalRange = 0.2f;

        private Transform followPosition;
        private Vector2 offset;

        private float baseBallSpeed;
        private PowerUpsManager powerUpsManager;

        public void Initialize(float baseBallSpeed, PowerUpsManager powerUpsManager)
        {
            this.baseBallSpeed = baseBallSpeed;
            this.powerUpsManager = powerUpsManager;
        }

        private void Awake()
        {
            if (rb == null)
                rb = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            PowerUpsManager.PowerUpActive += OnPowerUpActive;
            PowerUpsManager.PowerUpEnd += OnPowerUpEnd;
        }
        private void OnDisable()
        {
            PowerUpsManager.PowerUpActive -= OnPowerUpActive;
            PowerUpsManager.PowerUpEnd -= OnPowerUpEnd;
        }

        public void OnSpawn(Transform paddleAnchor)
        {
            if (paddleAnchor == null)
                throw new ArgumentNullException(nameof(paddleAnchor));

            // Kolejnosc ma znaczenie: referencja musi istniec przed kolejnymi operacjami.
            followPosition = paddleAnchor;
            offset = Vector2.zero;

            MakeStatic(true);
            transform.position = paddleAnchor.position;
        }

        public void Launch()
        {
            if (Launched || followPosition == null)
                return;

            Vector2 initialDirection = new Vector2(
                UnityEngine.Random.Range(-launchHorizontalRange, launchHorizontalRange),
                1f
            );

            MakeStatic(false);
            offset = Vector2.zero;
            SetDirection(initialDirection);
        }

        public void LaunchSplit(
            Transform paddleAnchor,
            Vector2 spawnPosition,
            Vector2 direction)
        {
            if (direction.sqrMagnitude < 0.0001f)
                throw new ArgumentException("Invalid ball direction.");

            // Zapamiêtujemy paletkê dla mechaniki Catch.
            followPosition = paddleAnchor;

            MakeStatic(false);
            offset = Vector2.zero;

            // Ustawiamy pozycjê i prêdkoœæ nowej pi³ki.
            rb.position = spawnPosition;
            SetDirection(direction);
        }

        // Wywoluje BallBounce po odbiciu od paletki z aktywnym Catch.
        public void CatchOnPaddle()
        {
            if (!Launched || followPosition == null)
                return;

            offset = rb.position - (Vector2)followPosition.position;
            MakeStatic(true);
        }

        // BallBounce ustala kierunek, BallController nadaje prawidlowa szybkosc.
        public void SetDirection(Vector2 direction)
        {
            if (!Launched || direction.sqrMagnitude < 0.0001f)
                return;

            rb.linearVelocity = direction.normalized * GetBallSpeed();
        }

        // Zachowana publiczna metoda, aby nie lamac obecnych wywolan.
        public void MakeStatic(bool enable)
        {
            Launched = !enable;
            rb.bodyType = enable ? RigidbodyType2D.Kinematic : RigidbodyType2D.Dynamic;

            if (enable)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }
        }

        public override void ReturnToPool()
        {
            MakeStatic(true);
            offset = Vector2.zero;
            followPosition = null;
            base.ReturnToPool();
        }

        private float GetBallSpeed()
        {
            bool slowed = powerUpsManager.CurrentPowerUp
                          == EPowerUp.SpeedDown;

            return slowed ? baseBallSpeed * 0.5f : baseBallSpeed;
        }

        private void OnPowerUpActive(EPowerUp powerUp)
        {
            if (powerUp == EPowerUp.SpeedDown)
                RefreshSpeed();
        }
        private void OnPowerUpEnd(EPowerUp powerUp)
        {
            if (powerUp == EPowerUp.SpeedDown)
                RefreshSpeed();

            if (powerUp == EPowerUp.Catch)
                offset = Vector2.zero;
        }

        private void RefreshSpeed()
        {
            if (Launched && rb.linearVelocity.sqrMagnitude > 0.0001f)
                SetDirection(rb.linearVelocity);
        }

        private void CheckBallSpeed()
        {
            if (rb.linearVelocity.magnitude <= 0.0001f)
                SetDirection(UnityEngine.Random.insideUnitCircle.normalized);

            if (rb.linearVelocity.magnitude <= GetBallSpeed())
                RefreshSpeed();
        }

        private void FixedUpdate()
        {
            if (!Launched)
            {
                if (followPosition != null)
                    rb.MovePosition((Vector2)followPosition.position + offset);

                return;
            }

            CheckBallSpeed();

            if (rb.position.y >= outOfBoundsY)
                return;

            ReturnToPool();
            OnBallDestroy?.Invoke(this);
        }
    }
}