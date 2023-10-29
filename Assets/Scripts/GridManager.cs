using UnityEngine;

public class GridManager : MonoBehaviour
{
  public Camera cam;
  public Vector2 gridSize;
  public Vector2 cellSize;
  public GridType[,] grid;

  public enum GridType
  {
    Empty,
    Room
  }

  private void Start()
  {
    cellSize = new Vector2(0.2f, 0.2f);
    DetermineGridSize();
  }
  private void DetermineGridSize()
  {
    float widthInWorldUnits = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0)).x - Camera.main.ScreenToWorldPoint(Vector3.zero).x;
    float heightInWorldUnits = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height)).y - Camera.main.ScreenToWorldPoint(Vector3.zero).y;

    gridSize.x = Mathf.CeilToInt(widthInWorldUnits / cellSize.x);
    gridSize.y = Mathf.CeilToInt(heightInWorldUnits / cellSize.y);
  }


  public void InitializeGrid()
  {
    Vector2 totalGridSize = new Vector2(gridSize.x * cellSize.x, gridSize.y * cellSize.y);
    Vector2 start = (Vector2)cam.transform.position - totalGridSize * 0.5f;

    grid = new GridType[(int)gridSize.x, (int)gridSize.y];

    for (int x = 0; x < gridSize.x; x++)
    {
      for (int y = 0; y < gridSize.y; y++)
      {
        grid[x, y] = GridType.Empty;

        Vector2 cellCenter = start + new Vector2(x * cellSize.x + cellSize.x * 0.5f, y * cellSize.y + cellSize.y * 0.5f);
        Vector2 cellBottomLeft = cellCenter - cellSize * 0.5f;
        Vector2 cellTopRight = cellCenter + cellSize * 0.5f;

        Collider2D[] colliders = Physics2D.OverlapBoxAll(cellCenter, cellSize, 0);

        foreach (Collider2D collider in colliders)
        {
          if (collider.CompareTag("Node"))
          {
            Vector2 nodeSize = collider.bounds.size;
            Vector2 nodeCenter = collider.transform.position;
            Vector2 nodeBottomLeft = nodeCenter - nodeSize * 0.5f;
            Vector2 nodeTopRight = nodeCenter + nodeSize * 0.5f;

            float overlapArea = Mathf.Max(0, Mathf.Min(cellTopRight.x, nodeTopRight.x) - Mathf.Max(cellBottomLeft.x, nodeBottomLeft.x))
                               * Mathf.Max(0, Mathf.Min(cellTopRight.y, nodeTopRight.y) - Mathf.Max(cellBottomLeft.y, nodeBottomLeft.y));

            if (overlapArea / (cellSize.x * cellSize.y) >= 0.5f)  // Check for 50% overlap
            {
              grid[x, y] = GridType.Room;
              break;
            }
          }
        }
      }
    }
  }


  void OnDrawGizmos()
  {
    if (grid != null)
    {
      Vector2 totalGridSize = new Vector2(gridSize.x * cellSize.x, gridSize.y * cellSize.y);
      Vector2 start = (Vector2)cam.transform.position - totalGridSize * 0.5f;

      for (int x = 0; x < gridSize.x; x++)
      {
        for (int y = 0; y < gridSize.y; y++)
        {
          Vector2 cellCenter = start + new Vector2(x * cellSize.x + cellSize.x * 0.5f, y * cellSize.y + cellSize.y * 0.5f);

          if (grid[x, y] == GridType.Room)
          {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(cellCenter, cellSize);
          }
        }
      }
    }
  }
}
