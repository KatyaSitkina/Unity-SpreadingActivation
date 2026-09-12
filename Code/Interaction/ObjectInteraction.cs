using TMPro;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.Events;

public class ObjectInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] TextAsset inkJsonAsset;
    [SerializeField] TextMeshProUGUI textField;
    [SerializeField] Sprite interactionSprite;
    Animator popUpAnimation;

    Story story;
    bool interactionIsActive = false;

    [SerializeField] UnityEvent OnInteractionStart;
    [SerializeField] UnityEvent OnInteractionEnd;

    void Start()
    {
        popUpAnimation = textField.GetComponent<Animator>();
    }

    void Update()
    {
        if (!PauseMenu.isPaused && interactionIsActive)
            if (Input.GetKeyDown(KeyCode.Space))
            {
                popUpAnimation.Play("TextPopUp", -1, 0f);
                DisplayNextLine();
            }
    }

    public void Interact()
    {
        interactionIsActive = true;
        OnInteractionStart.Invoke();

        story = new Story(inkJsonAsset.text);
        popUpAnimation.Play("TextPopUp", -1, 0f);
        DisplayNextLine();
    }

    public Sprite InteractionSprite()
    {
        return interactionSprite;
    }

    public bool IsInteracting()
    {
        return interactionIsActive;
    }

    void DisplayNextLine()
    {
        interactionIsActive = story.canContinue;

        if (story.canContinue)
        {
            string text = story.Continue();
            textField.text = text;
        }
        else
            OnInteractionEnd.Invoke();
    }
}
