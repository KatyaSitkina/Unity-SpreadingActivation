using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour, IInteractable
{
    [SerializeField] TextAsset inkJsonAsset; // json файл с текстом диалога
    [SerializeField] TextMeshProUGUI textField; // поле, где будет отображаться текст
    [SerializeField] VerticalLayoutGroup choiceButtonContainer; // контейнер для кнопок-ответов
    [SerializeField] Button choiceButtonPrefab; // префаб кнопки-ответа

    [SerializeField] Sprite interactionSprite;
    [SerializeField] UnityEvent OnInteractionStart;
    [SerializeField] UnityEvent OnInteractionEnd;

    Story story;
    int selectedChoice = 0;
    Button[] choiceButtons;
    [HideInInspector] public bool dialogueIsActive = false; // публичная для скрипта поворота головы персонажа

    float typingSpeed = 0.03f;  // скорость эффекта печати
    Coroutine displayLineCoroutine;

    void Start()
    {
        story = new Story(inkJsonAsset.text);
    }
    
    void LateUpdate()
    {
        if (!PauseMenu.isPaused && dialogueIsActive)
        {
            if (Input.GetKeyDown(KeyCode.Space) && (story.currentChoices.Count == 0 || displayLineCoroutine != null))
                DisplayNextLine();
            else if (story.currentChoices.Count > 0)
                ChoiceSelection();
        }
    }

    public void Interact()
    {
        dialogueIsActive = true;
        OnInteractionStart.Invoke();
        story.state.GoToStart();
        DisplayNextLine();
    }

    public Sprite InteractionSprite()
    {
        return interactionSprite;
    }

    public bool IsInteracting()
    {
        return dialogueIsActive;
    }

    // следующая реплика персонажа
    void DisplayNextLine()
    {
        // если пробел был нажат, когда реплика ещё полностью не показлась
        // останавливаем корутину и показываем реплику целиком
        if (displayLineCoroutine != null)
        {
            StopTyping();
            return;
        }

        ClearChoiceButtons();
        dialogueIsActive = story.canContinue || story.currentChoices.Count > 0;

        if (story.canContinue)
        {
            string text = story.Continue(); // получаем следующую строку диалога
            // повторный вызов метода, если в конце осталась пустая реплика
            if (string.IsNullOrEmpty(text))
            {
                DisplayNextLine();
                return;
            }
            displayLineCoroutine = StartCoroutine(TypingTextEffect(text));
        }
        else
            OnInteractionEnd.Invoke();
    }

    // эффект печати текста
    IEnumerator TypingTextEffect(string line)
    {
        textField.text = line;
        textField.maxVisibleCharacters = 0;
        for(int i = 0; i < line.Length; i++)
        {
            //вместо перерисовки текстового поля просто увеличиваем кол-во видимых символов
            textField.maxVisibleCharacters++;
            yield return new WaitForSeconds(typingSpeed);
        }
        StopTyping();
    }

    // остановка корутины
    void StopTyping()
    {
        StopCoroutine(displayLineCoroutine);
        textField.maxVisibleCharacters = textField.text.Length;
        displayLineCoroutine = null;
        if (story.currentChoices.Count > 0)
            DisplayChoices();
    }

    // создание кнопок-ответов
    void DisplayChoices()
    {
        choiceButtons = new Button[story.currentChoices.Count];

        for (int i = 0; i < story.currentChoices.Count; i++)
        {
            var choice = story.currentChoices[i];
            var button = CreateChoiceButton(choice.text);
            choiceButtons[i] = button;
        }
        HighlightChoice();
    }

    // создание кнопки с заданным текстом
    Button CreateChoiceButton(string text)
    {
        var choiceButton = Instantiate(choiceButtonPrefab);
        choiceButton.transform.SetParent(choiceButtonContainer.transform, false);
        var buttonText = choiceButton.GetComponentInChildren<TextMeshProUGUI>(); // кнопка это сама кнопка и её текст, поэтому отдельно получаем текст
        buttonText.text = text;
        return choiceButton;
    }

    // управление выбранным ответом в диалоге на W и S
    void ChoiceSelection()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            RemoveHighlightChoice();
            selectedChoice++;
            if (selectedChoice >= story.currentChoices.Count)
                selectedChoice = 0;
            HighlightChoice();
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            RemoveHighlightChoice();
            selectedChoice--;
            if (selectedChoice < 0)
                selectedChoice = story.currentChoices.Count - 1;
            HighlightChoice();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            story.ChooseChoiceIndex(selectedChoice);
            selectedChoice = 0;
            DisplayNextLine();
            ParseTags();
        }
    }

    // поиск тега с названием узла
    // добавление узла в список усзлов на активацию при выборе ответа
    void ParseTags()
    {
        List<string> currentStoryTags = story.currentTags;
        if (currentStoryTags != null)
            foreach (string tag in currentStoryTags)
                if (tag.StartsWith("node:"))
                    GraphManager.Instance.nodesToActivate.Add(tag.Substring("node:".Length).Trim());
    }

    // подсвечивание ответа
    void HighlightChoice()
    {
        var currentChoice = choiceButtons[selectedChoice].GetComponentInChildren<TextMeshProUGUI>();
        currentChoice.color = new Color32(255, 255, 255, 255);
    }
    
    // сброс подсветки
    void RemoveHighlightChoice()
    {
        var previousChoice = choiceButtons[selectedChoice].GetComponentInChildren<TextMeshProUGUI>();
        previousChoice.color = new Color32(255, 255, 255, 50);
    }

    // очистка кнопок-ответов из контейнера
    void ClearChoiceButtons()
    {
        foreach (var choice in choiceButtonContainer.GetComponentsInChildren<Button>())
            Destroy(choice.gameObject);
    }
}
