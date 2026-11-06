using Enums;
using System;
using System.Collections;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Paddle
{
    public class PaddleController : MonoBehaviour
    {
        public static event System.Action Hit;

        public PaddleFireController FireController => fireController;

        [Header("Settings")]
        [SerializeField] private float baseWidth = 1.25f;
        [SerializeField] private float immuneTime = 1f;

        [Header("References")]
        [SerializeField] private PaddleMovement paddleMovement;
        [SerializeField] private PaddleFireController fireController;
        [SerializeField] private PaddleAnimationController animationController;
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private CapsuleCollider2D capsuleCollider;
        [SerializeField] private SpriteRenderer spriteRenderer;

        private float immuneTimer = 0;

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

        private void OnPowerUpActive(EPowerUp powerUp)
        {
            if (powerUp == EPowerUp.Laser)
            {
                animationController.EnableFireMode(true);
            }
            if (powerUp == EPowerUp.Expand)
            {
                UpdateWidth(2f);
            }
        }
        private void OnPowerUpEnd(EPowerUp powerUp)
        {
            if (powerUp == EPowerUp.Laser)
            {
                animationController.EnableFireMode(false);

            }
            if (powerUp == EPowerUp.Expand)
            {
                UpdateWidth(baseWidth);
            }
        }

        public void EnableActions()
        {
            paddleMovement.EnableActions();
            fireController.EnableActions();
        }
        public void DisableActions()
        {
            paddleMovement.DisableActions();
            fireController.DisableActions();
        }

        public void EnablePaddle()
        {
            transform.position = new Vector3(0, transform.position.y, 0);
            gameObject.SetActive(true);
        }
        public void DisablePaddle()
        {
            fireController.ClearMissles();
            gameObject.SetActive(false);
        }

        private void Start()
        {
            UpdateWidth(baseWidth);
            DisablePaddle();
        }
        private void Update()
        {
            if (immuneTimer > 0)
                immuneTimer -= Time.deltaTime;
        }

        private void UpdateWidth(float width)
        {
            capsuleCollider.size = new Vector2(width, capsuleCollider.size.y);
            spriteRenderer.size = new Vector2(width, spriteRenderer.size.y);
        }
        internal void OnHit()
        {
            if (immuneTimer > 0)
                return;

            Time.timeScale = 0f;
            StartCoroutine(DeathSequence());
            Hit?.Invoke();
        }
        private IEnumerator DeathSequence()
        {
            DisableActions();
            animationController.PlayDeath();
            yield return new WaitUntil(() => animationController.DeathEnd);
            yield return new WaitForSecondsRealtime(1f);
            transform.position = new Vector3(0f, -3.5f, 0f);
            animationController.PlaySpawn();
            yield return new WaitUntil(() => animationController.SpawnEnd);
            yield return new WaitForSecondsRealtime(1f);
            Time.timeScale = 1f;
            EnableActions();
            immuneTimer = immuneTime;
        }
    }
}