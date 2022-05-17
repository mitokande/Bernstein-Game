using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controls : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator anim;
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
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 120;
    }

    // Update is called once per frame
    void Update()
    {
        movementinput = Input.GetAxis("Horizontal");
        Animate();
        turn();
        Jump();
        Attack();
        // Increase timer that controls attack combo
        timeSinceAttack += Time.deltaTime;
        // Decrease timer that disables input movement. Used when attacking
        m_disableMovementTimer -= Time.deltaTime;
    }
    public void Jump()
    {
        if (Physics2D.OverlapCircle(groundcheck.transform.position, 0.35f, Yer))
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
            anim.SetBool("onGround", false);
        }
    }
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(movementinput * movementSpeed, rb.velocity.y-0.5f);
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
            //yon = -1;
            this.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (movementinput > 0)
        {
            //yon = 1;
            this.GetComponent<SpriteRenderer>().flipX = false;
        }
        //
    }
    public void Attack()
    {
        if (Input.GetButtonDown("Fire1") && timeSinceAttack > 0.1f)
        {
            // Reset timer
            timeSinceAttack = 0.0f;

            currentAttack++;

            // Loop back to one after second attack
            if (currentAttack > 2)
                currentAttack = 1;

            // Reset Attack combo if time since last attack is too large
            if (timeSinceAttack > 1.0f)
                currentAttack = 1;

            // Call one of the two attack animations "Attack1" or "Attack2"
            anim.SetTrigger("Attack" + currentAttack);

            // Disable movement 
            m_disableMovementTimer = 0.35f;
        }

    }
}
