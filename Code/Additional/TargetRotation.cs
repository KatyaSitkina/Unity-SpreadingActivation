using UnityEngine;

public class TargetRotation : MonoBehaviour
{
    [SerializeField] Transform source;
    [SerializeField] Transform player;

    void Update()
    {
        source.LookAt(player.position);
    }
}
