using UnityEngine;

namespace Paddle
{
    public class PaddleAnimationController : MonoBehaviour
    {
        private Animator animator;

        public bool DeathEnd { get; private set; } = false;
        public bool SpawnEnd { get; private set; } = false;

        private void Awake()
        {
            if(!animator)
                animator = GetComponent<Animator>();
        }

        public void EnableFireMode(bool enable)
        {
            animator.SetBool("BattleMode", enable);
        }

        public void PlayDeath()
        {
            DeathEnd = false;
            animator.SetTrigger("Death");
        }
        public void PlaySpawn()
        {
            SpawnEnd = false;
            animator.SetTrigger("Spawn");
        }

        public void OnDeathEnd()
        {
            DeathEnd = true;
        }
        public void OnSpawnEnd()
        {
            SpawnEnd = true;
        }
    }
}
