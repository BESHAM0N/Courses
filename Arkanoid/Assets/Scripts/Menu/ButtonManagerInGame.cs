using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class ButtonManagerInGame : MonoBehaviour
{
    [SerializeField] private Button _pauseButton;
    
    [Header("Pause menu")]
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private GameObject _pauseMenu;
    
    private void Start()
    {
        _pauseButton.onClick.AddListener(OpenPauseMenu);
        _restartButton.onClick.AddListener(RestartGame);
        _exitButton.onClick.AddListener(ExitGame);
        _closeButton.onClick.AddListener(ClosePauseMenu);
    }

    private void OpenPauseMenu()
    {
        _pauseMenu.SetActive(true);
        _pauseButton.gameObject.SetActive(false);
        Time.timeScale = 0;
    }

    private void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }

    private void ClosePauseMenu()
    {
        _pauseMenu.SetActive(false);
        _pauseButton.gameObject.SetActive(true);
        Time.timeScale = 1;
    }
    
    private void ExitGame()
    {
        
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        SceneManager.LoadScene("MainMenu");
    }
    
}
