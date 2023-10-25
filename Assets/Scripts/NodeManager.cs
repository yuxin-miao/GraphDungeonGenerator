using UnityEngine;
using UnityEngine.UI; // Required for UI elements

public class NodeManager : MonoBehaviour
{
  public GameObject nodePrefab;  // Assign your Node prefab in the inspector.
  public Button addButton;
  public Button finishButton;
  public GameObject processingPopup;  // A GameObject containing the UI for the "Processing..." popup.


  // Max attempts to reposition a new node if it overlaps with other nodes
  private const int maxPositionAttempts = 100;

  private GameObject selectedNode = null;

  void Start()
  {
    addButton.onClick.AddListener(AddNode);
    finishButton.onClick.AddListener(FinishProcedure);

    // Ensure the processing popup is initially hidden.
    processingPopup.SetActive(false);
  }

  public void AddNode()
  {
    // Get the center of the screen
    Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
    Vector3 newPosition = Camera.main.ScreenToWorldPoint(screenCenter);
    newPosition.z = 0;  // Ensure it's on the same Z plane as your other nodes

    // Ensure the new node does not overlap with existing nodes
    if (IsPositionClear(newPosition))
    {
      Instantiate(nodePrefab, newPosition, Quaternion.identity);
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
          Instantiate(nodePrefab, tryPosition, Quaternion.identity);
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
      }
    }
  }

  void FinishProcedure()
  {
    // Hide the Add and Finish buttons.
    addButton.gameObject.SetActive(false);
    finishButton.gameObject.SetActive(false);

    // Show the "Processing..." popup.
    processingPopup.SetActive(true);

    // Logic for when user input is finished.
    // After this, you'd probably also want to hide the processing popup at some point.
    // You can use a coroutine to simulate a delay, or you might hide it once actual processing is done.
    Debug.Log("User input finished!");
  }

  // Check if a given position is clear of nodes
  private bool IsPositionClear(Vector3 position)
  {
    Vector3 halfSize = nodePrefab.transform.localScale / 2; // Assuming the node has default scale when instantiated
    Collider2D[] overlappingColliders = Physics2D.OverlapAreaAll(position - halfSize, position + halfSize);

    foreach (Collider2D collider in overlappingColliders)
    {
      if (collider.CompareTag("Node"))
      {
        return false;
      }
    }

    return true;
  }
}
