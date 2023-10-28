using UnityEngine;
using System.Collections.Generic;
using TriangleNet.Geometry;

public class TriangulationManager : MonoBehaviour
{
  public GameManager gameManager;
  private NodeManager nodeManager;
  public List<VisualEdge> triangulatedEdges = new List<VisualEdge>();

  private void Awake()
  {
    if (nodeManager == null)
    {
      nodeManager = gameManager.nodeManager;
    }
  }


  public void PerformTriangulation()
  {
    if (nodeManager == null) return;

    var nodeObjects = nodeManager.nodes;

    Polygon polygon = new Polygon(); 

    foreach (var node in nodeObjects)
    {
      Vector3 pos = node.transform.position;
      polygon.Add(new Vertex(pos.x, pos.y));  // Adjust based on the actual method to add vertices.
    }

    // Triangulate
    TriangleNet.Meshing.IMesh triangulatedMesh = polygon.Triangulate();  // Adjust based on the actual method to triangulate.


    // Store the triangulated edges 
    foreach (var triangle in triangulatedMesh.Triangles)
    {
      AddEdgeIfUnique(new Vector2((float)triangle.GetVertex(0).X, (float)triangle.GetVertex(0).Y),
                      new Vector2((float)triangle.GetVertex(1).X, (float)triangle.GetVertex(1).Y));
      AddEdgeIfUnique(new Vector2((float)triangle.GetVertex(1).X, (float)triangle.GetVertex(1).Y),
                      new Vector2((float)triangle.GetVertex(2).X, (float)triangle.GetVertex(2).Y));
      AddEdgeIfUnique(new Vector2((float)triangle.GetVertex(2).X, (float)triangle.GetVertex(2).Y),
                      new Vector2((float)triangle.GetVertex(0).X, (float)triangle.GetVertex(0).Y));
    }

    gameManager.DrawTriangulatedEdges(triangulatedEdges); // Visualize the edges 

  }

  private void AddEdgeIfUnique(Vector2 start, Vector2 end)
  {
    // Checks for the existence of the edge or its reverse in the list.
    if (!triangulatedEdges.Exists(e => (e.StartPoint == start && e.EndPoint == end) || (e.StartPoint == end && e.EndPoint == start)))
    {
      triangulatedEdges.Add(new VisualEdge(start, end, Color.white));
    }
  }
}
