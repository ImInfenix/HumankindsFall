using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SavingSystem
{
    private static string DocumentsPath { get { return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); } }
    private static string MyGamesPath { get { return $"{DocumentsPath}\\My Games"; } }
    private static string GamePath { get { return $"{MyGamesPath}\\Humankind's Fall"; } }
    private static string SaveFilePath { get { return $"{GamePath}\\game.json"; } }

    public static void SaveData()
    {
        CheckDirectory(GamePath);
        File.WriteAllLines($"{SaveFilePath}", GetGameInformations());
    }

    private static IEnumerable<string> GetGameInformations()
    {
        List<string> dataToSave = new List<string>();

        dataToSave.Add(JsonUtility.ToJson(CreateSaveData(), true));

        return dataToSave;
    }

    static void CheckDirectory(string path)
    {
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
    }

    static SaveFile CreateSaveData()
    {
        SaveFile saveFile = new SaveFile();
        saveFile.SetUnitsToSave(GetAllUnitsToSave());
        saveFile.walletAmount = 150;
        return saveFile;
    }

    static List<UnitDescription> GetAllUnitsToSave()
    {
        List<UnitDescription> units = new List<UnitDescription>();

        units.Add(UnitGenerator.GenerateUnit(Unit.allyTag));
        units.Add(UnitGenerator.GenerateUnit(Unit.allyTag));
        units.Add(UnitGenerator.GenerateUnit(Unit.allyTag));
        units.Add(UnitGenerator.GenerateUnit(Unit.allyTag));
        units.Add(UnitGenerator.GenerateUnit(Unit.allyTag));

        return units;
    }
}

[Serializable]
public class MyClass
{
    public int level;
    public float timeElapsed;
    public string playerName;
}