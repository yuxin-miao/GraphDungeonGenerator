using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class NodeManager : MonoBehaviour
{
  public GameObject nodePrefab; 

  public List<GameObject> nodes = new List<GameObject>();
  MSTManager mstManager;
  // Final edges that would be used to generate hallways. Will be initialized by mstEdges 
  public List<VisualEdge> finalEdges = new List<VisualEdge>();

  public void InitializeFinalEdges()
  {
    finalEdges.AddRange(mstManager.mstEdges);
  }
  TriangulationManager triangulationManager;
  // Max attempts to reposition a new node if it overlaps with other nodes
  private const int maxPositionAttempts = 100;

  private GameObject selectedNode = null;
  private void Awake()
  {
    triangulationManager = FindObjectOfType<TriangulationManager>();
    mstManager = FindObjectOfType<MSTManager>();
  }

  public void AddNode()
  {
    // Get the center of the screen
    Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
    Vector3 newPosition = Camera.main.ScreenToWorldPoint(screenCenter);
    newPosition.z = 0; 

    // Ensure the new node does not overlap with existing nodes
    if (IsPositionClear(newPosition))
    {
      nodes.Add(Instantiate(nodePrefab, newPosition, Quaternion.identity));
    }
    else
    {
      int attempts = 0;
      while (attempts < maxPositionAttempts)
      {
        // Random screen position offset from the center of the screen
        Vector3 randomScreenOffset = new Vector3(Random.Range(-Screen.width / 2, Screen.width / 2), Random.Range(-Screen.height / 2, Screen.height / 2), 0);
        Vector3 randomScreenPosition = screenCenter + randomScreenOffset;

        // Convert that random screen position to world space
        Vector3 tryPosition = Camera.main.ScreenToWorldPoint(randomScreenPosition);
        tryPosition.z = 0;

        if (IsPositionClear(tryPosition))
        {
          nodes.Add(Instantiate(nodePrefab, tryPosition, Quaternion.identity));
          break;
        }

        attempts++;
      }

      if (attempts == maxPositionAttempts)
      {
        Debug.LogWarning("Could not find a clear position for the new node.");
      }
    }
  }


  void Update()
  {
    if (Input.GetMouseButtonDown(1))  // Right mouse button
    {
      RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

      if (hit.collider != null && hit.collider.gameObject.CompareTag("Node"))
      {
        selectedNode = hit.collider.gameObject;
        Destroy(selectedNode);  // Delete the node on right click
        nodes.Remove(selectedNode);
      }
    }
  }

  public void FinishUserNodesProcedure()
  {
    // Hide the Add and Finish buttons.
    foreach (GameObject node in nodes)
    {
      DraggableNode draggableNode = node.GetComponent<DraggableNode>();
      if (draggableNode)
      {
        draggableNode.SetDraggable(false);
      }
    }


    // When user input is finished, perfrom triangulation 
    triangulationManager.PerformTriangulation();

  }

  // Check if a given position is clear of nodes
  private bool IsPositionClear(Vector3 position)
  {
    float radius = nodePrefab.transform.localScale.x / 2; // Assuming nodes are roughly circular in shape
    Collider2D[] overlappingColliders = Physics2D.OverlapCircleAll(position, radius);

    foreach (Collider2D collider in overlappingColliders)
    {
      if (collider.CompareTag("Node"))
      {
        return false;
      }
    }

    return true;
  }


  public void OnMSTButtonPressed()
  {
    mstManager.GenerateMST(triangulationManager.triangulatedEdges);
    triangulationManager.DrawTriangulatedEdges(mstManager.mstEdges);
    InitializeFinalEdges();
  }

}


