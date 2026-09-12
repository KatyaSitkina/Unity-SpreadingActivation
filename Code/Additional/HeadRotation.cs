using UnityEngine;

public class HeadRotation : MonoBehaviour
{
    [SerializeField] Transform head;
    [SerializeField] Transform player;
    DialogueManager dialogueStatus;
    Quaternion targetToDefault;

    float rotationSpeed = 4.0f;
    float angleLimit = 70.0f; // угол обзора персонажа

    void Start()
    {
        dialogueStatus = GetComponent<DialogueManager>();
        targetToDefault = head.rotation;    // текущий поворот головы персонажа
    }

    void LateUpdate()
    {
        // поворот головы персонажа, если диалог активен
        if (dialogueStatus.dialogueIsActive)
        {
            Vector3 headToPlayer = player.position - head.position; // вектор от персонажа к игроку
            float playerAngle = Vector3.Angle(head.forward, headToPlayer);  // угол между векторами
            if (playerAngle <= angleLimit)
            {
                Quaternion targetRotation = Quaternion.LookRotation(headToPlayer);
                // плавный поворот головы персонажа в сторону игрока
                head.rotation = Quaternion.Slerp(head.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
        // возвращение головы персонажа к изначальному положению после завершения диалога
        else
            head.rotation = Quaternion.Slerp(head.rotation, targetToDefault, rotationSpeed * Time.deltaTime);
    }
}
