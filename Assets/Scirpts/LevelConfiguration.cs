using Enums;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfiguration", menuName = "Scriptable Objects/LevelConfiguration")]
public class LevelConfiguration : ScriptableObject
{
    [Header("Ball settings")]
    public float BaseBallSpeed = 9f;

    [Header("Blocks settings")]
    [TextArea(8, 8)] public string BlocksMatrix;
    [TextArea(8, 8)] public string ColorsMatrix;
    public List<Color> Colors;

    [Header("Enemies settings")]
    public List<EEnemyType> EnemiesIDs;
    public int MaxEnemies;
    public float SpawnCooldown;

    [Header("Bosses settings")]
    public EBossType BossID;

    public BlockData[,] GetBlocksMatrix()
    {
        if (string.IsNullOrWhiteSpace(BlocksMatrix))
            return null;

        if (string.IsNullOrWhiteSpace(ColorsMatrix))
            return null;

        int[,] blocksMatrix = GetIntMatrix(BlocksMatrix);
        int[,] colorsMatrix = GetIntMatrix(ColorsMatrix);

        int rows = blocksMatrix.GetLength(0);
        int columns = blocksMatrix.GetLength(1);

        // Sprawdzamy wymiary obu macierzy
        if (rows != colorsMatrix.GetLength(0) ||
            columns != colorsMatrix.GetLength(1))
        {
            throw new ArgumentException(
                "BlocksMatrix and ColorsMatrix have different dimensions."
            );
        }

        if (Colors == null)
            throw new InvalidOperationException(
                "Colors list is null."
            );

        BlockData[,] result = new BlockData[rows, columns];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                int blockHp = blocksMatrix[i, j];
                int colorIndex = colorsMatrix[i, j];

                if (blockHp < 0)
                    throw new FormatException(
                        $"Invalid block HP at [{i}, {j}]."
                    );

                // 0 HP oznacza puste miejsce
                if (blockHp == 0)
                    continue;

                // Sprawdzamy indeks koloru
                if (colorIndex < 0 || colorIndex >= Colors.Count)
                {
                    throw new IndexOutOfRangeException(
                        $"Invalid color index {colorIndex} at [{i}, {j}]."
                    );
                }

                result[i, j] = new BlockData(
                    blockHp,
                    Colors[colorIndex]
                );
            }
        }

        return result;
    }

    private int[,] GetIntMatrix(string layout)
    {

        string[] rows = layout.Trim()
            .Replace("\r", "")
            .Split('\n');

        int height = rows.Length;
        int width = SplitRow(rows[0]).Length;

        int[,] matrix = new int[height, width];

        for (int y = 0; y < height; y++)
        {
            string[] values = SplitRow(rows[y]);

            if (values.Length != width)
                throw new FormatException(
                    $"Invalid column count at row {y + 1}."
                );

            for (int x = 0; x < width; x++)
            {
                string value = values[x];

                if (!int.TryParse(value, out int intValue))
                {
                    throw new FormatException(
                        $"Invalid value '{value}' at row {y + 1}, column {x + 1}."
                    );
                }

                matrix[y, x] = intValue;
            }
        }

        return matrix;
    }

    private string[] SplitRow(string row)
    {
        return row.Split(
            new[] { ' ', '\t' },
            StringSplitOptions.RemoveEmptyEntries
        );
    }
}

public class BlockData
{
    public int BlockHP;
    public Color BlockColor;

    public BlockData(int blockHP, Color blockColor)
    {
        BlockHP = blockHP;
        BlockColor = blockColor;
    }
}