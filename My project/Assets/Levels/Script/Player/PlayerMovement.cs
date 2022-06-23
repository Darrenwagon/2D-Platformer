using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // creating reference so that you can use it in the code.
   [SerializeField] private float speed;
   [SerializeField] private float jumpPower;
   [SerializeField] private LayerMask groundLayer;
   [SerializeField] private LayerMask wallLayer;
   private Rigidbody2D body;
   private Animator anim;
   private float wallJumpCoolDown;
   /*private bool grounded;*/
   private BoxCollider2D boxCollider;
   private float horizontalInput;
   

    //awake is called everytime the script is loaded.
   private void Awake()
   {
       // As this script is attached to the player object the get component method 
       // will check for the component Rigidbody2d and store it inside the body variable.
      body = GetComponent<Rigidbody2D>();

      // Will check for the component Animator and store it inside the anim variable.
      anim = GetComponent<Animator>();

      // Will check for the component boxcollider2D and store it inside the boxCollider variable.
      boxCollider = GetComponent<BoxCollider2D>();
   }

    //Update runs automatically on unity, it runs on every frame of the game. It make
    //sures that in every frame of the game the inputs are recorded.
    private void Update()
    {
        //makes it easy to write code in the future
        horizontalInput = Input.GetAxis("Horizontal");

        //To flip the character in accordance to the direction the character is moving this 
        //only changes in the x axis.
        if (horizontalInput > 0.01f)//look right 
            transform.localScale = Vector3.one;

        else if (horizontalInput < -0.01f)//look left
            transform.localScale = new Vector3(-1, 1, 1);


        // set animator parameters
        anim.SetBool("walk", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());

        //wall jump logic.
        if (wallJumpCoolDown > 0.2f)
        {
            //to change how fast the player is moving and which direction he is moving.
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            //Wall jump logic.
            if (onWall() && !isGrounded())
            {
                body.gravityScale = 0;//The player will slide off the wall slowly.
                body.velocity = Vector2.zero;//When the player jumps on the wall he will get stuck.
            }
            else
                //set the gravity to same as the one in the level.
                body.gravityScale = 7;

            //The code is checking if the space key is preased.
            if (Input.GetKey(KeyCode.Space))
                //calling the jump method
                Jump();
        }
        else
            //Make wall jump cooldown constant rate regardless of the framerate.
            wallJumpCoolDown += Time.deltaTime;
    }

    //Creating a private method to make for easier implementation.
    private void Jump()
    {
        if (isGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower);//for jumping.
            anim.SetTrigger("jump");//setting the animation for jump.
            /*grounded = false;*///the value of the grounded variable is changed to false.
        }
        else if (onWall() && !isGrounded())
        {
            if (horizontalInput == 0)
            {
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x)*10, 0);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x)*3, 6);

            //To make sure that the player will wait befor jumping again.
            wallJumpCoolDown = 0;
        }

    }

    //Checking if the player element in colliding with another element with a collision box.
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
        /*if (collision.gameObject.tag == "Ground")//Checking if the colliding object is ground.
            grounded = true;//Set the grounded variable to true.
        */
    //}

    //This is to make sure to check if the player element is grounded.
    private bool isGrounded()
    {
        //This is to create a ray cast around the player element to check if there is any collisions.
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;//return true when the player is grounded.
    }

    //This is to check if the player element is near a wall/on wall
    private bool onWall()
    {
        //This is to create a ray cast around the player element to check if there is any collisions.In this case we will be changing the direction
        //to the direction the character is facing.
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;//return true when the player is grounded.
    }

    //Allow the player to attack when the player is not moving grounded or on the wall.
    public bool canAttack()
    {
        return horizontalInput == 0 && isGrounded() && !onWall();
    }
}
