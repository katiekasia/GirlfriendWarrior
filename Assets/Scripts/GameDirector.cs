using UnityEngine;
using TMPro;
using System.Collections;

public class GameDirector : MonoBehaviour
{
    [Header("Match Timing Configuration")]
    [Tooltip("Total prep phase time in seconds (5 minutes = 300 seconds)")]
    public float prepTimeDuration = 300f;

    // CHANGED: Fixed strictly to 1 wave of monsters for the entire game session
    private const int TOTAL_WAVES = 1;

    [Header("UI Text Overlays")]
    public TextMeshProUGUI timerTextUI;
    public TextMeshProUGUI statusTextUI;

    [Header("Combat Vectors")]
    [Tooltip("Drag the GameObject container that handles spawning enemies here")]
    public GameObject monsterSpawner;

    // Global authorization switches checked by player triggers
    public bool canUpgrade { get; private set; } = true;

    private float timeRemaining;
    private Coroutine activeStatusOverrideCoroutine;
    private bool areWavesRunning = false;

    void Start()
    {
        timeRemaining = prepTimeDuration;
        canUpgrade = true;
        areWavesRunning = false;

        if (monsterSpawner != null)
            monsterSpawner.SetActive(false);

        StartCoroutine(MasterMatchSequence());
    }

    IEnumerator MasterMatchSequence()
    {
        // 1. COUNTDOWN PREPARATION PHASE
        if (statusTextUI != null) statusTextUI.text = "Gather Flowers & Secure Wall!";
        StartCoroutine(ClearInitialStatusAfterDelay(5f));

        while (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateClockHUD(timeRemaining);
            yield return null;
        }

        // 2. LOCK DOWN ALL TRADING BOOTHS
        canUpgrade = false;
        areWavesRunning = true;

        if (activeStatusOverrideCoroutine != null) StopCoroutine(activeStatusOverrideCoroutine);
        if (timerTextUI != null) timerTextUI.text = "00:00";
        Debug.Log("Time ran out! Shop booths and defense adjustments are locked down.");

        // 3. SINGLE ENEMY ATTACK WAVE SEQUENCE
        // FIXED: Loops exactly 1 single time, dropping the breathing space delays between waves entirely
        yield return StartCoroutine(ExecuteCombatWave(1));

        // 4. MATCH VICTORY
        TriggerVictory();
    }

    IEnumerator ClearInitialStatusAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (!areWavesRunning && statusTextUI != null && statusTextUI.text == "Gather Flowers & Secure Wall!")
        {
            statusTextUI.text = "";
        }
    }

    public void DisplayTemporaryStatus(string message)
    {
        if (areWavesRunning) return;

        if (activeStatusOverrideCoroutine != null)
        {
            StopCoroutine(activeStatusOverrideCoroutine);
        }

        activeStatusOverrideCoroutine = StartCoroutine(RunTemporaryStatus(message));
    }

    IEnumerator RunTemporaryStatus(string targetMessage)
    {
        if (statusTextUI != null) statusTextUI.text = targetMessage;

        yield return new WaitForSeconds(5f);

        if (statusTextUI != null && statusTextUI.text == targetMessage)
        {
            statusTextUI.text = "";
        }
    }

    IEnumerator ExecuteCombatWave(int waveNum)
    {
        if (statusTextUI != null) statusTextUI.text = "MONSTERS INCOMING!";
        if (monsterSpawner != null) monsterSpawner.SetActive(true);

        // Continuous wave combat simulation (e.g., 60 seconds)
        float waveTimer = 60f;
        while (waveTimer > 0)
        {
            waveTimer -= Time.deltaTime;
            yield return null;
        }

        if (monsterSpawner != null) monsterSpawner.SetActive(false);
    }

    void UpdateClockHUD(float currentSecondsValue)
    {
        if (currentSecondsValue < 0) currentSecondsValue = 0;

        int minutes = Mathf.FloorToInt(currentSecondsValue / 60);
        int seconds = Mathf.FloorToInt(currentSecondsValue % 60);

        if (timerTextUI != null)
        {
            timerTextUI.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public void TriggerGameOver()
    {
        StopAllCoroutines();
        if (statusTextUI != null) statusTextUI.text = "THE FORTRESS HAS FALLEN! GAME OVER";
        if (monsterSpawner != null) monsterSpawner.SetActive(false);
        Time.timeScale = 0f;
    }

    void TriggerVictory()
    {
        if (statusTextUI != null) statusTextUI.text = "FORTRESS SURVIVED! VICTORY!";
        Time.timeScale = 0f;
    }
}