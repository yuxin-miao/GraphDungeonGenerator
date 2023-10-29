using UnityEngine;
using System.Collections.Generic;
public class GameManager : MonoBehaviour
{
  public enum GameState
  {
    Initialization,
    NodePlacement,
    Triangulation,
    MSTGeneration,
    EdgeModification,
    PathFinding,
    AddPrefab,
    GameOver
  }

  public NodeManager nodeManager;
  public UIManager uiManager;
  public TriangulationManager triangulationManager;
  public MSTManager mstManager;
  public EdgeInteraction edgeInteraction;

  public GameObject linePrefab;
  public Color finalColor = Color.blue;
  // Final edges that would be used to generate hallways
  public List<VisualEdge> finalEdges;
  private void Start()
  {
    // Skip Initialization state for now, might add later 
    SetState(GameState.NodePlacement);

  }

  [SerializeField]
  private GameState currentState;


  public GameState CurrentState
  {
    get { return currentState; }
  }

  public void SetState(GameState newState)
  {
    // Exit logic for current state
    ExitCurrentState();

    // Update the current state
    currentState = newState;

    // Enter logic for the new state
    EnterNewState();
  }

  private void ExitCurrentState()
  {
    switch (currentState)
    {
      case GameState.NodePlacement:

        break;

    }
  }

  private void EnterNewState()
  {
    switch (currentState)
    {
      case GameState.Initialization:
        // Logic for initialization like setting up game parameters or resetting values.
        break;

      case GameState.NodePlacement:
        uiManager.UpdateUIForNodePlacement();
        break;
      case GameState.Triangulation:
        uiManager.UpdateUIForTriangulation();
        nodeManager.FinishUserNodesProcedure();
        break;

      case GameState.MSTGeneration:
        uiManager.UpdateUIForMST();
        mstManager.GenerateMST(triangulationManager.triangulatedEdges);
        DrawTriangulatedEdges(mstManager.mstEdges);
        break;

      case GameState.EdgeModification:
        uiManager.UpdateUIForEdge();
        InitializeFinalEdges();
        break;

      case GameState.PathFinding:
        uiManager.UpdateUIForPath();
        edgeInteraction.ClearNonFinalEdges();
        break;

      case GameState.GameOver:
        break;
    }
  }

  private void InitializeFinalEdges()
  {
    finalEdges = new List<VisualEdge>(mstManager.mstEdges);

    foreach (VisualEdge edge in finalEdges)
    {
      // Update the EdgeColor property
      edge.EdgeColor = finalColor;
      DrawEdge(edge);
 
    }
  }

  public void DrawEdge(VisualEdge edge)
  {
    GameObject lineObj = Instantiate(linePrefab);
    lineObj.transform.position = Vector3.zero;
    lineObj.transform.rotation = Quaternion.identity;

    LineRenderer lr = lineObj.GetComponent<LineRenderer>();
    BoxCollider2D boxCollider = lineObj.AddComponent<BoxCollider2D>();

    Vector3 start = new Vector3(edge.StartPoint.x, edge.StartPoint.y, 0);
    Vector3 end = new Vector3(edge.EndPoint.x, edge.EndPoint.y, 0);

    lr.SetPosition(0, start);
    lr.SetPosition(1, end);
    lr.startColor = edge.EdgeColor;
    lr.endColor = edge.EdgeColor;
    lr.startWidth = 0.1f;
    lr.endWidth = 0.1f;

    // Calculate the midpoint and rotation for the box collider
    Vector2 midPoint = (start + end) / 2;
    float angle = Mathf.Atan2(end.y - start.y, end.x - start.x) * Mathf.Rad2Deg;
    float length = Vector2.Distance(start, end);

    boxCollider.transform.position = midPoint;
    boxCollider.transform.Rotate(0, 0, angle);
    boxCollider.size = new Vector2(length, 0.1f);

    EdgeDataHolder edgeDataHolder = lineObj.GetComponent<EdgeDataHolder>();
    edgeDataHolder.EdgeData = edge;
  }
  public void DrawTriangulatedEdges(List<VisualEdge> visualEdges)
  {
    foreach (var edge in visualEdges)
    {
      DrawEdge(edge);
    }
  }
}
