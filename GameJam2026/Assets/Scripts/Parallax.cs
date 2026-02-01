using UnityEngine;

public class Parallax : MonoBehaviour
{
    //Source: https://www.youtube.com/watch?v=wBol2xzxCOU

    //Can set in inspector; higher the number the more delay on the background.
    [SerializeField] private Vector2 parallaxEffectMultiplier;

    private Transform cameraTransform;

    //Stores the cameras last position
    private Vector3 lastCameraPosition;

    private float textureUnitSizeX;

    private void Start()
    {
        cameraTransform = Camera.main.transform;

        //Sets camera position to where it is when the game starts.
        lastCameraPosition = cameraTransform.position;

        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
    }

    private void LateUpdate()
    {
        //How much camera moved since previous frame.
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;

        //Makes the background follow the camera; the higher the multiplier the more delay on the background giving the parallax effect.
        transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier.x, deltaMovement.y * parallaxEffectMultiplier.y);
        lastCameraPosition = cameraTransform.position;

        //Makes it so the background repositions when player is close to the end of it.
        if (Mathf.Abs(cameraTransform.position.x - transform.position.x) >= textureUnitSizeX)
        {
            float offsetPositionX = (cameraTransform.position.x - transform.position.x) % textureUnitSizeX;
            transform.position = new Vector3(cameraTransform.position.x + offsetPositionX, transform.position.y);
        }
    }
}
