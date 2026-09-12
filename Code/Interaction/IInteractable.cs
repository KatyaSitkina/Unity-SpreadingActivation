using UnityEngine;

public interface IInteractable
{
    public void Interact();
    public Sprite InteractionSprite();
    public bool IsInteracting();
}
