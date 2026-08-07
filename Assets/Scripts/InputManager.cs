using UnityEngine;
using UnityEngine.Tilemaps;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    bool isDragging = false;
    Tile currentTile = null;
    Tile startTile = null;

    ColorType currentColor;

    [SerializeField] private GameObject linePrefab;

    public Sprite horizontalSprite;
    public Sprite verticalSprite;

    public Sprite cornerTopLeft;
    public Sprite cornerTopRight;
    public Sprite cornerBottomLeft;
    public Sprite cornerBottomRight;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckForNode();
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            Drag();
        }

        if (Input.GetMouseButtonUp(0))
        {
            StopDragging();
        }
    }
    void CheckForNode()
    {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Collider2D hit = Physics2D.OverlapPoint(worldPos);

        if (hit == null)
        {
            return;
        }

        Tile tile = hit.GetComponent<Tile>();

        if (tile == null)
        {
            return;
        }

        if (tile.Type != TileType.Node)
        {
            return;
        }



        startTile = tile;
        //reset node if line already drawn
        if (startTile.ConnectedTiles.Count > 1)
        {
            for (int i = 0; i < startTile.ConnectedTiles.Count; i++)
            {
                Destroy(startTile.ConnectedTiles[i].NormalLine);
            }
        }

        startTile.ConnectedTiles.Clear();



        startTile.ConnectedTiles.Add(tile);

        currentTile = startTile;
        currentColor = tile.Color;

        isDragging = true;

        Debug.Log("Started dragging tile");
    }

    private void Drag()
    {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Collider2D hit = Physics2D.OverlapPoint(worldPos);

        if (hit == null)
        {
            return;
        }

        Tile tile = hit.GetComponent<Tile>();

        if (tile == null)
        {
            return;
        }

        if (tile == currentTile)
        {
            return;
        }
        startTile.ConnectedTiles.Add(tile);

        SpawnLine(tile);

        if (startTile.ConnectedTiles.Count >= 3)
        {
            UpdateLineSprite(startTile.ConnectedTiles.Count - 2);
        }

        currentTile = tile;

        Debug.Log("Moved to " + tile.Position);

    }

    private void StopDragging()
    {
        isDragging = false;

        startTile = null;
        currentTile = null;
    }

    private void SpawnLine(Tile tile)
    {
        Tile previousTile = startTile.ConnectedTiles[startTile.ConnectedTiles.Count - 2];

        Vector2Int direction = tile.Position - previousTile.Position;

        tile.NormalLine = Instantiate(linePrefab, tile.transform.position, Quaternion.identity, tile.transform);

        SpriteRenderer sr = tile.NormalLine.GetComponent<SpriteRenderer>();

        if (direction == Vector2Int.left || direction == Vector2Int.right)
        {
            sr.sprite = horizontalSprite;
        }
        else if (direction == Vector2Int.up || direction == Vector2Int.down)
        {
            sr.sprite = verticalSprite;
        }

    }

    private void UpdateLineSprite(int index)
    {
        Tile current = startTile.ConnectedTiles[index];
        Tile previous = startTile.ConnectedTiles[index - 1];
        Tile next = startTile.ConnectedTiles[index + 1];

        // Directions FROM the current tile
        Vector2Int toPrevious = previous.Position - current.Position;
        Vector2Int toNext = next.Position - current.Position;

        SpriteRenderer sr = current.NormalLine.GetComponent<SpriteRenderer>();

        // Horizontal
        if ((toPrevious == Vector2Int.left && toNext == Vector2Int.right) ||
            (toPrevious == Vector2Int.right && toNext == Vector2Int.left))
        {
            sr.sprite = horizontalSprite;
        }
        // Vertical
        else if ((toPrevious == Vector2Int.up && toNext == Vector2Int.down) ||
                 (toPrevious == Vector2Int.down && toNext == Vector2Int.up))
        {
            sr.sprite = verticalSprite;
        }
        // Connects UP + RIGHT
        else if ((toPrevious == Vector2Int.up && toNext == Vector2Int.right) ||
                 (toPrevious == Vector2Int.right && toNext == Vector2Int.up))
        {
            sr.sprite = cornerBottomLeft;
        }
        // Connects UP + LEFT
        else if ((toPrevious == Vector2Int.up && toNext == Vector2Int.left) ||
                 (toPrevious == Vector2Int.left && toNext == Vector2Int.up))
        {
            sr.sprite = cornerBottomRight;
        }
        // Connects DOWN + RIGHT
        else if ((toPrevious == Vector2Int.down && toNext == Vector2Int.right) ||
                 (toPrevious == Vector2Int.right && toNext == Vector2Int.down))
        {
            sr.sprite = cornerTopLeft;
        }
        // Connects DOWN + LEFT
        else if ((toPrevious == Vector2Int.down && toNext == Vector2Int.left) ||
                 (toPrevious == Vector2Int.left && toNext == Vector2Int.down))
        {
            sr.sprite = cornerTopRight;
        }
    }
}