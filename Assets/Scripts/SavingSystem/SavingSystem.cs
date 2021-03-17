using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SavingSystem
{
    private static SaveFile gameFileContent;

    private static string DocumentsPath { get { return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); } }
    private static string MyGamesPath { get { return $"{DocumentsPath}\\My Games"; } }
    private static string GamePath { get { return $"{MyGamesPath}\\Humankind's Fall"; } }
    private static string SaveFilePath { get { return $"{GamePath}\\game.json"; } }

    public static void SaveData()
    {
        CheckDirectory(GamePath);
        try
        {
            File.WriteAllLines($"{SaveFilePath}", GetGameInformations());
        }
        catch (Exception e)
        {
            Debug.Log($"Error while saving data to disk\n{e}");
        }
    }

    public static bool GameSaveExists()
    {
        return File.Exists(SaveFilePath);
    }

    public static SaveFile RetrieveData()
    {
        SaveFile saveFile = gameFileContent;
        gameFileContent = null;
        return saveFile;
    }

    public static void RetrieveDataFromDisk()
    {
        if (!GameSaveExists())
            return;

        try
        {
            gameFileContent = JsonUtility.FromJson<SaveFile>(File.ReadAllText(SaveFilePath));
        }
        catch (Exception e)
        {
            gameFileContent = null;
            Debug.Log($"Error while loading data from disk\n{e}");
        }
    }

    public static void DeleteData()
    {
        if (File.Exists(SaveFilePath))
            File.Delete(SaveFilePath);
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
        saveFile.finishedLevels = Marker.finishedLevels;
        saveFile.SetUnitsToSave(GetAllUnitsToSave());
        saveFile.walletAmount = (int)Player.instance?.Wallet?.GetAmount();
        saveFile.unitGeneratorId = UnitDescription.currentId;
        return saveFile;
    }

    static List<UnitDescription> GetAllUnitsToSave()
    {
        List<UnitDescription> units = new List<UnitDescription>();

        units.AddRange(Player.instance.Inventory.GetAllUnits());

        return units;
    }
}