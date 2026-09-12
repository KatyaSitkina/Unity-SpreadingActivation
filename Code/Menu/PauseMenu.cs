using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
    [SerializeField] UnityEvent OnInteractionStart;
    [SerializeField] UnityEvent OnInteractionEnd;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            if (isPaused)
                Resume();
            else
                Pause();
    }

    // показ меню паузы и замораживание времени
    void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        AudioListener.pause = true;

        isPaused = true;
        OnInteractionStart.Invoke();
        Time.timeScale = 0f;
    }

    // продолжение игры и возобновление времени
    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        AudioListener.pause = false;

        isPaused = false;
        OnInteractionEnd.Invoke();
        Time.timeScale = 1.0f;
    }

    // выход из игры или PLayMode в Unity
    public void ExitGame()
    {
        Time.timeScale = 1.0f;
        AudioListener.pause = false;
        isPaused = false;
        SceneManager.LoadScene("MainMenu");
    }
}
