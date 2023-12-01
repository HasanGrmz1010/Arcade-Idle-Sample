using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] Transform toFollow;
    [SerializeField] Vector3 offset;
    public float lerpSpeed;

    void Update()
    {
        transform.position = Vector3.Slerp(transform.position, toFollow.position - offset, lerpSpeed * Time.deltaTime);
    }
}
