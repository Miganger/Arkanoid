using Blocks;
using Enemies;
using Enemies.Boss;
using PowerUps;
using Saving;
using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static System.Action<uint> OnScoreUpdated;
    public static System.Action<uint> OnBestScoreUpdated;

    public uint Score
    {
        get
        {
            return score;
        }
        private set
        {
            score = value;
            OnScoreUpdated?.Invoke(score);

            if(Score > BestScore)
            {
                BestScore = Score;
                SaveSystem.Data.BestScore = BestScore;
            }
        }
    }
    public uint BestScore
    {
        get
        {
            return bestScore;
        }
        private set
        {
            bestScore = value;
            OnBestScoreUpdated?.Invoke(bestScore);
        }
    }

    private uint score;
    private uint bestScore;

    private void OnEnable()
    {
        BlockController.BlockDestroyed += OnBlockDestroyed;
        PowerUpObject.PickedUp += OnPowerUpPickedUp;
        EnemyController.Death += OnEnemyDeath;
        BossController.Hit += OnBossHit;
    }
    private void OnDisable()
    {
        BlockController.BlockDestroyed -= OnBlockDestroyed;
        PowerUpObject.PickedUp -= OnPowerUpPickedUp;
        EnemyController.Death -= OnEnemyDeath;
        BossController.Hit -= OnBossHit;
    }

    private void OnBossHit()
    {
        Score += 1000;
    }

    private void OnEnemyDeath()
    {
        Score += 100;
    }

    private void OnPowerUpPickedUp()
    {
        Score += 1000;
    }
    private void OnBlockDestroyed(BlockController _)
    {
        Score += 80;
    }

    public void RestartScore()
    {
        Score = 0;
        LoadBestScore();
    }

    private void LoadBestScore()
    {
        if(BestScore == 0)
        {
            BestScore = SaveSystem.Data.BestScore;
        }
    }
}
