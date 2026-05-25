using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;

public class GameOverController : MonoBehaviour
{
    [Header("UI Element References")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI statusText;

    [Header("Arcade Input Focus")]
    public Button rematchButton;

    private bool isPaused = false;
    private bool forceMatchLock = false;

    void Start()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        isPaused = false;
        forceMatchLock = false;
    }

    void Update()
    {
        if (forceMatchLock) return;

        bool pauseButtonPressedThisFrame = false;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.tabKey.wasPressedThisFrame || Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                pauseButtonPressedThisFrame = true;
            }
        }

        if (Gamepad.current != null)
        {
            if (Gamepad.current.startButton.wasPressedThisFrame)
            {
                pauseButtonPressedThisFrame = true;
            }
        }

        if (pauseButtonPressedThisFrame)
        {
            TogglePauseMenu();
        }
    }

    public void TogglePauseMenu()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            if (gameOverPanel != null) gameOverPanel.SetActive(true);

            if (statusText != null)
            {
                statusText.text = "Girlfriend Warrior";
                statusText.color = Color.white;
            }

            Time.timeScale = 0f;

            if (rematchButton != null)
            {
                rematchButton.Select();
            }
        }
        else
        {
            ResumeGame();
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void TriggerMatchWin()
    {
        forceMatchLock = true;
        DisplayEndScreen("VICTORY!", Color.green);
    }

    public void TriggerMatchLoss()
    {
        forceMatchLock = true;
        DisplayEndScreen("DEFEAT!", Color.red);
    }

    private void DisplayEndScreen(string message, Color textColor)
    {
        if (gameOverPanel == null || statusText == null) return;

        gameOverPanel.SetActive(true);
        statusText.text = message;
        statusText.color = textColor;

        Time.timeScale = 0f;

        if (rematchButton != null)
        {
            rematchButton.Select();
        }
    }

    public void ExecuteRematch()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExecuteQuit()
    {
        Debug.Log("Exiting match layout completely...");
        Application.Quit();
    }
}