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

    private void Awake()
    {
        instance = this;

        backgroundRectTransform = transform.Find("Background").GetComponent<RectTransform>();
        tooltipText = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        rectTransform = transform.GetComponent<RectTransform>();
        gameObject.SetActive(false);
    }
    private void ShowTooltip(string def)
    {
        gameObject.SetActive(true);

        tooltipText.SetText(def);
        tooltipText.ForceMeshUpdate();
        Vector2 textSize = tooltipText.GetRenderedValues(false);
        Vector2 paddingSize = new Vector2(8, 8);
        backgroundRectTransform.sizeDelta = textSize+paddingSize;
        transform.Find("Text").GetComponent<RectTransform>().localPosition = new Vector3(-textSize.x, paddingSize.y/2, 0);
        backgroundRectTransform.localPosition = new Vector3(-textSize.x - paddingSize.x/2, 0, 0);
    }

    private void HideTooltip()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        rectTransform.anchoredPosition = Input.mousePosition;
    }

    public static void ShowTooltip_Static(string def)
    {
        instance.ShowTooltip(def);
    }

    public static void HideTooltip_Static()
    {
        instance.HideTooltip();
    }
}
