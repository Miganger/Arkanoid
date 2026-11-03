using UnityEngine;
using UnityEngine.InputSystem;

namespace Paddle
{
    public class PaddleMovement : MonoBehaviour
    {
        [Header("Input Actions")]
        [SerializeField] private InputActionReference moveAction;

        [Header("Settings")]
        [SerializeField] private float baseSpeed = 1f;

        [Header("References")]
        [SerializeField] private Rigidbody2D rb;

        public void EnableActions()
        {
            moveAction.action.Enable();
        }
        public void DisableActions()
        {
            moveAction.action.Disable();
        }

        private void Update()
        {
            var moveDir = moveAction.action.ReadValue<Vector2>();
            if (moveDir.x != 0)
            {
                rb.linearVelocity = transform.right * moveDir.x * baseSpeed;
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
    }
}