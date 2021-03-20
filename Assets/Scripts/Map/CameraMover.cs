using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


//Note : Map is in 0.09, -0.23, i don't know why. But because all position calculs were made based on this coordinates, pls don't move it :)
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
         maxZoom = 2;
         minZoom = 5;
         moveSpeed = 0.05f;
    }

    public void moveCamera()
    {
        string panel = GetPanelUnderMouse();
        if (panel.Equals("PanelLeft") && !isOnBoarderLeft())
        {
            cam.transform.position += new Vector3(-moveSpeed, 0.0f, 0.0f);
        }
        if (panel.Equals("PanelRight") && !isOnBoarderRight())
        {
            cam.transform.position += new Vector3(+moveSpeed, 0.0f, 0.0f);
        }
        if (panel.Equals("PanelDown") && !isOnBoarderDown())
        {
            cam.transform.position += new Vector3(0.0f, -moveSpeed, 0.0f);
        }
        if (panel.Equals("PanelUp") && !isOnBoarderUp())
        {
            cam.transform.position += new Vector3(0.0f, +moveSpeed, 0.0f);
        }
        if (panel.Equals("PanelUpLeft"))
        {
            if (!isOnBoarderUp())
            {
                if (!isOnBoarderLeft())
                {
                    cam.transform.position += new Vector3(-moveSpeed, +moveSpeed, 0.0f);
                }
                else
                {
                    cam.transform.position += new Vector3(0.0f, +moveSpeed, 0.0f);
                }
            }
            else if (!isOnBoarderLeft())
            {
                cam.transform.position += new Vector3(-moveSpeed, 0.0f, 0.0f);
            }
        }

        if (panel.Equals("PanelUpRight"))
        {
            if (!isOnBoarderUp())
            {
                if (!isOnBoarderRight())
                {
                    cam.transform.position += new Vector3(+moveSpeed, +moveSpeed, 0.0f);
                }
                else
                {
                    cam.transform.position += new Vector3(0.0f, +moveSpeed, 0.0f);
                }
            }
            else if (!isOnBoarderRight())
            {
                cam.transform.position += new Vector3(+moveSpeed, 0.0f, 0.0f);
            }
        }

        if (panel.Equals("PanelDownLeft"))
        {
            if (!isOnBoarderDown())
            {
                if (!isOnBoarderLeft())
                {
                    cam.transform.position += new Vector3(-moveSpeed, -moveSpeed, 0.0f);
                }
                else
                {
                    cam.transform.position += new Vector3(0.0f, -moveSpeed, 0.0f);
                }
            }
            else if (!isOnBoarderLeft())
            {
                cam.transform.position += new Vector3(-moveSpeed, 0.0f, 0.0f);
            }
        }

        if (panel.Equals("PanelDownRight"))
        {
            if (!isOnBoarderDown())
            {
                if (!isOnBoarderRight())
                {
                    cam.transform.position += new Vector3(+moveSpeed, -moveSpeed, 0.0f);
                }
                else
                {
                    cam.transform.position += new Vector3(0.0f, -moveSpeed, 0.0f);
                }
            }
            else if (!isOnBoarderRight())
            {
                cam.transform.position += new Vector3(+moveSpeed, 0.0f, 0.0f);
            }
        }
    }

    public void ZoomCamera()
    {
        float x = cam.transform.position.x;
        float y = cam.transform.position.y;
        if(Input.mouseScrollDelta.y < 0 && cam.orthographicSize < minZoom)
        {
            cam.orthographicSize += 1;
        }
        if(Input.mouseScrollDelta.y > 0 && cam.orthographicSize > maxZoom)
        {
            cam.orthographicSize -= 1;
        }
        if (cam.orthographicSize == 5)
        {
            if (x> 1.4f)
                cam.transform.position = new Vector3(1.3f, y, -10.0f);                
            if (x <-1.3f)
                cam.transform.position = new Vector3(-1.2f, y, -10.0f);
            if (y > 2.5f)
                cam.transform.position = new Vector3(x, 2.4f, -10.0f);
            if (y < -2.9f)
                cam.transform.position = new Vector3(x, -2.8f, -10.0f);
        }
        if (cam.orthographicSize == 4)
        {
            if (x > 3.2f)
                cam.transform.position = new Vector3(3.1f, y, -10.0f);
            if (x < -3.1f)
                cam.transform.position = new Vector3(-3.0f, y, -10.0f);
            if (y > 3.5f)
                cam.transform.position = new Vector3(x, 3.4f, -10.0f);
            if (y < -3.9f)
                cam.transform.position = new Vector3(x, -3.8f, -10.0f);
        }
        if (cam.orthographicSize == 3)
        {
            if (x > 5f)
                cam.transform.position = new Vector3(4.9f, y, -10.0f);
            if (x < -4.9f)
                cam.transform.position = new Vector3(-4.8f, y, -10.0f);
            if (y > 4.5f)
                cam.transform.position = new Vector3(x, 4.4f, -10.0f);
            if (y < -5f)
                cam.transform.position = new Vector3(x, -4.8f, -10.0f);
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
            if (panelUnderMouse.tag == "PanelScroll")
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
        if (PauseMenu.isGamePaused)
            return;

        moveCamera();
        ZoomCamera();
        printData();
    }

    public bool isOnBoarderRight()
    {
        float size = cam.orthographicSize;
        float x = cam.transform.position.x;
        return ((size == 5 && x > 1.35) || (size == 4 && x > 3.1) || (size == 3 && x > 4.9) || (size == 2 && x > 6.7));
    }

    public bool isOnBoarderLeft()
    {
        float size = cam.orthographicSize;
        float x = cam.transform.position.x;
        return ((size == 5 && x < -1.2) || (size == 4 && x < -3) || (size == 3 && x < -4.8) || (size == 2 && x < -6.5));
    }

    public bool isOnBoarderUp()
    {
        float size = cam.orthographicSize;
        float y = cam.transform.position.y;
        return ((size == 5 && y > 2.4) || (size == 4 && y > 3.4) || (size == 3 && y > 4.4) || (size == 2 && y > 5.4));
    }
    public bool isOnBoarderDown()
    {
        float size = cam.orthographicSize;
        float y = cam.transform.position.y;
        return ((size == 5 && y < -2.8) || (size == 4 && y < -3.8) || (size == 3 && y < -4.9) || (size == 2 && y < -5.9));
    }

}
