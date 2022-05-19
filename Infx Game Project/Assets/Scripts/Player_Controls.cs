using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Controls : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator anim;
    public PlayerCombat PC;
    public GameObject groundcheck;
    public LayerMask Yer;
    public bool isGrounded;
    public float timeSinceAttack = 0.0f;
    public float m_disableMovementTimer = 0f;
    private float movementinput;
    public float movementSpeed;
    public float jumpSpeed;
    public bool isAttack;
    public int currentAttack = 1;
    public int isFacingRight;
    public bool onledgegrab;
    public float wallx;
    public float wally;
    public WallSensor wallsensorR1;
    public WallSensor wallsensorR2;
    public WallSensor wallsensorL1;
    public WallSensor wallsensorL2;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 120;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
        if(m_disableMovementTimer < 0.0f && !onledgegrab)
        {
            movementinput = Input.GetAxis("Horizontal");
        }
        else
        {
            movementinput = 0;
        }
        Animate();
        turn();
        Jump();
        LedgeGrab();
        PC.Attack();
        // Increase timer that controls attack combo
        timeSinceAttack += Time.deltaTime;
        // Decrease timer that disables input movement. Used when attacking
        m_disableMovementTimer -= Time.deltaTime;
    }
    public void Jump()
    {
        if (Physics2D.OverlapCircle(groundcheck.transform.position, 0.55f, Yer) && !onledgegrab)
        {
            isGrounded = true;
            anim.SetBool("onGround", true);
            if (Input.GetButtonDown("Jump"))
            {
                anim.SetTrigger("Jump");
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + jumpSpeed);
            }
        }
        else
        {
            isGrounded = false;
            if (!onledgegrab)
            {
                anim.SetBool("onGround", false);
            }
        }
    }
    private void FixedUpdate()
    {
        if (!onledgegrab)
        {
            rb.velocity = new Vector2(movementinput * movementSpeed, rb.velocity.y - 0.5f);
        }
        //float grav = 0;
        //if (!isGrounded)
        //{
        //    grav = -0.001f;
        //}
        //transform.Translate(new Vector2(movementinput * movementSpeed, grav));

    }
    public void Animate()
    {
        if(movementinput != 0)
        {
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }
    }
    public void turn()
    {
        //Karakter Hangi yöne bakýyor?
        if (movementinput < 0)
        {
            isFacingRight = -1;
            this.GetComponent<SpriteRenderer>().flipX = true;
           // this.GetComponent<Transform>().localScale = new Vector3(-4, transform.localScale.y, transform.localScale.z);
        }
        else if (movementinput > 0)
        {
            //yon = 1;
            //this.GetComponent<Transform>().localScale = new Vector3(4, transform.localScale.y, transform.localScale.z);
            this.GetComponent<SpriteRenderer>().flipX = false;
            isFacingRight = 1;
        }
        //
    }
    public void LedgeGrab()
    {
        if (!isGrounded && !onledgegrab && ((!wallsensorL1.colliding && wallsensorL2.colliding)  || (!wallsensorR1.colliding && wallsensorR2.colliding)))
        {
            this.GetComponent<CapsuleCollider2D>().enabled = false;
            onledgegrab = true;
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
            anim.SetBool("onGround", true);
            anim.SetTrigger("Grab");
            //

            if (isFacingRight == 1)
            {
                var ledge = wallsensorR2.ledge;
                transform.position = ledge.transform.position + new Vector3(ledge.leftGrabPosition.x, ledge.leftGrabPosition.y, 0);
                Debug.Log(wallx + " " + wally);
            }
            else
            {
                var ledge = wallsensorL2.ledge;
                transform.position = ledge.transform.position + new Vector3(ledge.rightGrabPosition.x, ledge.rightGrabPosition.y, 0);
                Debug.Log(wallx + " " + wally);

            }
            
            //Debug.Log(wallsensorR2.transform.position + "\n" + wallsensorR2.transform.position + new Vector3(0.198f, -0.224f, 0f)); 
        }

    }
    public void Climb()
    {

        onledgegrab = false;
        if (isFacingRight == 1)
        {
            var ledge = wallsensorR2.ledge;
            transform.position = ledge.transform.position + new Vector3(ledge.topClimbPosition.x, ledge.topClimbPosition.y, 0);
           //transform.position = Vector2.Lerp(transform.position, ledge.transform.position + new Vector3(ledge.topClimbPosition.x, ledge.topClimbPosition.y, 0), Time.deltaTime*10);
        }
        else
        {
            var ledge = wallsensorL2.ledge;
            transform.position = ledge.transform.position + new Vector3(ledge.topClimbPosition.x, ledge.topClimbPosition.y, 0);

        }
        anim.SetTrigger("AfterClimb");
        rb.gravityScale = 1;
        this.GetComponent<CapsuleCollider2D>().enabled = true;

    }

}
