using Blocks;
using ObjectsPool;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelsManager : PooledMonoBehaviour
{
    public float BaseBallSpeed { get; private set; }

    [Header("Configuration")]
    [SerializeField] private List<LevelConfiguration> configs;

    [Header("Settings")]
    [SerializeField] private float startX = -6;
    [SerializeField] private float startY = 4;
    [SerializeField] private float xStep = 1;
    [SerializeField] private float yStep = .5f;
    [SerializeField] private int maxRows = 13;
    [SerializeField] private int maxCols = 13;

    [Header("Debug")]
    [SerializeField] private Vector3 boxSize;

    private List<BlockController> blocksToWin = new();

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
        if (blocksToWin.Contains(blockController))
            blocksToWin.Remove(blockController);

        if (blocksToWin.Count == 0)
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

        for (var y = 0; y < height; y++)
        {
            var newY = startY - (yStep * (y + 1));
            for (var x = 0; x < width; x++)
            {
                var newX = startX + (xStep * (x + 1));

                BlockData block = blocks[y, x];

                if (block == null)
                    continue;

                var pos = new Vector3(newX, newY, 0);

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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        for (int j = 1; j <= maxCols; j++)
        {
            var newY = startY - (yStep * j);
            for (int i = 1; i <= maxRows; i++)
            {
                var newX = startX + (xStep * i);
                Gizmos.DrawWireCube(new Vector3(newX, newY), boxSize);
            }
        }
    }
}