using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyStates currentstate;
    public Collider2D ChaseArea;
    public Collider2D AttackArea;
    private Animator anim;
    public ContactFilter2D PlayerFilter;

    [SerializeField]
    public EnemyStates[] states = new EnemyStates[3];
    // Start is called before the first frame update
    void Start()
    {
        anim = this.GetComponent<Animator>();
        states[0] = new IdleState();
        states[1] = new ChaseState();
        states[2] = new AttackState();
        foreach(EnemyStates state in states)
        {
            state.animowner = anim;
            state.self = this.gameObject;
        }
        currentstate = states[0];
    }

    // Update is called once per frame
    void Update()
    {
        DetectPlayer();
        currentstate.executeState();
    }
    public void Damage()
    {

    }
    public void DetectPlayer()
    {
        Collider2D[] player = new Collider2D[1];
        Physics2D.OverlapCollider(ChaseArea, PlayerFilter, player);
        if (player[0] != null)
        {
            states[1].target = player[0].transform;
            currentstate = states[1];

        }if (player[0] == null)
        {

            currentstate = states[0];
        }
        
    }
}

[System.Serializable]
public class EnemyStates 
{
    public Animator animowner;
    public GameObject self;
    public virtual void executeState() { }
    public Transform target;
}
public class IdleState : EnemyStates
{


    public override void executeState()
    {
        animowner.SetBool("Idle", true);
        animowner.SetBool("Chase", false);
        animowner.ResetTrigger("Attack");
    }
}
public class ChaseState : EnemyStates
{

    public override void executeState()
    {
        animowner.SetBool("Idle", false);
        animowner.SetBool("Chase", true);
        animowner.ResetTrigger("Attack");
        int yon = 1;
        if(target.position.x < self.transform.position.x)
        {
            self.transform.localScale = new Vector3(-4, self.transform.localScale.y, self.transform.localScale.z);
            yon = -1;
        }
        else
        {
            self.transform.localScale = new Vector3(4, self.transform.localScale.y, self.transform.localScale.z);
        }
        Rigidbody2D rb = self.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(yon*10, rb.velocity.y); 
    }
}
public class AttackState : EnemyStates
{

    public override void executeState()
    {
        animowner.SetBool("Idle", false);
        animowner.SetBool("Chase", false);
        animowner.SetTrigger("Attack");
    }
}