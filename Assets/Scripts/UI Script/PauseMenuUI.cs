using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;

    private void Start()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        bool isPaused = Time.timeScale == 0f;

        Time.timeScale = isPaused ? 1f : 0f;
        pausePanel.SetActive(!isPaused);
    }

    public void RestartScene()
    {
        Time.timeScale = 1f;
        GameManager.instance.RestartScene();
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        GameManager.instance.ChangeScene("MainMenu");
    }
}