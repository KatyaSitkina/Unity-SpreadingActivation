using UnityEngine;

public class DoorKnock : MonoBehaviour
{
    AudioSource knockAudio;

    void Start()
    {
        knockAudio = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        knockAudio.Play();
        Destroy(GetComponent<BoxCollider>());
    }
}
