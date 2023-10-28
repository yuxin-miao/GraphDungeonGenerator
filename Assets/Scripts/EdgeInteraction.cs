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

      if (hit.collider != null && hit.collider.CompareTag("Edge"))
      {
        EdgeDataHolder edgeDataHolder = hit.collider.GetComponent<EdgeDataHolder>();
        if (edgeDataHolder && edgeDataHolder.EdgeData != null)
        {
          HandleEdgeClick(edgeDataHolder);
        }
      }
    }
  }
  private void HandleEdgeClick(EdgeDataHolder edgeDataHolder)
  {
    GameObject edgeGameObject = edgeDataHolder.gameObject;
    VisualEdge clickedEdge = edgeDataHolder.EdgeData;
    if (gameManager.finalEdges.Contains(clickedEdge))
    {
      gameManager.finalEdges.Remove(clickedEdge);

      if (edgeGameObject != null)
      {
        Destroy(edgeGameObject);
      }
    }
    else
    {
      clickedEdge.EdgeColor = Color.red;
      gameManager.finalEdges.Add(clickedEdge);
      gameManager.DrawEdge(clickedEdge);

    }
  }

}
