using Blocks;
using ObjectsPool;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelsManager : PooledMonoBehaviour
{
    public float BaseBallSpeed { get;private set; }

    [Header("Configuration")]
    [SerializeField] private List<LevelConfiguration> configs;

    private List<BlockController> blocksToWin = new();

    private void OnEnable()
    {
        BlockController.BlockDestroyed += OnBlockDestroyed;
    }
    private void OnDisable()
    {
        BlockController.BlockDestroyed -= OnBlockDestroyed;
    }

    private void OnBlockDestroyed(Vector3 _)
    {
        if(blocksToWin.Count == 0)
        {
            GameManager.Instance.EnableNextLevel();
        }
    }

    public void GenerateLevel(int levelID)
    {
        var config = configs[levelID];

        BaseBallSpeed = config.BaseBallSpeed;

        var blocks = config.GetBlocksMatrix();

        var height = blocks.GetLength(0);
        var width = blocks.GetLength(1);

        float startX = -6;
        float startY = 4;

        float xStep = 1;
        float yStep = .5f;

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                BlockData block = blocks[y, x];

                if (block == null)
                    continue;

                var pos = new Vector3(
                    startX + (xStep * x),
                    startY - (yStep * y),
                    0);

                var blockObject = Get() as BlockController;
                blockObject.transform.localPosition = pos;
                blockObject.Setup(block.BlockHP, block.BlockColor);

                if (block.BlockHP < 3)
                    blocksToWin.Add(blockObject);
            }
        }
    }

    public void ClearLevel()
    {
        blocksToWin.Clear();
        ReleaseAllActive();
    }

    internal int GetMaxLevel()
    {
        return configs.Count - 1;
    }
}