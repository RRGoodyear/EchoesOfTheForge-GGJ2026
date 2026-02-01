using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public GameObject[] notDestroy;

    //This variable is set to read whether the player is facing right or left and if so, flip the character sprite accordingly.
    public float speedMove;

    [SerializeField] private float maxSpeed = 3.0f;
    [SerializeField] private float moveForce = 200.0f;

    //Movement speed value; higher it is the faster the sprite moves.
    public float movementSpeed = 10f;
    public float playerJumpAcceleration;
    private float playerOriginalJumpAcceleration;
    private int jumpAmount = 0;
    private Rigidbody2D rb;
    private float rbGravityOriginal;

    private bool canMove;
    private bool canJump;
    private bool facingRight;
    private bool stickySurface = false;

    public Camera mainCam;
    public GameObject maskWhiteBox;
    private SpriteRenderer maskWhiteBoxRend;

    private bool aquiredMask1 = false;
    private bool aquiredMask2 = false;
    private bool aquiredMask3 = false;
    public bool selectedMask1 = false;
    public bool selectedMask2 = false;
    public bool selectedMask3 = false;

    public PhysicsMaterial2D normalPhysicalMat;
    public PhysicsMaterial2D stickyPhysicalMat;
    private Collider2D playerCollider;

    //UI
    public Image MaskIndicator;
    public Image Mask1; 
    public Image Mask2;  
    public Image Mask3;
    public TMP_Text deathCountText;
    
    private Vector2 mask1IndicatorPos; Vector2 mask2IndicatorPos; Vector2 mask3IndicatorPos;
    private Vector2 maskIndicatorOffset;

    //checkpoint
    private int playerDeathCount = 0;
    private Transform recentCheckPoint;
    private void Awake()
    {
        for (int i = 0;  i < notDestroy.Length; i++)
        {
            DontDestroyOnLoad(notDestroy[i]);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rbGravityOriginal = rb.gravityScale;
        playerOriginalJumpAcceleration = playerJumpAcceleration;
        maskWhiteBoxRend = maskWhiteBox.GetComponent<SpriteRenderer>();
        canMove = true;
        canJump = true;
        MaskIndicator.enabled = false; Mask1.enabled = false; Mask2.enabled = false; Mask3.enabled = false;
        mask1IndicatorPos = Mask1.rectTransform.anchoredPosition; mask2IndicatorPos = Mask2.rectTransform.anchoredPosition; mask3IndicatorPos = Mask3.rectTransform.anchoredPosition;
        maskIndicatorOffset = new Vector2(5, -50);
        deathCountText.text = "Deaths: " + playerDeathCount.ToString();
        playerCollider = GetComponent<Collider2D>();
        
    }
    private void FixedUpdate()
    {
        if (canMove)
        {
            Movement();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && aquiredMask1)
        {
            print("useing mask 1");
            rb.gravityScale *= 1.5f; playerJumpAcceleration *= 1.8f;
            playerCollider.sharedMaterial = normalPhysicalMat;
            selectedMask1 = true; selectedMask2 = false; selectedMask3 = false;
            MaskIndicator.enabled = true;
            MaskIndicator.rectTransform.anchoredPosition = mask1IndicatorPos + maskIndicatorOffset;
            maskWhiteBoxRend.color = Mask1.color;

            
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && aquiredMask2)
        {
            print("useing mask 2");
            rb.gravityScale = rbGravityOriginal; playerJumpAcceleration = playerOriginalJumpAcceleration;
            playerCollider.sharedMaterial = stickyPhysicalMat;
            selectedMask1 = false; selectedMask2 = true; selectedMask3 = false;
            MaskIndicator.enabled = true;
            MaskIndicator.rectTransform.anchoredPosition = mask2IndicatorPos + maskIndicatorOffset;
            maskWhiteBoxRend.color = Mask2.color;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && aquiredMask3)
        {
            rb.gravityScale = rbGravityOriginal; playerJumpAcceleration = playerOriginalJumpAcceleration;
            mainCam.cullingMask |= 1 << LayerMask.NameToLayer("Mask3LayerNew"); //add layer
            mainCam.cullingMask &= ~(1 << LayerMask.NameToLayer("Mask3LayerOriginal")); //remove layer
            playerCollider.sharedMaterial = normalPhysicalMat;
            selectedMask1 = false; selectedMask2 = false; selectedMask3 = true;
            MaskIndicator.enabled = true;
            MaskIndicator.rectTransform.anchoredPosition = mask3IndicatorPos + maskIndicatorOffset;
            maskWhiteBoxRend.color = Mask3.color;
        }
        if (Input.GetKeyUp(KeyCode.Alpha3) && aquiredMask3)
        {
            mainCam.cullingMask |= 1 << LayerMask.NameToLayer("Mask3LayerOriginal"); //add layer
            mainCam.cullingMask &= ~(1 << LayerMask.NameToLayer("Mask3LayerNew")); //remove layer
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
        speedMove = Input.GetAxis("Horizontal");
        if (Input.GetKey(KeyCode.D) && facingRight)
        {
            FlipPlayer();
        }

        if (Input.GetKey(KeyCode.A) && !facingRight)
        {
            FlipPlayer();
        }
        if (!selectedMask1 && !selectedMask2)
        {
            rb.linearVelocity = new Vector2(speedMove * movementSpeed, rb.linearVelocity.y);
        }
        if (selectedMask1)
        {
            rb.AddForce(Vector2.right * speedMove * moveForce);
            //Clamp max speed(but allow braking)
            rb.linearVelocity = new Vector2(Mathf.Clamp(rb.linearVelocity.x, -maxSpeed, maxSpeed),rb.linearVelocity.y);
        }
        if (selectedMask2)
        {
            if (!stickySurface)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y);
            }
            if (stickySurface)
            {
                rb.linearVelocity = new Vector2(0, 0);
                //rb.mass = 0.0f;
            }
        }
    }
    void Jump()
    {
        //Accessing the "AddForce" method of RigidBody2D to move the player up times the playerJumpAcceleration (a declared variable).
        if (jumpAmount < 1)
        {
            rb.AddForce(Vector2.up * playerJumpAcceleration);
        }
            
    
    }
    void FlipPlayer()
    {
        facingRight = !facingRight;
        //Accessing the scale of the game objetcs transform.
        Vector2 localScale = gameObject.transform.localScale;
        //Changing the scale of game object a negative (resulting in the sprite flipping).
        localScale.x *= -1;
        transform.localScale = localScale;
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
        if (collision.CompareTag("CheckPoint"))
        {
            recentCheckPoint = collision.transform;
        }
        if (collision.CompareTag("DeathTrigger"))
        {
            transform.position = recentCheckPoint.position;
            playerDeathCount += 1;
            deathCountText.text = "Deaths: " + playerDeathCount.ToString(); //update string
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Floor") && selectedMask2)
        {
            stickySurface = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            stickySurface = false;
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
