using UnityEngine;

public class PatCat : MonoBehaviour, IInteractable
{
    [SerializeField] Sprite interactionSprite;
    Animator patCat;
    AudioSource meowAudio;

    void Start()
    {
        patCat = GetComponent<Animator>();
        meowAudio = GetComponent<AudioSource>();
    }

    public void Interact()
    {
        patCat.Play("PatCat", -1, 0f);
        meowAudio.Play();
    }

    public Sprite InteractionSprite()
    {
        return interactionSprite;
    }

    public bool IsInteracting()
    {
        return false;
    }

}
