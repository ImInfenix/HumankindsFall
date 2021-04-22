using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Marker : MonoBehaviour
{
    public static List<string> finishedLevels;
    public static string currentBattle;

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
    private Material classic;
    [SerializeField]
    private Material outline;
    [SerializeField]
    private LevelDescription levelDescription;
    [SerializeField]
    private Sprite sprite;
    [SerializeField]
    private CombatPreviewData combatPreview;

    private List<string> eachLine;

    public void Awake()
    {
        if (!isAccessible)
            gameObject.SetActive(false);
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = RedFlag;
        spriteRenderer.material = outline;
        ReadDescription();
    }

    private void ReadDescription()
    {
        eachLine = new List<string>();
        string AllFile = combatPreview.descriptionTextFile.text;
        eachLine.AddRange(AllFile.Split("\n"[0]));
    }

    private void Start()
    {
        if (finishedLevels != null && finishedLevels.Contains(combatPreview.descriptionTextFile.name))
            FinishLevel();
    }

    public void FinishLevel()
    {
        spriteRenderer.sprite = GreenFlag;
        spriteRenderer.material = classic;
        foreach (Marker marker in Neighbours)
        {
            marker.Unlock();
        }
    }

    public void Unlock()
    {
        if (!isAccessible)
            gameObject.SetActive(true);
    }

    public void OnMouseDown()
    {
        if (PauseMenu.isGamePaused)
            return;

        Tooltip.HideTooltip_Static();

        levelDescription.ChangeDescription(eachLine[1]);
        levelDescription.ChangeTitle(eachLine[0]);
        levelDescription.ChangeName(combatPreview.sceneName);
        levelDescription.ChangeImage(sprite);
        levelDescription.Show();
    }

    public static void Add(string level)
    {
        if (finishedLevels == null)
            finishedLevels = new List<string>();

        if (!finishedLevels.Contains(level))
            finishedLevels.Add(level);
    }

    public CombatPreviewData GetCombatPreviewData()
    {
        return combatPreview;
    }
}
