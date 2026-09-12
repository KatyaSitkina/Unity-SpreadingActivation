using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Animator door = null;
    bool isOpen = false;
    float transitionTime = 1.5f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void StartGame()
    {
        // сброс данных графа при повторном запуске новой игры
        if (GraphManager.Instance != null)
            GraphManager.Instance.ResetGraph();
        StartCoroutine(WaitForDoorOpen());
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }

    IEnumerator WaitForDoorOpen()
    {
        // ожидание проигрывания анимации перед запуском первой локации
        isOpen = true;
        door.SetBool("isOpen", isOpen);
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene("ControllersLearn");
    }
}
