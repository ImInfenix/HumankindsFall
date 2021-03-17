using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelDescription : MonoBehaviour
{
    private const string BattleScenesFolder = "Scenes/BattleScenes";

    [SerializeField]
    private string placeName;
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

    public void ChangeName(string text)
    {
        placeName = text;
    }

    public void ChangeDescription(string description)
    {
        Description.text = description;
    }

    public void ChangeImage(Sprite newImage)
    {
        image.sprite = newImage;
    }

    //Il faudra que la scène ait le même nom que le niveau
    public void LoadBattle()
    {
        GameManager.instance.EnterBattle(placeName);
    }

}
