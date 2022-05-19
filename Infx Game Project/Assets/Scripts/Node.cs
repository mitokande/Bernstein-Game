using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public List<Node> connectedNodes = new List<Node>();
    // Start is called before the first frame update
    public void OnDrawGizmos()
    {
        // Draws the Light bulb icon at position of the object.
        // Because we draw it inside OnDrawGizmos the icon is also pickable
        // in the scene view.

        Gizmos.DrawIcon(transform.position, "blendSampler");
        foreach(Node n in connectedNodes)
        {
            Gizmos.DrawLine(transform.position, n.transform.position);
        }
    }
}
