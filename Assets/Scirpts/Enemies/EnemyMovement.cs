using UnityEngine;

namespace Enemies
{
    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField] private float speed = 4;
        [SerializeField] private float randomChangeDirectionTime = 3;

        private Rigidbody2D rb;
        private float timer = 0;

        private void Awake()
        {
            if (!rb)
                rb = GetComponent<Rigidbody2D>();

            ChangeDirection(-Vector3.up);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            ChangeToRandomDirection();
        }

        private void Update()
        {
            if (timer <= 0)
            {
                ChangeToRandomDirection();
                timer = randomChangeDirectionTime;
                return;
            }

            timer -= Time.deltaTime;
        }

        public void Stop()
        {
            rb.bodyType = RigidbodyType2D.Static;
        }
        public void StartMove()
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            timer = 0;
        }

        private void ChangeToRandomDirection()
        {
            var rand = UnityEngine.Random.Range(0, 2);
            var dirRand = UnityEngine.Random.Range(0, 2);
            if (rand == 0)
            {
                ChangeDirection(new Vector3(dirRand == 0 ? -1 : 1, 0, 0));
            }
            else
            {
                ChangeDirection(new Vector3(0, dirRand == 0 ? -1 : 1, 0));
            }
        }
        private void ChangeDirection(Vector3 dir)
        {
            if (rb.bodyType == RigidbodyType2D.Dynamic)
                rb.linearVelocity = dir.normalized * speed;
        }
    }
}