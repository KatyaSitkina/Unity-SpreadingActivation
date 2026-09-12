using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelLoader : MonoBehaviour
{
    [SerializeField] Animator transition;
    float transitionTime = 1.0f;
    bool sceneIsLoading = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !sceneIsLoading)
            StartCoroutine(LoadNextScene());
    }

    IEnumerator LoadNextScene()
    {
        sceneIsLoading = true;
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);    // ожидание пока проиграется анимация
        GraphManager.Instance.ActivateNodesList();  // активация всех узлов из списка
        GraphManager.Instance.NextRoomSearch(); // поиск следующей локации
        GraphManager.Instance.nodesToActivate.Add(GraphManager.Instance.currentRoom);

        // загрузка следующей сцены
        AsyncOperation operation = SceneManager.LoadSceneAsync(GraphManager.Instance.currentRoom);
        while (!operation.isDone)
            yield return null;
    }
}
