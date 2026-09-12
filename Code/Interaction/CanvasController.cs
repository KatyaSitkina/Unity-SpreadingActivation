using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    Canvas canvas;
    GraphicRaycaster raycaster;

    void Awake()
    {
        canvas = GetComponent<Canvas>();
        raycaster = GetComponent<GraphicRaycaster>();
    }

    public void ActivateCanvas()
    {
        canvas.enabled = true;
        if (raycaster != null)
            raycaster.enabled = true;
    }

    public void DeactivateCanvas()
    {
        canvas.enabled = false;
        if (raycaster != null)
            raycaster.enabled = false;
    }
}
