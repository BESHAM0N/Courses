using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManagerStartMenu : MonoBehaviour
{
    [Header("General menu")] [SerializeField]
    private Button _payButton;

    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _exitButtonGM;
    [SerializeField] private GameObject _generalMenu;

    [Header("Settings menu")] [SerializeField]
    private Button _exitButtonSM;

    [SerializeField] private GameObject _settingsMenu;

    private MusicManager _musicManager;
    private JSONController _jsonController;

    private void Start()
    {
        _jsonController = GetComponent<JSONController>();
        _jsonController.LoadGameSettings();
        _settingsButton.onClick.AddListener(SwitchOnSettingsMenu);
        _exitButtonGM.onClick.AddListener(Exit);
        _exitButtonSM.onClick.AddListener(Exit);
        _exitButtonSM.onClick.AddListener(SaveGameSettings);
        _payButton.onClick.AddListener(StartPlayGame);
    }

    private void SwitchOnSettingsMenu()
    {
        _generalMenu.gameObject.SetActive(false);
        _settingsMenu.gameObject.SetActive(true);
    }

    private void Exit()
    {
        if (_generalMenu.gameObject.activeSelf)
        {
            Application.Quit();
            _generalMenu.gameObject.SetActive(false);
        }
        else if (_settingsMenu.gameObject.activeSelf)
        {
            _settingsMenu.gameObject.SetActive(false);
            _generalMenu.gameObject.SetActive(true);
        }
    }
    private void StartPlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void SaveGameSettings()
    {
        _jsonController.SaveGameSettings();
    }
    
}