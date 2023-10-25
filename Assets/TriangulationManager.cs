using UnityEngine;
using TriangleNet;
using TriangleNet.Geometry; // Make sure this namespace is available in the Unity-adapted version.

public class TriangulationManager : MonoBehaviour
{
  public NodeManager nodeManager;
  public GameObject linePrefab;

  private void Awake()
  {
    if (nodeManager == null)
    {
      nodeManager = FindObjectOfType<NodeManager>();
    }
  }
  public void DrawLine(Vector3 start, Vector3 end)
  {
    GameObject lineObj = Instantiate(linePrefab);
    LineRenderer lr = lineObj.GetComponent<LineRenderer>();

    lr.SetPosition(0, start);
    lr.SetPosition(1, end);
  }
  public void PerformTriangulation()
  {
    // Ensure there's a valid NodeManager reference before continuing.
    if (nodeManager == null) return;

    var nodeObjects = nodeManager.nodes;

    Polygon polygon = new Polygon();  // Assuming Polygon is the replacement for InputGeometry.

    foreach (var node in nodeObjects)
    {
      Vector3 pos = node.transform.position;
      polygon.Add(new Vertex(pos.x, pos.y));  // Adjust based on the actual method to add vertices.
    }

    // Triangulate
    TriangleNet.Meshing.IMesh mesh = polygon.Triangulate();  // Adjust based on the actual method to triangulate.

    // Visualization
    foreach (var triangle in mesh.Triangles)
    {
      var A = new Vector2((float)triangle.GetVertex(0).X, (float)triangle.GetVertex(0).Y);
      var B = new Vector2((float)triangle.GetVertex(1).X, (float)triangle.GetVertex(1).Y);
      var C = new Vector2((float)triangle.GetVertex(2).X, (float)triangle.GetVertex(2).Y);
      Debug.Log(A);
      DrawLine(A, B);
      DrawLine(B, C);
      DrawLine(C, A);
    }
  }
}
