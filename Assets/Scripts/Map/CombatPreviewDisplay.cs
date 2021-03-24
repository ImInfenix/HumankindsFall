using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CombatPreviewDisplay : MonoBehaviour
{
    private Camera raycastCamera;

    private CombatPreviewData currentPreview;

    private void Awake()
    {
        raycastCamera = Camera.main;
    }

    private void Update()
    {
        Ray ray = raycastCamera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit))
        {
            if (currentPreview != null)
                ClearPreview();
            return;
        }

        Marker hoveredMarker = hit.collider.GetComponent<Marker>();

        if (currentPreview == null)
        {
            DisplayPreview(hoveredMarker.GetCombatPreviewData());
        }
    }

    public void DisplayPreview(CombatPreviewData combatPreviewData)
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        currentPreview = combatPreviewData;

        string toolTip = $"{combatPreviewData.sceneName}\n{combatPreviewData.ennemiesCount} ennemies";

        Tooltip.ShowTooltip_Static(toolTip);
    }

    public void ClearPreview()
    {
        currentPreview = null;
        Tooltip.HideTooltip_Static();
    }
}
