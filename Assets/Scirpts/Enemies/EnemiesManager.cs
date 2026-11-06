using ObjectsPool;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class EnemiesManager : PooledMonoBehaviour
    {
        [Header("References")]
        [SerializeField] private List<Transform> spawnPoints;

        [Header("Dependencies")]
        [SerializeField] private LevelsManager levelsManager;

        private bool isActive = false;
        private float timer = 0;

        public void StartSpawning()
        {
            timer = 0;
            isActive = true;
        }
        public void StopSpawning()
        {
            isActive = false;
            ReleaseAllActive();
        }

        private void Update()
        {
            if (GameManager.Instance.StateMachine.CurrentStateID != Enums.EGameState.Gameplay)
                return;

            if (!isActive)
                return;

            if (GetActiveCount() >= levelsManager.CurrentLevelConfig.MaxEnemies)
                return;

            if(timer < levelsManager.CurrentLevelConfig.SpawnCooldown)
            {
                timer += Time.deltaTime;
            }
            else
            {
                SpawnEnemy();
                timer = 0;
            }
        }

        private void SpawnEnemy()
        {
            var enemy = Get() as EnemyController;
            var randSpawn = UnityEngine.Random.Range(0, spawnPoints.Count);
            var randEnemyID = UnityEngine.Random.Range(0, levelsManager.CurrentLevelConfig.EnemiesIDs.Count);

            enemy.Setup(
                (int)levelsManager.CurrentLevelConfig.EnemiesIDs[randEnemyID], 
                spawnPoints[randSpawn].position);
        }
    }
}