using UnityEngine;

namespace Enemies
{
    public class EnemyAnimationController : MonoBehaviour
    {
        [SerializeField] private EnemyController controller;

        private Animator animator;

        private void Awake()
        {
            if(!animator)
                animator = GetComponent<Animator>();
        }

        public void SetModel(int model)
        {
            animator.SetFloat("ID", model);
        }

        public void ReturnToPool()
        {
            controller.ReturnToPool();
        }

        public void PlayDeath()
        {
            animator.SetTrigger("Death");
        }
    }
}