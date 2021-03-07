using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMover : MonoBehaviour
{
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private GameObject map;
    [SerializeField]
    private int maxZoom;
    [SerializeField]
    private int minZoom ;
    [SerializeField]
    private float moveSpeed;

    public void Awake()
    {
         maxZoom = 3;
         minZoom = 5;
         moveSpeed = 0.05f;
    }

    public void moveCamera()
    {
        string panel = GetPanelUnderMouse();
        if (panel.Equals("PanelLeft"))
        {
            cam.transform.position += new Vector3(-moveSpeed, 0.0f, 0.0f);
        }
        if (panel.Equals("PanelRight"))
        {
            cam.transform.position += new Vector3(+moveSpeed, 0.0f, 0.0f);
        }
        if (panel.Equals("PanelDown"))
        {
            cam.transform.position += new Vector3(0.0f, -moveSpeed, 0.0f);
        }
        if (panel.Equals("PanelUp"))
        {
            cam.transform.position += new Vector3(0.0f, +moveSpeed, 0.0f);
        }
        if (panel.Equals("PanelUpLeft"))
        {
            cam.transform.position += new Vector3(-moveSpeed, +moveSpeed, 0.0f);
        }
        if (panel.Equals("PanelUpRight"))
        {
            cam.transform.position += new Vector3(+moveSpeed, +moveSpeed, 0.0f);
        }
        if (panel.Equals("PanelDownLeft"))
        {
            cam.transform.position += new Vector3(-moveSpeed, -moveSpeed, 0.0f);
        }
        if (panel.Equals("PanelDownRight"))
        {
            cam.transform.position += new Vector3(+moveSpeed, -moveSpeed, 0.0f);
        }
    }

    public void ZoomCamera()
    {
        if(Input.mouseScrollDelta.y < 0 && cam.orthographicSize < minZoom)
        {
            cam.orthographicSize += 1;
        }
        if(Input.mouseScrollDelta.y > 0 && cam.orthographicSize > maxZoom)
        {
            cam.orthographicSize -= 1;
        }
    }

    public string GetPanelUnderMouse()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            pointerId = -1,
        };

        pointerData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        GameObject panelUnderMouse = null;

        foreach (RaycastResult r in results)
        {
            panelUnderMouse = r.gameObject;
            if (panelUnderMouse != null)
                break;
        }
        if (results.Count == 0)
        {
            return "";
        }
        return panelUnderMouse.name;
    }

    public void printData()
    {
        if (Input.GetKeyDown("space"))
        {
            Debug.Log(cam.orthographicSize);
            Debug.Log(cam.transform.position);
            Debug.Log(map.GetComponent<Renderer>().bounds);
        }
            
    }
    public void Update()
    {
        moveCamera();
        ZoomCamera();
        printData();
    }

}
