using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // REQUIRED FOR NEW INPUT SYSTEM CAPABILITIES
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
        // Don't allow pausing if the match is officially over!
        if (forceMatchLock) return;

        // FIXED CRITIQUE: Rewritten using modern New Input System formatting
        bool pauseButtonPressedThisFrame = false;

        // 1. Check Keyboard Input (Tab key or Escape key)
        if (Keyboard.current != null)
        {
            if (Keyboard.current.tabKey.wasPressedThisFrame || Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                pauseButtonPressedThisFrame = true;
            }
        }

        // 2. Check Arcade Machine/Gamepad Input (Menu / Start Button)
        if (Gamepad.current != null)
        {
            // Start button on controllers automatically maps to the machine's Start button layout
            if (Gamepad.current.startButton.wasPressedThisFrame)
            {
                pauseButtonPressedThisFrame = true;
            }
        }

        // Trigger the pause menu toggle cleanly if any matching input was detected
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

            // Set your custom pause title!
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