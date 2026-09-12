using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitlesLevel : MonoBehaviour
{
    [SerializeField] List<string> lines;
    [SerializeField] TextMeshProUGUI textField;
    Animator popUpAnimation;

    int currentLine = 0;
    [SerializeField] string nextScene;

    void Start()
    {
        Cursor.visible = false;
        popUpAnimation = textField.GetComponent<Animator>();
        DisplayNextLine();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentLine < lines.Count)
                DisplayNextLine();
            else
            {
                textField.text = "";
                SceneManager.LoadScene(nextScene);
            }
        }
    }

    // показ следующей строки из списка и проигрывание анимации
    void DisplayNextLine()
    {
        textField.text = lines[currentLine];
        popUpAnimation.Play("TextPopUp", -1, 0f);
        currentLine++;
    }
}
