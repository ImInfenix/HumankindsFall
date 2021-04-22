using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelDescription : MonoBehaviour
{
    [SerializeField]
    private string sceneToLoadName;
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

    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void ChangeTitle(string text)
    {
        Title.text = text;
    }

    public void ChangeName(string text)
    {
        sceneToLoadName = text;
    }

    public void ChangeDescription(string description)
    {
        Description.text = description;
    }

    public void ChangeImage(Sprite newImage)
    {
        image.sprite = newImage;
    }

    public void LoadBattle()
    {
        Marker.currentBattle = sceneToLoadName;
        GameManager.instance.EnterBattle(sceneToLoadName);
    }

}
