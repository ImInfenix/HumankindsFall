using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    private static Tooltip instance;

    private TextMeshProUGUI tooltipText;
    private RectTransform backgroundRectTransform;
    private RectTransform rectTransform;
    public Canvas canvas;

    private static bool isActive;

    private void Awake()
    {
        instance = this;

        backgroundRectTransform = transform.Find("Background").GetComponent<RectTransform>();
        tooltipText = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        rectTransform = transform.GetComponent<RectTransform>();
        gameObject.SetActive(false);
        isActive = false;
    }

    private void ShowTooltip(string def)
    {
        gameObject.SetActive(true);
        isActive = true;

        tooltipText.SetText(def);
        tooltipText.ForceMeshUpdate();
        Vector2 textSize = tooltipText.GetRenderedValues(false);
        Vector2 paddingSize = new Vector2(8, 8);
        backgroundRectTransform.sizeDelta = textSize+paddingSize;
        transform.Find("Text").GetComponent<RectTransform>().localPosition = new Vector3(-textSize.x, paddingSize.y /2, 0);
        backgroundRectTransform.localPosition = new Vector3(-textSize.x - 4, 0, 0);
    }

    private void HideTooltip()
    {
        gameObject.SetActive(false);
        isActive = false;
    }

    private void Update()
    {
        RectTransform cnvs = canvas.GetComponent<RectTransform>();
        Vector2 offSet = new Vector2(cnvs.sizeDelta.x / 2f, cnvs.sizeDelta.y / 2f);
        Vector2 viewPort = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        Vector2 position = new Vector2(cnvs.sizeDelta.x * viewPort.x - rectTransform.rect.width / 2 + 45 , cnvs.sizeDelta.y * viewPort.y + rectTransform.rect.height / 2 - 50);
        rectTransform.anchoredPosition = position;
    }

    public static void ShowTooltip_Static(string def)
    {
        instance.ShowTooltip(def);
    }

    public static void HideTooltip_Static()
    {
        instance.HideTooltip();
    }

    public static bool GetIsActive()
    {
        return isActive;
    }
}
