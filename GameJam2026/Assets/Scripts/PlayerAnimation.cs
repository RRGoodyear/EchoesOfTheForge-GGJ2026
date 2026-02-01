using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private PlayerMovement playerMovement;
    private SpriteRenderer playerRenderer;
    public Sprite defaultCat;
    public Sprite mask1Cat;
    public Sprite mask2Cat;
    public Sprite mask3Cat;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        playerRenderer = GetComponent<SpriteRenderer>();
        playerRenderer.sprite = defaultCat;
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Speed", playerMovement.speedMove);
        animator.SetBool("Mask1Equipped", playerMovement.selectedMask1);
        animator.SetBool("Mask2Equipped", playerMovement.selectedMask2);
        animator.SetBool("Mask3Equipped", playerMovement.selectedMask3);
        print(playerMovement.selectedMask1);
    }
}
