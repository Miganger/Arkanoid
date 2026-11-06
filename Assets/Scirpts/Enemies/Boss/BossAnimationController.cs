using UnityEngine;

namespace Enemies.Boss
{
    public class BossAnimationController : MonoBehaviour
    {
        public event System.Action Hit;
        public event System.Action Death;

        [SerializeField] private Animator animator;

        public void PlayHit()
        {
            animator.SetTrigger("Hit");
        }
        public void SetBattleState(bool enable)
        {
            if (enable)
                animator.ResetTrigger("Hit");
            animator.SetBool("BattleState", enable);
        }
        public void PlayDeath()
        {
            animator.SetTrigger("Death");
        }
        public void DisableMe()
        {
            Death?.Invoke();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Hit?.Invoke();
        }
    }
}
