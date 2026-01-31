using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //Source: https://gist.github.com/bendux/76a9b52710b63e284ce834310f8db773

    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);

    //Makes the camera delay behind the player, gives a smooth camera effect.
    [SerializeField] private float smoothTime = 0.25f;

    private Vector3 velocity = Vector3.zero;

    //The asset thats the camera follows.
    [SerializeField] private Transform target;

    private void Update()
    {
        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
