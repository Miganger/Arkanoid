using ObjectsPool;
using System;
using UnityEngine;

namespace Enemies.Boss
{
    public class BossController : PooledObject
    {
        public static event System.Action Death;
        public static event System.Action Hit;

        [Header("References")]
        [SerializeField] private BossAnimationController bossAnimator;
        [SerializeField] private BossMisslesController misslesController;

        [Header("General settings")]
        [SerializeField] private int baseHP = 10;

        [Header("Battle mode settings")]
        [SerializeField] private float battleModeInterval = 3;
        [SerializeField] private float battleModeDuration = 5;

        private int currentHP;
        private bool inBattleMode = false;

        private float timer = 0;
        private bool isDead = false;

        private void OnEnable()
        {
            bossAnimator.Hit += OnHit;
            bossAnimator.Death += OnDeath;
        }
        private void OnDisable()
        {
            bossAnimator.Hit -= OnHit;
            bossAnimator.Death -= OnDeath;
        }

        private void OnDeath()
        {
            ReturnToPool();
            Death?.Invoke();
        }

        public void Initialize()
        {
            ChangeToIdle();
            currentHP = baseHP;
            isDead = false;
            gameObject.SetActive(true);
        }

        private void Update()
        {
            if (isDead)
                return;

            if (GameManager.Instance.StateMachine.CurrentStateID != Enums.EGameState.Gameplay)
                return;

            if (currentHP <= 0)
                return;

            if (timer <= 0)
            {
                if (inBattleMode)
                    ChangeToIdle();
                else
                    ChangeToBattleMode();
            }

            timer -= Time.deltaTime;
        }

        private void ChangeToBattleMode()
        {
            bossAnimator.SetBattleState(true);
            inBattleMode = true;
            timer = battleModeDuration;
            misslesController.SetEnable(true);
        }
        private void ChangeToIdle()
        {
            bossAnimator.SetBattleState(false);
            inBattleMode = false;
            timer = battleModeInterval;
            misslesController.SetEnable(false);
        }


        private void OnHit()
        {
            if (isDead)
                return;

            if (inBattleMode)
            {
                currentHP--;
                Hit?.Invoke();
                if (currentHP <= 0)
                {
                    misslesController.SetEnable(false);
                    bossAnimator.PlayDeath();
                    GameManager.Instance.BallsController.ClearBalls();
                    isDead = true;
                }
                else
                {
                    bossAnimator.PlayHit();
                }
            }
        }
    }

}