using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelDescription : MonoBehaviour
{

    [SerializeField]
    private TMP_Text Title;
    [SerializeField]
    private TMP_Text Description;
    [SerializeField]
    private Image image;

    public void Awake()
    {
        Hide();
    }

    public void OnClick()
    {
        Hide();
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void ChangeTitle(string text)
    {
        Title.text = text;
    }

    public void ChangeDescription(string description)
    {
        Description.text = description;
    }

    public void ChangeImage(Sprite newImage)
    {
        image.sprite = newImage;
    }
}
