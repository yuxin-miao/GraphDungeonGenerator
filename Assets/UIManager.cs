using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{

  public NodeManager nodeManager;
  public Button addButton; // For user to add nodes at the beginning 
  public Button actionButton; // What would user do next 
  public GameObject processingPopup; // When it is under processing, dunno if needed  

  private enum Stage { AddNodes, MST, ModifyEdges }
  private Stage currentStage = Stage.AddNodes;

  private void Start()
  {
    processingPopup.SetActive(false);
    actionButton.GetComponentInChildren<Text>().text = "Finish";
    addButton.onClick.AddListener(nodeManager.AddNode);
    actionButton.onClick.AddListener(OnActionButtonClicked);

  }

  public void OnActionButtonClicked()
  {
    switch (currentStage)
    {
      case Stage.AddNodes:
        addButton.gameObject.SetActive(false);
        nodeManager.FinishUserNodesProcedure();

        currentStage = Stage.MST;
        actionButton.GetComponentInChildren<Text>().text = "Perform MST";

        break;

      case Stage.MST:
        nodeManager.OnMSTButtonPressed();

        currentStage = Stage.ModifyEdges;
        actionButton.GetComponentInChildren<Text>().text = "Modify Edges";
        break;

      case Stage.ModifyEdges:

        break;
    }

  }

}

