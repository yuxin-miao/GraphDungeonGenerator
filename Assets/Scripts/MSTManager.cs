using UnityEngine;
using System.Collections.Generic;

public class MSTManager : MonoBehaviour
{
  private Color mstEdgeColor = Color.green;
  public List<VisualEdge> mstEdges = new List<VisualEdge>();

  public void GenerateMST(List<VisualEdge> visualEdges)
  {
    visualEdges.Sort((a, b) => a.Weight.CompareTo(b.Weight));  // Sort edges by weight

    Dictionary<Vector2, Vector2> parent = new Dictionary<Vector2, Vector2>();

    foreach (var edge in visualEdges)
    {
      Vector2 rootA = Find(edge.StartPoint, parent);
      Vector2 rootB = Find(edge.EndPoint, parent);

      if (rootA != rootB)
      {
        edge.EdgeColor = mstEdgeColor;
        mstEdges.Add(edge);
        Union(rootA, rootB, parent);
      }
    }
  }

  private Vector2 Find(Vector2 point, Dictionary<Vector2, Vector2> parent)
  {
    if (!parent.ContainsKey(point))
    {
      parent[point] = point;
    }
    if (parent[point] != point)
    {
      parent[point] = Find(parent[point], parent);
    }
    return parent[point];
  }

  private void Union(Vector2 a, Vector2 b, Dictionary<Vector2, Vector2> parent)
  {
    var rootA = Find(a, parent);
    var rootB = Find(b, parent);
    if (rootA != rootB)
    {
      parent[rootA] = rootB;
    }
  }
}
