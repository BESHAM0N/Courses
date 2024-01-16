using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class LocalDataProvider : MonoBehaviour
{
    private const string _fileName = "Units";
    private const string _saveFileExtension = ".json";
    private string _savePath => Application.dataPath;

    private void Awake()
    {
        LoadEnemyConfig();
    }
    
    private string GetFullPath(string fileName)
    {
        string fullPath = Path.Combine(_savePath, $"{fileName}{_saveFileExtension}");
        return fullPath;
    }
    
    private bool IsDataFileExist(string fullPath)
    {
        var result = File.Exists(fullPath);
        if (!result)
            Debug.Log($"Файл по пути {fullPath} не найден");
        return result;
    }
    
    public void SavePlayerInventory()
    {
        var serializeObject = JsonConvert.SerializeObject(UnitsStorage.Units, Formatting.Indented);
        File.WriteAllText(GetFullPath(_fileName), serializeObject);
    }
    
    public bool LoadEnemyConfig()
    {
        var path = GetFullPath(_fileName);
        if (!IsDataFileExist(path))
            return false;

        var jsonText = File.ReadAllText(path);
        if (string.IsNullOrEmpty(jsonText))
        {
            Debug.Log("Файл пустой");
            return false;
        }

        try
        {
            var test = JsonConvert.DeserializeObject<List<Unit>>(jsonText);
            UnitsStorage.Units = test;
            //EventManager.CallCheckTarget(UnitsStorage.Units);
        }
        catch (Exception ex)
        {
            Debug.Log($"Произошло исключение при чтении json файла: {ex.Message}");
        }
        return true;
    }
    
}
