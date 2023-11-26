using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Unity.VisualScripting;
using UnityEngine.Serialization;

public class JSONController : MonoBehaviour
{
    [SerializeField] private Toggle _turnSoundOffToggle;

    [SerializeField] private ToggleGroup _levelToggleGroup;

    private const string _fileName = "SaveSettings";
    private const string _saveFileExtension = ".json";
    private string _savePath => Application.dataPath;
    private List<Toggle> _levelSettings;

    private void Awake()
    {
        _levelSettings = _levelToggleGroup.GetComponentsInChildren<Toggle>().ToList();
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

    public void SaveGameSettings()
    {
        var gameSettings = GetGameSettingsState();
        var serializeObject = JsonConvert.SerializeObject(gameSettings, Formatting.Indented);
        File.WriteAllText(GetFullPath(_fileName), serializeObject);
    }

    private GameSettings GetGameSettingsState()
    {
        var gameSettings = new GameSettings();
        gameSettings.IsOn = _turnSoundOffToggle.isOn;

        gameSettings.Difficulty = _levelSettings.FirstOrDefault(t => t.isOn).name switch
        {
            "Simple" => Difficulty.Simple,
            "Medium" => Difficulty.Medium,
            "Hard" => Difficulty.Hard,
            _ => Difficulty.Simple
        };

        return gameSettings;
    }

    public bool LoadGameSettings()
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
            var gameSettings = JsonConvert.DeserializeObject<GameSettings>(jsonText);
            SetGameSettings(gameSettings);
        }
        catch (Exception ex)
        {
            Debug.Log($"Произошло исключение при чтении json файла: {ex.Message}");
        }

        return true;
    }

    public void SetGameSettings(GameSettings gameSettings)
    {
        _turnSoundOffToggle.isOn = gameSettings.IsOn;
        _levelToggleGroup.SetAllTogglesOff();
        _levelSettings.FirstOrDefault(t => t.name == gameSettings.Difficulty.ToString()).isOn = true;
        
        // foreach (var t in _levelToggleGroup.GetComponentsInChildren<Toggle>())
        // {
        //     if (t.name == gameSettings.Difficulty.ToString())
        //     {
        //         t.isOn = true;
        //     }
        // }
    }
}