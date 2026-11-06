using ObjectsPool;
using UnityEngine;

namespace Enemies.Boss
{
    public class BossMisslesController : PooledMonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float misslesFireDelay = .2f;

        private bool isEnable = false;
        private float misslesTimer = 0;

        public void SetEnable(bool enable)
        {
            misslesTimer = 0;
            isEnable = enable;
        }

        private void OnDisable()
        {
            ReleaseAllActive();
        }

        private void Update()
        {
            if(!isEnable) 
                return;

            FireMissle();
        }

        private void FireMissle()
        {
            if (misslesTimer <= 0)
            {
                var missle = Get() as BossMissle;
                var direction = (GameManager.Instance.PaddleController.gameObject.transform.position - gameObject.transform.position).normalized;
                missle.SetDirection(transform.position, direction);
                misslesTimer = misslesFireDelay;
            }

            misslesTimer -= Time.deltaTime;
        }
    }
}