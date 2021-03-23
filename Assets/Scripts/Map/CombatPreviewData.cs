using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Combat Preview Data", menuName = "Combat Preview Data")]
public class CombatPreviewData : ScriptableObject
{
    [Header("Scene")]
    public string sceneName;
    public TextAsset descriptionTextFile;

    [Header("Combat")]
    public int ennemiesCount;
}
