using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int width = 5;
    [SerializeField] private int height = 5;
    [SerializeField] private float tileSize = 0.5f;

    [SerializeField] private Tile tilePrefab;


    private Tile[,] grid;

    private void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        grid = new Tile[width, height];

        float xOffset = (width - 1) * tileSize * 0.5f;
        float yOffset = (height - 1) * tileSize * 0.5f;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector3 position = new Vector3(x * tileSize - xOffset, y * tileSize - yOffset, 0);
                Tile tile = Instantiate(tilePrefab, position, Quaternion.identity, transform);
                tile.Position = new Vector2Int(x, y);
                tile.name = $"Tile ({x}, {y})";
                grid[x, y] = tile;
            }
        }
    }

    public Tile GetTile(Vector2Int Position)
    {
        if (Position.x < 0 || Position.x >= width || Position.y < 0 || Position.y >= height)
        {
            return null;
        }

        return grid[Position.x, Position.y];

    }
}
