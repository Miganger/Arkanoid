using UITransition.Enums;
using UnityEngine;

namespace UITransition
{
    [RequireComponent(typeof(Animator))]
    public class UITransitionController : MonoBehaviour
    {
        private Animator animator;

        public bool IsScreenCovered { get; private set; } = false;
        public bool IsScreenRevealed { get; private set; } = false;

        private ETransition currentTransition;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void PlayTransition(ETransition transition)
        {
            if(transition == currentTransition)
            {
                IsScreenRevealed = transition == ETransition.Reveal;
                IsScreenCovered = transition == ETransition.Cover;
                return;
            }

            switch (transition)
            {
                case ETransition.Reveal:
                    IsScreenRevealed = false;

                    ClearTriggers();
                    animator.SetTrigger(ETransition.Reveal.ToString());
                    break;

                case ETransition.Cover:
                    IsScreenCovered = false;

                    ClearTriggers();
                    animator.SetTrigger(ETransition.Cover.ToString());
                    break;
            }

            currentTransition = transition;
        }


        public void OnScreenCovered()
        {
            IsScreenCovered = true;
        }
        public void OnScreenRevealed()
        {
            IsScreenRevealed = true;
        }

        private void ClearTriggers()
        {
            animator.ResetTrigger(ETransition.Cover.ToString());
            animator.ResetTrigger(ETransition.Reveal.ToString());
        }
    }
}