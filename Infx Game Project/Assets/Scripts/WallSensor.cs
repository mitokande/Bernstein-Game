using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSensor : MonoBehaviour
{
    public bool colliding;
    public GrabableLedge ledge;
    public float disabletimer = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        colliding = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ledge") && disabletimer<0.5f)
        {
            ledge = collision.gameObject.GetComponent<GrabableLedge>();
            colliding = true;
            disabletimer = 3f;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            colliding = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        disabletimer -= Time.deltaTime;

        if (disabletimer > 0)
        {

            colliding = false;
        }
        else
        {
            ledge = null;

        }
    }
}
