using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("UI Panel Reference Links")]
    public GameObject mainMenuPanel;
    public GameObject tutorialPanel;

    [Header("Arcade Navigation Focus")]
    [Tooltip("Drag your main Play Button here")]
    public Button playButton;

    [Tooltip("Drag the Back/Close Button inside your tutorial panel here")]
    public Button closeTutorialButton;

    void Start()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (tutorialPanel != null) tutorialPanel.SetActive(false);

        Time.timeScale = 0f;
        ResetMainMenuFocus();
    }

    public void PressPlayGame()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        Time.timeScale = 1f;
        Debug.Log("Game unpaused, starting combat!");
    }

    public void OpenTutorialPage()
    {
        if (tutorialPanel != null) tutorialPanel.SetActive(true);

        if (closeTutorialButton != null)
        {
            closeTutorialButton.Select();
        }
    }

    public void CloseTutorialPage()
    {
        if (tutorialPanel != null) tutorialPanel.SetActive(false);

        ResetMainMenuFocus();
    }

    private void ResetMainMenuFocus()
    {
        if (playButton != null)
        {
            playButton.Select();
        }
    }
}