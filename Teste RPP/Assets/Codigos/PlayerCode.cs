using System.Collections;       
using System.Collections.Generic;
using UnityEngine;



public class PlayerCode : MonoBehaviour
{
    [Header("Ground Properties")]
    public LayerMask groundLayer;
    public float groundDistance;
    public bool isGrounded;
    public Vector3[] footOffset;

    public Animator animator;



    public float speed = 2f;
    public float jumpforce = 2f;
    private Rigidbody2D rb2d; 
    private Vector2 movement;
    private float xvelocity;

    private int direction =1;
    private float originalXScale;

    RaycastHit2D leftCheck;
    RaycastHit2D rightCheck;

    

        
       
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        originalXScale = transform.localScale.x;
        animator = GetComponent<Animator>();


    }

    // Update is called once per frame
    void Update()

    {
        if (Input.GetAxis("Horizontal") != 0)
        {
            animator.SetBool("andando", true);
        }
        else
        {
            animator.SetBool("andando", false);
            
        }
        
        float horizontal = Input.GetAxisRaw("Horizontal");
        movement = new Vector2(horizontal, 0);
        if(xvelocity * direction < 0f)
        {
            Flip();
        }
        PhysicsCheck();

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            rb2d.velocity = Vector2.zero;
            rb2d.AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);
            animator.SetBool("pulando", true);
            
        }

        
    }
    private void FixedUpdate()
    {
        xvelocity = movement.normalized.x * speed;
        rb2d.velocity = new Vector2(xvelocity,rb2d.velocity.y);
        
    }
    private void Flip()
    {
        direction *= -1;
        Vector3 scale = transform.localScale;
        scale.x = originalXScale * direction;
        transform.localScale = scale;
    }
    private void PhysicsCheck()
    {
        isGrounded = false;
        leftCheck = Raycast(new Vector2(footOffset[0].x, footOffset[0].y), 
            Vector2.down, groundDistance, groundLayer);
        rightCheck = Raycast(new Vector2(footOffset[1].x, footOffset[1].y),
            Vector2.down, groundDistance, groundLayer);
            
        if (leftCheck || rightCheck)
        {                                                                                                                                                                                                                                                                                                           
            isGrounded = true;
        }
    }
    private RaycastHit2D Raycast(Vector3 origin, Vector2 rayDirection, float lenght, LayerMask mask)
    {
        Vector3 pos = transform.position;

        RaycastHit2D hit = Physics2D.Raycast(pos + origin, rayDirection, lenght, mask);

            Color color = hit ? Color.red : Color.green;
            Debug.DrawRay(pos + origin, rayDirection * lenght, color);
        return hit;
        
    }

}
