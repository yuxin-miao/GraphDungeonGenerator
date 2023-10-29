using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{

  public Button addButton; // For user to add nodes at the beginning 
  public Button actionButton; // What would user do next 
  public GameObject processingPopup; // When it is under processing, dunno if needed  
  public GameManager gameManager;
  private NodeManager nodeManager;

  private void Awake()
  {
    nodeManager = gameManager.nodeManager;
  }
  private void Start()
  {
    processingPopup.SetActive(false);
    addButton.onClick.AddListener(nodeManager.AddNode);
    actionButton.onClick.AddListener(OnActionButtonClicked);

  }

  public void OnActionButtonClicked()
  {
    switch (gameManager.CurrentState)
    {
      case GameManager.GameState.NodePlacement:
        addButton.gameObject.SetActive(false);

        gameManager.SetState(GameManager.GameState.Triangulation);
        break;
      case GameManager.GameState.Triangulation:
        gameManager.SetState(GameManager.GameState.MSTGeneration);
        break;

      case GameManager.GameState.MSTGeneration:
        gameManager.SetState(GameManager.GameState.EdgeModification);
        break;

      case GameManager.GameState.EdgeModification:
        gameManager.SetState(GameManager.GameState.PathFinding);
        break;

      case GameManager.GameState.PathFinding:
        break;
    }

  }
  public void UpdateUIForNodePlacement()
  {
    actionButton.GetComponentInChildren<Text>().text = "Finish";
  }

  public void UpdateUIForTriangulation()
  {
    actionButton.GetComponentInChildren<Text>().text = "Perform MST";
  }

  public void UpdateUIForMST()
  {
    actionButton.GetComponentInChildren<Text>().text = "Modify Edges";
  }

  public void UpdateUIForEdge()
  {
    actionButton.GetComponentInChildren<Text>().text = "Find Path";
  }

  public void UpdateUIForPath()
  {
    actionButton.GetComponentInChildren<Text>().text = "AddPrefab";
  }
}

