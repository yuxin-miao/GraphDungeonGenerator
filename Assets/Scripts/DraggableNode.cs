using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DraggableNode : MonoBehaviour
{
  private bool isDragging = false;
  private bool isResizing = false;
  private Vector3 offset;
  private Vector3 startingMousePosition;
  private Vector3 originalSize;
  private Vector3 originalPosition;

  // These thresholds define the width of the resizable borders.
  private float edgeThreshold = 0.1f;

  private enum ResizeDirection
  {
    None,
    VerticalUp,
    VerticalDown,
    HorizontalLeft,
    HorizontalRight
  }
  private ResizeDirection currentResizeDirection = ResizeDirection.None;

  void Update()
  {
    if (isDragging)
    {
      Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
      transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
    }

    if (isResizing)
    {
      Vector3 dragDelta = Camera.main.ScreenToWorldPoint(Input.mousePosition) - startingMousePosition;
      Vector3 newSize = originalSize;
      Vector3 newPosition = originalPosition;

      switch (currentResizeDirection)
      {
        case ResizeDirection.VerticalUp:
          newSize.y += dragDelta.y;
          newPosition.y = originalPosition.y + dragDelta.y / 2;
          break;
        case ResizeDirection.VerticalDown:
          newSize.y -= dragDelta.y;
          newPosition.y = originalPosition.y + dragDelta.y / 2;
          break;
        case ResizeDirection.HorizontalLeft:
          newSize.x -= dragDelta.x;
          newPosition.x = originalPosition.x + dragDelta.x / 2;
          break;
        case ResizeDirection.HorizontalRight:
          newSize.x += dragDelta.x;
          newPosition.x = originalPosition.x + dragDelta.x / 2;
          break;
      }

      transform.localScale = newSize;
      transform.position = newPosition;
    }
  }

  void OnMouseDown()
  {
    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    offset = transform.position - mousePosition;

    // Check if an edge was clicked for resizing.
    if (Mathf.Abs(mousePosition.x - transform.position.x) > (transform.localScale.x / 2) - edgeThreshold)
    {
      currentResizeDirection = mousePosition.x > transform.position.x ? ResizeDirection.HorizontalRight : ResizeDirection.HorizontalLeft;
    }
    else if (Mathf.Abs(mousePosition.y - transform.position.y) > (transform.localScale.y / 2) - edgeThreshold)
    {
      currentResizeDirection = mousePosition.y > transform.position.y ? ResizeDirection.VerticalUp : ResizeDirection.VerticalDown;
    }

    if (currentResizeDirection != ResizeDirection.None)
    {
      isResizing = true;
      startingMousePosition = mousePosition;
      originalSize = transform.localScale;
      originalPosition = transform.position;
      return;
    }

    isDragging = true;
  }

  void OnMouseUp()
  {
    isDragging = false;
    isResizing = false;
    currentResizeDirection = ResizeDirection.None;
  }
}
