using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] Animator door = null;
    [SerializeField] AudioSource doorOpenAudio;
    [SerializeField] AudioSource doorCloseAudio;
    bool isOpen = false;
    float delay = 0.3f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isOpen)
        {
            isOpen = true;
            door.SetBool("isOpen", isOpen);
            doorOpenAudio.Play();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isOpen)
        {
            isOpen = false;
            door.SetBool("isOpen", isOpen);
            doorCloseAudio.PlayDelayed(delay);
        }
    }
}
