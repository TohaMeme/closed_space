using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] GameObject pausePanel;

    [Header("Scene")]
    [SerializeField] string mainMenuSceneName = "MainMenu";

    [Header("Pause targets")]
    [Tooltip("Скрипты, которые нужно отключать во время паузы (например PlayerMovement, AI, таймеры и т.п.)")]
    [SerializeField] MonoBehaviour[] behavioursToDisable;

    [Header("Cursor")]
    [SerializeField] bool lockCursorOnResume = true;

    bool isPaused;
    float previousTimeScale = 1f;
    bool[] previousEnabledStates;

    void Start()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    public void TogglePause()
    {
        SetPaused(!isPaused);
    }

    public void SetPaused(bool paused)
    {
        if (isPaused == paused) return;
        isPaused = paused;

        if (paused)
            EnterPause();
        else
            ExitPause();
    }

    void EnterPause()
    {
        previousTimeScale = Time.timeScale;
        Time.timeScale = 0f; 
        AudioListener.pause = true; 

        if (pausePanel != null)
            pausePanel.SetActive(true);

        if (behavioursToDisable != null)
        {
            previousEnabledStates = new bool[behavioursToDisable.Length];
            for (int i = 0; i < behavioursToDisable.Length; i++)
            {
                var b = behavioursToDisable[i];
                previousEnabledStates[i] = (b != null) && b.enabled;
                if (b != null) b.enabled = false;
            }
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void ExitPause()
    {
        Time.timeScale = previousTimeScale > 0f ? previousTimeScale : 1f;
        AudioListener.pause = false;

        if (pausePanel != null)
            pausePanel.SetActive(false);

        if (behavioursToDisable != null && previousEnabledStates != null)
        {
            for (int i = 0; i < behavioursToDisable.Length; i++)
            {
                var b = behavioursToDisable[i];
                if (b != null) b.enabled = previousEnabledStates[i];
            }
        }

        if (lockCursorOnResume)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    
    public void Resume()
    {
        SetPaused(false);
    }

    
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
