using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Marker : MonoBehaviour
{
    [SerializeField]
    private List<Marker> Neighbours;
    [SerializeField]
    private bool isAccessible;
    private bool isFinished;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Sprite GreenFlag;
    [SerializeField]
    private Sprite RedFlag;
    [SerializeField]
    private LevelDescription levelDescription;
    [SerializeField]
    private Sprite sprite;
    [SerializeField]
    private TextAsset descriptionTextFile;

    private List<string> eachLine;

    public void Awake()
    {
        if (!isAccessible)
            this.gameObject.SetActive(false);
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = RedFlag;
        eachLine = new List<string>();
        string AllFile = descriptionTextFile.text;
        eachLine.AddRange(AllFile.Split("\n"[0]));
    }
    public void FinishLevel()
    {
        spriteRenderer.sprite = GreenFlag;
        foreach(Marker marker in Neighbours)
        {
            marker.Unlock();
        }
    }

    public void Unlock()
    {
        if (!isAccessible)
            this.gameObject.SetActive(true);

    }

    public void Update()
    {
        if (Input.GetKeyDown("space"))
            FinishLevel();
    }

    public void OnMouseDown()
    {
        levelDescription.ChangeDescription(eachLine[1]);
        levelDescription.ChangeTitle(eachLine[0]);
        levelDescription.ChangeImage(sprite);
        levelDescription.Show();
        FinishLevel();
    }

    
}
