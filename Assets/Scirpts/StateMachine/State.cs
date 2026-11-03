using System.Collections;
using UnityEngine;

namespace StateMachine
{
    public abstract class State<T> : MonoBehaviour
    {
        [field: SerializeField] public T ID { get; private set; }

        protected StateMachine<T> StateMachine { get; private set; }

        public bool IsActive { get; private set; }

        internal void Initialize(StateMachine<T> stateMachine)
        {
            StateMachine = stateMachine;
        }

        private Coroutine stateCoroutine;

        internal void EnterInternal()
        {
            IsActive = true;
            stateCoroutine = StartCoroutine(Enter());
        }

        internal void TickInternal()
        {
            if (!IsActive)
            {
                return;
            }

            Tick();
        }

        internal IEnumerator ExitInternal()
        {
            if (!IsActive)
                yield break;

            IsActive = false;

            if (stateCoroutine != null)
            {
                StopCoroutine(stateCoroutine);
                stateCoroutine = null;
            }

            yield return Exit();
        }

        /// <summary>
        /// Wywo³ywane raz po wejœciu do stanu.
        /// </summary>
        protected virtual IEnumerator Enter()
        {
            yield break;
        }

        /// <summary>
        /// Wywo³ywane co klatkê, gdy stan jest aktywny.
        /// Wiêkszoœæ stanów prawdopodobnie tego nie potrzebuje.
        /// </summary>
        protected virtual void Tick()
        {
        }

        /// <summary>
        /// Wywo³ywane raz przed opuszczeniem stanu.
        /// </summary>
        protected virtual IEnumerator Exit()
        {
            yield break;
        }

        protected void ChangeState(T nextState)
        {
            StateMachine.ChangeState(nextState);
        }
    }
}