using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InteractionController : MonoBehaviour
{
    [SerializeField] Image interactionImage;
    public static bool interactionIsActive = false;
    IInteractable lastInteractable = null;

    [SerializeField] UnityEvent ShowIcon;
    [SerializeField] UnityEvent HideIcon;

    void Update()
    {
        InteractRaycast();
    }

    // выпускается луч
    // проверяется не пересёк ли луч объект, с которым можно начать взаимодействие
    void InteractRaycast()
    {
        if (interactionIsActive)    // не выпускаем луч, если взаимодействие уже идёт
            return;

        Ray ray = new Ray(transform.position, transform.forward);
        float rayLength = 3.0f;
        if (Physics.Raycast(ray, out var hit, rayLength))
        {
            // если с объектом, в который попал луч, можно взаимодействовать
            // т.е у объекта есть скрипт, реализующий инфтерфейс IInteractable
            if (hit.collider.TryGetComponent<IInteractable>(out var hitObject))
            {
                if (hitObject != lastInteractable)
                {
                    lastInteractable = hitObject;
                    interactionImage.sprite = hitObject.InteractionSprite();
                    ShowIcon.Invoke();
                }
                if (Input.GetKeyDown(KeyCode.E))
                    StartCoroutine(HandleInteraction(hitObject));
            }
            else
                ClearInteraction();
        }
        else
            ClearInteraction();
    }

    void ClearInteraction()
    {
        if (lastInteractable != null)
        {
            lastInteractable = null;
            HideIcon.Invoke();
        }
    }

    IEnumerator HandleInteraction(IInteractable interactable)
    {
        interactionIsActive = true;
        HideIcon.Invoke();
        interactable.Interact();
        
        while (interactable.IsInteracting())
            yield return null;
        interactionIsActive = false;
        ShowIcon.Invoke();
    }
}
