using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float sensitivity = 3.0f;
    float limitAngleY = 80.0f;
    float rotationY = 0.0f;
    [SerializeField] Transform player;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // блокировка курсора в центре экрана
        Cursor.visible = false;
    }

    void Update()
    {
        if (!PauseMenu.isPaused && !InteractionController.interactionIsActive)
            CameraMovement();
    }

    void CameraMovement()
    {
        float rotationX = Input.GetAxis("Mouse X") * sensitivity;
        rotationY -= Input.GetAxis("Mouse Y") * sensitivity;

        rotationY = Mathf.Clamp(rotationY, -limitAngleY, limitAngleY);  // ограничение угла
        transform.localRotation = Quaternion.Euler(rotationY, 0, 0);    // вращение камеры

        player.Rotate(Vector3.up * rotationX);  // вращение персонажа
    }
}
