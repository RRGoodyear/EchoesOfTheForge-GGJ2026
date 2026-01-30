using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    //Movement variables.
    float xmove = 0.0f;
    float ymove = 0.0f;

    //This variable is set to read whether the player is facing right or left and if so, flip the character sprite accordingly.
    float xmoveHorizontal = 0.0f;

    //Movement speed value; higher it is the faster the sprite moves.
    public float movementSpeed = 10f;
    public float playerJumpAcceleration = 2;
    private int jumpAmount = 0;

    private bool canMove;
    private bool canJump;

    public Camera mainCam;

    private bool aquiredMask1 = false;
    private bool aquiredMask2 = false;
    private bool aquiredMask3 = false;
    private bool selectedMask1 = false;

    //UI
    public Image MaskIndicator;
    public Image Mask1; 
    public Image Mask2;  
    public Image Mask3;
    
    private Vector2 mask1IndicatorPos; Vector2 mask2IndicatorPos; Vector2 mask3IndicatorPos;
    private Vector2 maskIndicatorOffset;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canMove = true;
        canJump = true;
        MaskIndicator.enabled = false; Mask1.enabled = false; Mask2.enabled = false; Mask3.enabled = false;
        mask1IndicatorPos = Mask1.rectTransform.anchoredPosition; mask2IndicatorPos = Mask2.rectTransform.anchoredPosition; mask3IndicatorPos = Mask3.rectTransform.anchoredPosition;
        maskIndicatorOffset = new Vector2(5, -50);
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            Movement();
        }
        if (canJump && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
            jumpAmount += 1;
            print(jumpAmount);
        }
    }

    //This method defines everything there is when it comes to player movement. Method is called in Update().
    private void Movement()
    {
        //Changes animation based on "Speed"
        //animator.SetFloat("Speed", xmoveHorizontal);

        Vector3 Player1Position = new Vector2(xmove, ymove);

        //Makes the variable number equal the Horizontal Axis.
        xmoveHorizontal = Input.GetAxis("Horizontal");

        float speedMove = Input.GetAxis("Horizontal");
        Player1Position += new Vector3(speedMove * 4, 0) * Time.deltaTime;

        //Setting movement keybinds to the axis the player is moving e.g. D moves the player right.
        if (Input.GetKey(KeyCode.D))
        {
            Player1Position = new Vector2((movementSpeed), ymove) * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            Player1Position = new Vector2(-(movementSpeed), ymove) * Time.deltaTime;
        }

        
        if (Input.GetKeyDown(KeyCode.Alpha1) && aquiredMask1)
        {
            print("useing mask 1");
            selectedMask1 = true;
            MaskIndicator.enabled = true;
            MaskIndicator.rectTransform.anchoredPosition = mask1IndicatorPos + maskIndicatorOffset;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && aquiredMask1)
        {
            print("useing mask 2");
            selectedMask1 = false;
            MaskIndicator.enabled = true;
            MaskIndicator.rectTransform.anchoredPosition = mask2IndicatorPos + maskIndicatorOffset;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && aquiredMask3)
        {
            mainCam.cullingMask |= 1 << LayerMask.NameToLayer("Mask3LayerNew"); //add layer
            mainCam.cullingMask &= ~(1 << LayerMask.NameToLayer("Mask3LayerOriginal")); //remove layer
            selectedMask1 = false;
            MaskIndicator.enabled = true;
            MaskIndicator.rectTransform.anchoredPosition = mask3IndicatorPos + maskIndicatorOffset;
        }
        if (Input.GetKeyUp(KeyCode.Alpha3) && aquiredMask3)
        {
            mainCam.cullingMask |= 1 << LayerMask.NameToLayer("Mask3LayerOriginal"); //add layer
            mainCam.cullingMask &= ~(1 << LayerMask.NameToLayer("Mask3LayerNew")); //remove layer
        }

        transform.Translate(Player1Position);
    }
    void Jump()
    {
        //Accessing the "AddForce" method of RigidBody2D to move the player up times the playerJumpAcceleration (a declared variable).
        if (jumpAmount < 1)
        {
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * playerJumpAcceleration);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Mask1"))
        {
            aquiredMask1 = true;
            Mask1.enabled = true;
            collision.gameObject.SetActive(false);
        }
        if (collision.CompareTag("Mask2"))
        {
            aquiredMask2 = true;
            Mask2.enabled = true;
            collision.gameObject.SetActive(false);
        }
        if (collision.CompareTag("Mask3"))
        {
            aquiredMask3 = true;
            Mask3.enabled = true;
            collision.gameObject.SetActive(false);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            canJump = true;
            jumpAmount = 0;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor") && !selectedMask1)
        {
            canJump = false;
        }
    }
}
