using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Player_Controls pc;
    public PlayerData pd;
    public Collider2D playerattackcollider;
    public ContactFilter2D enemylayer;
    int dmg = 0;
    // Start is called before the first frame update
    void Start()
    {
        pd.playerpos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            hit(10);
        }
    }
    public void Attack()
    {
        if (Input.GetButtonDown("Fire1") && pc.timeSinceAttack > 0.1f)
        {
            // Reset timer
            pc.timeSinceAttack = 0.0f;

            pc.currentAttack++;

            // Loop back to one after second attack
            if (pc.currentAttack > 2)
                pc.currentAttack = 1;

            // Reset Attack combo if time since last attack is too large
            if (pc.timeSinceAttack > 1.0f)
                pc.currentAttack = 1;

            // Call one of the two attack animations "Attack1" or "Attack2"
            pc.anim.SetTrigger("Attack" + pc.currentAttack);

            // Disable movement 
            pc.m_disableMovementTimer = 0.55f;
        }

    }
    public void Damage()
    {
        List<Collider2D> hits = new List<Collider2D>();
        Physics2D.OverlapCollider(playerattackcollider, enemylayer, hits);
        if(hits.Count != 0)
        {
            hits.ForEach(x => { Debug.Log(x.name); });
        }
        
    }
    public void hit(float damaga)
    {
        pd.health -= damaga;
        if(pd.health <= 0)
        {
            pc.anim.SetTrigger("Death");
        }
        else
        {
            pc.anim.SetTrigger("Hurt");

        }
    }
}
