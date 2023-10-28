using UnityEngine;
using System.Collections.Generic;
using TriangleNet.Geometry;

public class TriangulationManager : MonoBehaviour
{
  public GameManager gameManager;
  private NodeManager nodeManager;
  public GameObject linePrefab;
  public List<VisualEdge> triangulatedEdges = new List<VisualEdge>();

  private void Awake()
  {
    if (nodeManager == null)
    {
      nodeManager = gameManager.nodeManager;
    }
  }
  private void DrawEdge(VisualEdge edge)
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

    DrawTriangulatedEdges(triangulatedEdges); // Visualize the edges 

  }

  public void DrawTriangulatedEdges(List<VisualEdge> visualEdges)
  {
    foreach (var edge in visualEdges)
    {
      DrawEdge(edge);
    }
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
