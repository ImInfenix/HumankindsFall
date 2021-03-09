using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Marker : MonoBehaviour
{
    [SerializeField]
    private List<Marker> Neighbours;
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
    private string descriptionText;
    [SerializeField]
    private string titleText;
    [SerializeField]
    private Sprite sprite;


    public void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = RedFlag;
    }
    public void FinishLevel()
    {
        spriteRenderer.sprite = GreenFlag;
    }

    public void Update()
    {
        if (Input.GetKeyDown("space"))
            FinishLevel();
    }

    public void OnMouseDown()
    {
        levelDescription.Show();
        levelDescription.ChangeDescription(descriptionText);
        levelDescription.ChangeTitle(titleText);
        levelDescription.ChangeImage(sprite);
    }


}
