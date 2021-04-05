using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlacementTilemapReader : MonoBehaviour
{
    [SerializeField]
    private CombatPreviewData combatPreviewData;

    [SerializeField]
    private TileBase ennemyTile;

    public void GenerateData()
    {
        combatPreviewData.sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        int ennemiesCount = 0;

        Tilemap tilemap = GetComponent<Tilemap>();

        var it = tilemap.cellBounds.allPositionsWithin;

        while (it.MoveNext())
        {
            if (tilemap.GetTile(it.Current) == ennemyTile)
                ennemiesCount++;
        }

        combatPreviewData.ennemiesCount = ennemiesCount;
    }

    public CombatPreviewData GetData()
    {
        return combatPreviewData;
    }
}
