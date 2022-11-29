using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    private readonly Dictionary<EPuzzleCategories, string> _puzzleCatDirectory = new Dictionary<EPuzzleCategories, string>();
    private int _settings;
    private const int SettingsNumber = 2;
    public enum EPairNumber
    {
        NotSet = 0,
        E10Pair = 10,
        E15Pair = 15,
        E20Pair = 20,

    }
    public enum EPuzzleCategories
    {
        NotSet,
        Fruits,
        Vegetables
    }
    public struct Settings
    {
        public EPairNumber PairsNumber;
        public EPuzzleCategories PuzzleCategory;
    }
    private Settings _gameSettings;
    public static GameSettings Instance;
    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(target: this);
            Instance = this;
        }
        else
        {
            Destroy(obj: this);
        }
    }
    void Start()
    {
        SetPuzzleCatDirectory();
        _gameSettings = new Settings();
        ResetGameSettings();

    }
    public void SetPuzzleCatDirectory()
    {
        _puzzleCatDirectory.Add(EPuzzleCategories.Fruits, "Fruits");
        _puzzleCatDirectory.Add(EPuzzleCategories.Vegetables, "Vegetables");
    }
    public void SetPairNumbers(EPairNumber Number)
    {
        if (_gameSettings.PairsNumber == EPairNumber.NotSet)
            _settings++;
        _gameSettings.PairsNumber = Number;
    }
    public void SetPuzzleCategories(EPuzzleCategories cat)
    {
        if (_gameSettings.PuzzleCategory == EPuzzleCategories.NotSet)
            _settings++;
        _gameSettings.PuzzleCategory = cat;
    }
    public EPairNumber GetPairNumber()
    {
        return _gameSettings.PairsNumber;
    }
    public EPuzzleCategories GetEPuzzleCategories()
    {
        return _gameSettings.PuzzleCategory;
    }
    public void ResetGameSettings()
    {
        _settings = 0;
        _gameSettings.PuzzleCategory = EPuzzleCategories.NotSet;
        _gameSettings.PairsNumber = EPairNumber.NotSet;
    }
    public bool AllSettingsReady()
    {
        return _settings == SettingsNumber;
    }
    public string GetMaterialDirectoryName()
    {
        return "Materials/";
    }
    public string GetPuzzleCategoryTextureDirectoryName()
    {
        if (_puzzleCatDirectory.ContainsKey(_gameSettings.PuzzleCategory))
        {
            return "Graphics/PuzzleCat/" + _puzzleCatDirectory[_gameSettings.PuzzleCategory] + "/";
        }
        else
        {
            Debug.LogError("ERROR : CANOT GET DIRECTORY NAME");
            return "";
        }
    }

}
