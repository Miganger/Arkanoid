
using Blocks;
using Enums;
using ObjectsPool;
using PowerUps;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpsManager : PooledMonoBehaviour
{
    public static Action<EPowerUp> PowerUpActive;
    public static Action<EPowerUp> PowerUpEnd;

    public EPowerUp CurrentPowerUp { get; private set; } = EPowerUp.None;

    [SerializeField, Range(0, 100)]
    private int powerUpSpawnChance = 1;

    internal void ApplyPowerUp(EPowerUp powerUpType)
    {
        bool wasActive = CurrentPowerUp == powerUpType;

        // Aktywujemy efekt tylko za pierwszym razem
        if (!wasActive)
        {
            DeactivePowerUp();

            CurrentPowerUp = powerUpType;

            PowerUpActive?.Invoke(powerUpType);

            if (powerUpType == EPowerUp.PlayerExtend || 
                powerUpType == EPowerUp.Disruption)
            {
                DeactivePowerUp();
                return;
            }
        }
    }

    public void DeactivePowerUp()
    {
        if (CurrentPowerUp != EPowerUp.None)
        {
            PowerUpEnd?.Invoke(CurrentPowerUp);
            CurrentPowerUp = EPowerUp.None;
        }
    }

    private void OnEnable()
    {
        BlockController.BlockDestroyed += OnBlockDestroyed;
    }
    private void OnDisable()
    {
        BlockController.BlockDestroyed -= OnBlockDestroyed;
    }

    private void OnBlockDestroyed(BlockController blockController)
    {
        SpawnRandomPowerUp(blockController.transform.position);
    }

    private void SpawnRandomPowerUp(Vector3 position)
    {
        var randValue = UnityEngine.Random.Range(0, 100);
        if (randValue >= powerUpSpawnChance)
            return;

        var powerUp = Get() as PowerUpObject;
        var randID = UnityEngine.Random.Range(0, 7);
        powerUp.Setup(position, (EPowerUp)randID);
    }

    public void ClearPowerUps()
    {
        DeactivePowerUp();
        ReleaseAllActive();
    }
}