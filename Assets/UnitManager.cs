using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public RectTransform selectionPanelTransform;
    
    private bool isHoldingSelection;
    private Vector2 startSelectionPosition;
    private Vector2 currentSelectionPosition;
    
    
    private List<AgentMovement> selectedUnits = new List<AgentMovement>();
    private List<AgentMovement> SelectedUnits
    {
        get
        {
            return selectedUnits;
        }
        set
        {
            selectedUnits.ForEach(unit => unit.SetOutline(false));
            selectedUnits = value;
        }
    }
    
    private void Update()
    {
        if (isHoldingSelection)
        {
            UpdateSelectionVisual();
            
            if (Input.GetMouseButtonUp(0))
            {
                FinishSelectingUnits();
            }
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("Unit")))
            {
                AgentMovement unit = hit.collider.GetComponent<AgentMovement>();
                if (unit != null)
                {
                    UnselectAll();
                    SelectUnit(unit);
                }
            }
            else if (Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("Default")))
            {
                SelectedUnits.ForEach(unit => unit.SetTarget(hit.point));
            }
            else
            {
                UnselectAll();
                StartSelectingUnits();
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            UnselectAll();
        }
    }

    private void UpdateSelectionVisual()
    {
        currentSelectionPosition = Input.mousePosition;

        var xMin = Mathf.Min(startSelectionPosition.x, currentSelectionPosition.x);
        var yMin = Mathf.Min(startSelectionPosition.y, currentSelectionPosition.y);
        var xMax = Mathf.Max(startSelectionPosition.x, currentSelectionPosition.x);
        var yMax = Mathf.Max(startSelectionPosition.y, currentSelectionPosition.y);

        var topLeft = new Vector2(xMin, yMin);
        var bottomRight = new Vector2(xMax, yMax);
        
        selectionPanelTransform.offsetMin = topLeft;
        selectionPanelTransform.offsetMax = bottomRight;
    }

    private void StartSelectingUnits()
    {
        isHoldingSelection = true;
        startSelectionPosition = Input.mousePosition;
    }

    private void FinishSelectingUnits()
    {
        isHoldingSelection = false;
        selectionPanelTransform.offsetMin = Vector2.zero;
        selectionPanelTransform.offsetMax = Vector2.zero;
        var agentsAll = GameObject.FindObjectsByType<AgentMovement>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        foreach (var agent in agentsAll)
        {
            var screenAgentPosition = Camera.main.WorldToScreenPoint(agent.transform.position);

            if (screenAgentPosition.x > Mathf.Min(currentSelectionPosition.x, startSelectionPosition.x) && 
                screenAgentPosition.x < Mathf.Max(currentSelectionPosition.x, startSelectionPosition.x) &&
                screenAgentPosition.y > Mathf.Min(currentSelectionPosition.y, startSelectionPosition.y) &&
                screenAgentPosition.y < Mathf.Max(currentSelectionPosition.y, startSelectionPosition.y))
            {
                SelectUnit(agent);
            }
        }
    }

    private void UnselectAll()
    {
        SelectedUnits.ForEach(unit => unit.SetOutline(false));
        SelectedUnits.Clear();
    }

    private void SelectUnit(AgentMovement newUnit)
    {
        if (newUnit != null)
        {
            SelectedUnits.Add(newUnit);
            newUnit.SetOutline(true);
        }
    }
}
