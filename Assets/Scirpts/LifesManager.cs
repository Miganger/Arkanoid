using Ball;
using Enums;
using Paddle;
using System;
using UI;
using UnityEngine;

public class LifesManager : MonoBehaviour
{
    public static event System.Action<int, int> LifesChanged;
    public static event System.Action<int> LifeLose;

    public int CurrentLifes { get; private set; }

    [SerializeField] private int baseLifes = 3;

    private void OnEnable()
    {
        PaddleController.Hit += OnPaddleHitted;
        BallsController.OnBallsDestroy += OnBallsDestroy;
        PowerUpsManager.PowerUpActive += OnPowerUpActive;
    }

    private void OnDisable()
    {
        PaddleController.Hit -= OnPaddleHitted;
        BallsController.OnBallsDestroy -= OnBallsDestroy;
        PowerUpsManager.PowerUpActive -= OnPowerUpActive;
    }

    private void OnPaddleHitted()
    {
        RemoveLife();
    }

    private void OnPowerUpActive(EPowerUp powerUp)
    {
        if(powerUp == EPowerUp.PlayerExtend)
        {
            AddLife();
        }
    }

    private void OnBallsDestroy(int amount)
    {
        if (amount == 0)
        {
            RemoveLife();
        }
    }

    private void RemoveLife()
    {
        CurrentLifes--;
        LifeLose?.Invoke(CurrentLifes);
        LifesChanged?.Invoke(baseLifes, CurrentLifes);
    }
    public void RestoreLifes()
    {
        CurrentLifes = baseLifes;
        LifesChanged?.Invoke(baseLifes, CurrentLifes);
    }
    public void AddLife()
    {
        CurrentLifes += 1;
        LifesChanged?.Invoke(baseLifes, CurrentLifes);
    }
}
