using UnityEngine;
using TMPro;
using System.Collections;

public class GameDirector : MonoBehaviour
{
    [Header("Match Timing Configuration")]
    [Tooltip("Total prep phase time in seconds (5 minutes = 300 seconds)")]
    public float prepTimeDuration = 300f;

    private const int TOTAL_WAVES = 1;

    [Header("UI Text Overlays")]
    public TextMeshProUGUI timerTextUI;
    public TextMeshProUGUI statusTextUI;

    [Header("Combat Vectors")]
    [Tooltip("Drag the GameObject container that handles spawning enemies here")]
    public GameObject monsterSpawner;

    [Header("Audio Configuration")]
    [Tooltip("Drag an AudioSource here configured with your peaceful prep music track")]
    public AudioSource prepMusicSource;
    [Tooltip("Drag an AudioSource here configured with your loopable combat music track")]
    public AudioSource waveMusicSource;

    public bool canUpgrade { get; private set; } = true;

    private float timeRemaining;
    private Coroutine activeStatusOverrideCoroutine;
    private bool areWavesRunning = false;
    private bool isMatchOver = false;

    void Start()
    {
        timeRemaining = prepTimeDuration;
        canUpgrade = true;
        areWavesRunning = false;
        isMatchOver = false;

        if (monsterSpawner != null)
            monsterSpawner.SetActive(false);

        // FIXED: Instantly start playing the peaceful prep phase music on match startup!
        if (prepMusicSource != null)
        {
            prepMusicSource.Play();
        }

        StartCoroutine(MasterMatchSequence());
    }

    IEnumerator MasterMatchSequence()
    {
        // 1. COUNTDOWN PREPARATION PHASE
        if (statusTextUI != null) statusTextUI.text = "Gather Flowers & Secure Wall!";
        StartCoroutine(ClearInitialStatusAfterDelay(5f));

        while (timeRemaining > 0)
        {
            if (isMatchOver) yield break;

            timeRemaining -= Time.deltaTime;
            UpdateClockHUD(timeRemaining);
            yield return null;
        }

        if (isMatchOver) yield break;

        // FIXED: The clock hit zero! Cut off the peaceful prep music cleanly right now.
        if (prepMusicSource != null)
        {
            prepMusicSource.Stop();
        }

        // 2. INITIATE SIEGE PHASE
        areWavesRunning = true;

        if (activeStatusOverrideCoroutine != null) StopCoroutine(activeStatusOverrideCoroutine);
        if (timerTextUI != null) timerTextUI.text = "00:00";
        Debug.Log("Clock hit zero! Octopuses are attacking, but trading remains OPEN!");

        // 3. SINGLE ENEMY ATTACK WAVE SEQUENCE
        yield return StartCoroutine(ExecuteCombatWave(1));

        // 4. MATCH VICTORY
        if (!isMatchOver)
        {
            TriggerVictory();
        }
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
        if (isMatchOver) return;

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
        if (isMatchOver) yield break;

        if (statusTextUI != null) statusTextUI.text = "MONSTERS INCOMING!";
        if (monsterSpawner != null) monsterSpawner.SetActive(true);

        // Start playing the monster wave background music clip
        if (waveMusicSource != null)
        {
            waveMusicSource.Play();
        }

        float spawnDurationLimit = 15f;
        while (spawnDurationLimit > 0)
        {
            if (isMatchOver) yield break;
            spawnDurationLimit -= Time.deltaTime;
            yield return null;
        }

        if (monsterSpawner != null) monsterSpawner.SetActive(false);

        if (statusTextUI != null) statusTextUI.text = "ELIMINATE THE REMAINING OCTOPUSES!";

        bool elementsRemaining = true;
        while (elementsRemaining)
        {
            if (isMatchOver) yield break;

            GameObject remainingMonster = GameObject.FindWithTag("Monster");

            if (remainingMonster == null)
            {
                elementsRemaining = false;
            }
            else
            {
                yield return null;
            }
        }
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
        if (isMatchOver) return;
        isMatchOver = true;

        StopAllCoroutines();

        if (statusTextUI != null) statusTextUI.text = "THE FORTRESS HAS FALLEN! GAME OVER";
        if (monsterSpawner != null) monsterSpawner.SetActive(false);

        // Safety Catch: Stop BOTH tracks if a game over hits unexpectedly
        if (prepMusicSource != null) prepMusicSource.Stop();
        if (waveMusicSource != null) waveMusicSource.Stop();

        GameOverController gameOver = Object.FindFirstObjectByType<GameOverController>();
        if (gameOver != null)
        {
            gameOver.TriggerMatchLoss();
        }
        else
        {
            Debug.LogError("Director Error: GameOverController component missing from active scene hierarchy!");
        }
    }

    void TriggerVictory()
    {
        if (isMatchOver) return;
        isMatchOver = true;

        StopAllCoroutines();

        if (statusTextUI != null) statusTextUI.text = "FORTRESS SURVIVED! VICTORY!";

        // Safety Catch: Stop BOTH tracks on victory state updates
        if (prepMusicSource != null) prepMusicSource.Stop();
        if (waveMusicSource != null) waveMusicSource.Stop();

        GameOverController gameOver = Object.FindFirstObjectByType<GameOverController>();
        if (gameOver != null)
        {
            gameOver.TriggerMatchWin();
        }
        else
        {
            Debug.LogError("Director Error: GameOverController component missing from active scene hierarchy!");
        }
    }
}