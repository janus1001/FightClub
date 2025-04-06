using UnityEngine;

public class UnitManager : MonoBehaviour
{
    private AgentMovement selectedUnit;
    private AgentMovement SelectedUnit
    {
        get
        {
            return selectedUnit;
        }
        set
        {
            selectedUnit?.SetOutline(false);
            selectedUnit = value;
        }
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("Unit")))
            {
                AgentMovement unit = hit.collider.GetComponent<AgentMovement>();
                if (unit != null)
                {
                    SelectUnit(unit);
                }
            }
            else if (Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("Default")))
            {
                SelectedUnit.SetTarget(hit.point);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            SelectedUnit = null;
        }
    }

    private void SelectUnit(AgentMovement newUnit)
    {
        SelectedUnit = newUnit;
        
        if (SelectedUnit != null)
        {
            SelectedUnit.SetOutline(true);
        }
    }
}
