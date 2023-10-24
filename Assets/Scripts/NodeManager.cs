using UnityEngine;
using UnityEngine.UI; // Required for UI elements

public class NodeManager : MonoBehaviour
{
  public GameObject nodePrefab;  // Assign your Node prefab in the inspector.
  public Button addButton;
  public Button finishButton;
  public GameObject processingPopup;  // A GameObject containing the UI for the "Processing..." popup.


  private GameObject selectedNode = null;

  void Start()
  {
    addButton.onClick.AddListener(AddNode);
    finishButton.onClick.AddListener(FinishProcedure);

    // Ensure the processing popup is initially hidden.
    processingPopup.SetActive(false);
  }

  void AddNode()
  {
    Vector2 spawnPosition = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width / 2, Screen.height / 2));
    Instantiate(nodePrefab, spawnPosition, Quaternion.identity);
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
}
