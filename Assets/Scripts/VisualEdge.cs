using UnityEngine;

[System.Serializable]  // This attribute lets Unity know that this class can be serialized and shown in the Inspector
public class VisualEdge
{
  public Vector2 StartPoint { get; private set; }
  public Vector2 EndPoint { get; private set; }
  public Color EdgeColor { get; set; }
  public float Weight;

  // Constructor
  public VisualEdge(Vector2 startPoint, Vector2 endPoint, Color edgeColor)
  {
    StartPoint = startPoint;
    EndPoint = endPoint;
    EdgeColor = edgeColor;
    Weight = Vector2.Distance(startPoint, endPoint);
  }
}
