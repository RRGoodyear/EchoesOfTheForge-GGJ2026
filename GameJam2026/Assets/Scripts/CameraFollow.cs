using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //Source: https://gist.github.com/bendux/76a9b52710b63e284ce834310f8db773

    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);

    //Makes the camera delay behind the player, gives a smooth camera effect.
    [SerializeField] private float smoothTime = 0.25f;
    //The asset thats the camera follows.
    private Transform target;

    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    private Vector3 velocity = Vector3.zero;

    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
        target = GameObject.Find("CameraFollow").transform;
    }
    private void Update()
    {
        float camHeight = cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;

        float clampedX = Mathf.Clamp(
            target.position.x,
            minX + camWidth,
            maxX - camWidth
        );

        float clampedY = Mathf.Clamp(
            target.position.y,
            minY + camHeight,
            maxY - camHeight
        );
        //transform.position = new Vector3(
        //    clampedX,
        //    clampedY,
        //    transform.position.z
        //);

        Vector3 targetPosition = new Vector3(clampedX, clampedY, transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
    private void LateUpdate()
    {
        
    }
}
