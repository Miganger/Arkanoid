using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
    public abstract class StateMachine<T> : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private T initialState;
        [SerializeField] protected bool logTransitions = true;

        protected readonly Dictionary<T, State<T>> states = new();

        public State<T> CurrentState { get; private set; }

        public T? CurrentStateID => CurrentState != null ? CurrentState.ID : default(T);

        private bool initialized = false;

        private Coroutine changeStateCoroutine;

        public void RegisterStates()
        {
            if (initialized)
                return;

            State<T>[] foundStates = GetComponentsInChildren<State<T>>(true);

            foreach (State<T> state in foundStates)
            {
                if (states.ContainsKey(state.ID))
                {
                    Debug.LogError(
                        $"GameStateMachine: Istniej� dwa stany o ID {state.ID}.",
                        state
                    );

                    continue;
                }

                state.Initialize(this);
                states.Add(state.ID, state);
            }

            if (states.Count == 0)
            {
                Debug.LogError(
                    "GameStateMachine: Nie znaleziono �adnych stan�w.",
                    this
                );
            }

            initialized = true;
            ChangeState(initialState);
        }

        private IEnumerator ChangeStateCoroutine(T nextStateID)
        {
            if (!states.TryGetValue(nextStateID, out State<T> nextState))
            {
                Debug.LogError(
                    $"GameStateMachine: Stan {nextStateID} nie zosta� zarejestrowany.",
                    this
                );

                yield break;
            }

            // Nie wchodzimy ponownie do tego samego stanu.
            if (CurrentState == nextState)
            {
                yield break;
            }

            string previousStateName =
                CurrentState ? CurrentState.ID.ToString() : "None";

            yield return CurrentState?.ExitInternal();

            CurrentState = nextState;

            if (logTransitions)
            {
                Debug.Log(
                    $"Game state: {previousStateName} -> {nextStateID}",
                    this
                );
            }

            CurrentState.EnterInternal();
            changeStateCoroutine = null;
        }

        public void ChangeState(T nextStateID)
        {
            if(changeStateCoroutine != null)
            {
                Debug.LogError("You cannot change the state until you have finished changing the previous state.");
                return;
            }
            changeStateCoroutine = StartCoroutine(ChangeStateCoroutine(nextStateID));
        }

        private void Update()
        {
            CurrentState?.TickInternal();
        }
    }
}