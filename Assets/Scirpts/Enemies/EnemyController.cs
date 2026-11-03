using ObjectsPool;
using UnityEngine;

namespace Enemies
{
    public class EnemyController : PooledObject
    {
        public static event System.Action EnemyDeath;

        [Header("Gameplay")]
        [SerializeField] private float outOfBoundsY = -6f;

        [Header("References")]
        [SerializeField] private EnemyMovement movement;
        [SerializeField] private EnemyAnimationController animationController;
        [SerializeField] private CircleCollider2D circleCollider;

        public void Setup(int ID, Vector3 spawnPoint)
        {
            transform.position = spawnPoint;
            animationController.SetModel(ID);
            circleCollider.enabled = true;
            movement.StartMove();
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.CompareTag("Missle") || 
                collision.transform.CompareTag("Paddle") || 
                collision.transform.CompareTag("Ball"))
            {
                movement.Stop();
                animationController.PlayDeath();
                circleCollider.enabled = false;
                EnemyDeath?.Invoke();
            }
        }
        private void FixedUpdate()
        {
            if (transform.position.y >= outOfBoundsY)
                return;

            ReturnToPool();
        }
    }
}