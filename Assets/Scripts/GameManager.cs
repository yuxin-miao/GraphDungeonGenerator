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
    GameOver
  }

  public NodeManager nodeManager;
  public UIManager uiManager;
  public TriangulationManager triangulationManager;
  public MSTManager mstManager;
  //public EdgeInteraction edgeInteraction;

  // Final edges that would be used to generate hallways. Will be initialized by mstEdges 
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
        triangulationManager.DrawTriangulatedEdges(mstManager.mstEdges);
        break;

      case GameState.EdgeModification:
        uiManager.UpdateUIForEdge();
        finalEdges = new List<VisualEdge>(mstManager.mstEdges);
        break;

      case GameState.PathFinding:
        break;

      case GameState.GameOver:
        break;
    }
  }

}
