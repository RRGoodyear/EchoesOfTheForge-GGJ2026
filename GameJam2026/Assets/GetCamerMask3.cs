using UnityEngine;

public class GetCamerMask3 : MonoBehaviour
{
    private PlayerMovement playerMove;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMove = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        playerMove.mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
