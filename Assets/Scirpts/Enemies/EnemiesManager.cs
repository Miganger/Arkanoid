using ObjectsPool;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class EnemiesManager : PooledMonoBehaviour
    {
        [Header("References")]
        [SerializeField] private List<Transform> spawnPoints;
        
        [Header("Settings")]
        [SerializeField] private float spawnCooldown = 5;
        [SerializeField] private int maxActive = 3;

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
            if (!isActive)
                return;

            if (GetActiveCount() >= maxActive)
                return;

            if(timer < spawnCooldown)
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
            var randEnemyID = UnityEngine.Random.Range(0, 5);
            enemy.Setup(randEnemyID, spawnPoints[randSpawn].position);
        }
    }
}