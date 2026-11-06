using UnityEngine;
using ObjectsPool;
using System;

namespace Enemies.Boss
{
    public class BossManager : PooledMonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private LevelsManager levelsManager;

        private void OnEnable()
        {
            BossController.Death += OnDeath;
        }
        private void OnDisable()
        {
            BossController.Death -= OnDeath;
        }

        private void OnDeath()
        {
            if(GetActiveCount() == 0)
            {
                GameManager.Instance.WinEndGame();
            }
        }

        internal void StartSpawning()
        {
            var config = levelsManager.CurrentLevelConfig;

            if (config.BossID == Enums.EBossType.None)
                return;

            var boss = Get((int)config.BossID) as BossController;
            boss.Initialize();
        }

        internal void StopSpawning()
        {
            ReleaseAllActive();
        }
    }
}