using UnityEngine;
using System.Collections.Generic;

public class EdgeInteraction : MonoBehaviour
{
  public GameManager gameManager;

  private void Update()
  {
    if (gameManager.CurrentState != GameManager.GameState.EdgeModification)
      return;

    if (Input.GetMouseButtonDown(0))
    {
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      int layerMask = 1 << LayerMask.NameToLayer("Edges");
      RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, layerMask);

      Debug.Log("Input.GetMouseButtonDown(0)");
      if (hit.collider != null)
      {
        Debug.Log("Hit object: " + hit.collider.gameObject.name);
      }
      else
      {
        Debug.Log("No hit detected");
      }
      if (hit.collider != null && hit.collider.CompareTag("Edge"))
      {
        EdgeDataHolder edgeDataHolder = hit.collider.GetComponent<EdgeDataHolder>();
        if (edgeDataHolder && edgeDataHolder.EdgeData != null)
        {
          HandleEdgeClick(edgeDataHolder.EdgeData);
        }
      }
    }
  }

  private void HandleEdgeClick(VisualEdge clickedEdge)
  {
    if (gameManager.finalEdges.Contains(clickedEdge))
    {
      gameManager.finalEdges.Remove(clickedEdge);
    }
    else
    {
      gameManager.finalEdges.Add(clickedEdge);
    }
  }
}
