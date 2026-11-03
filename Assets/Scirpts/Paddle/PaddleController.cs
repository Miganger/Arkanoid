using Enums;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Paddle
{
    public class PaddleController : MonoBehaviour
    {
        public PaddleFireController FireController => fireController;

        [Header("Settings")]
        [SerializeField] private float baseWidth = 1.25f;

        [Header("References")]
        [SerializeField] private PaddleMovement paddleMovement;
        [SerializeField] private PaddleFireController fireController;
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private CapsuleCollider2D capsuleCollider;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Animator animator;

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
                EnableFireMode(true);
            }
            if(powerUp == EPowerUp.Expand)
            {
                UpdateWidth(2f);
            }
        }
        private void OnPowerUpEnd(EPowerUp powerUp)
        {
            if (powerUp == EPowerUp.Laser)
            {
                EnableFireMode(false);
            }
            if (powerUp == EPowerUp.Expand)
            {
                UpdateWidth(baseWidth);
            }
        }
        private void EnableFireMode(bool enable)
        {
            animator.SetBool("BattleMode", enable);
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

        private void UpdateWidth(float width)
        {
            capsuleCollider.size = new Vector2(width, capsuleCollider.size.y);
            spriteRenderer.size = new Vector2(width, spriteRenderer.size.y);
        }
    }
}