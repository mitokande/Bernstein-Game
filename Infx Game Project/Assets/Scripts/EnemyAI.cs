using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public List<Node> nodes = new List<Node>();
    public Node targetNode;
    public Node closestNode;
    public Transform target;
    public List<Node> path;

    public Rigidbody2D rb;
    private float movementinput;
    public float movementSpeed;
    private void Awake()
    {
        nodes = FindObjectsOfType<Node>().ToList();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    Node GetClosestNodeTo(Transform t)
    {
        Node fnode = null;
        float minDistance = Mathf.Infinity;
        foreach(Node n in nodes)
        {
            float distance = Vector3.Distance(n.transform.position, t.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                fnode = n;
            }
        }
        
        
        return fnode;
    }
    // Update is called once per frame
    void Update()
    {
        if (!GetClosestNodeTo(target).Equals(targetNode))
        {
            FindPath();
        }
        MoveTowardsPath();
        rb.velocity = new Vector2(movementinput * movementSpeed, rb.velocity.y - 0.5f);

    }

    public void FindPath()
    {
        path.Clear();
        targetNode = GetClosestNodeTo(target);
        closestNode = GetClosestNodeTo(transform);
        if(targetNode == null || closestNode == null)
        {
            Debug.Log("Error");
            return;
        }
        HashSet<Node> VisitedNodes = new HashSet<Node>();
        Queue<Node> Q = new Queue<Node>();
        Dictionary<Node, Node> nodeAndParent = new Dictionary<Node, Node>();
        Q.Enqueue(closestNode);
        while (Q.Count > 0)
        {
            Node n = Q.Dequeue();
            if (n.Equals(targetNode))
            {
                MakePath(nodeAndParent);
                return;
            }
            foreach(Node f in n.connectedNodes)
            {
                if (!VisitedNodes.Contains(f))
                {
                    VisitedNodes.Add(f);
                    nodeAndParent.Add(f,n);
                    Q.Enqueue(f);

                }
            }
        }
    }

    public void MakePath(Dictionary<Node,Node> nap)
    {
        if (nap.Count > 0)
        {
            if (nap.ContainsKey(targetNode))
            {
                Node cnode = targetNode;
                while(cnode != closestNode)
                {
                    path.Add(cnode);
                    cnode = nap[cnode];
                }
                path.Add(closestNode);
                path.Reverse();
            }
        }
    }
    public void MoveTowardsPath()
    {
        Node currentNode = path.First();

        if (path.Count > 0)
        {
            var xmag = Mathf.Abs(currentNode.transform.position.x - transform.position.x);
            var ymag = Mathf.Abs(currentNode.transform.position.y - transform.position.y);
            Debug.Log("X:" + xmag + " Y:" + ymag);
            if(currentNode && xmag >= 2 && ymag<= 5)
            {
                if(transform.position.x > currentNode.transform.position.x)
                {
                    movementinput = -1;
                }
                if (transform.position.x < currentNode.transform.position.x)
                {
                    movementinput = 1;
                }
                if (transform.position.y < currentNode.transform.position.y && ymag>3)
                {
                    rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + movementSpeed);
                }
            }
            else
            {
                if (path.Count > 1)
                {
                    path.Remove(path.First());
                }
                if (path.First() == targetNode && Vector2.Distance(currentNode.transform.position, transform.position) < 2)
                {
                    path.Clear();
                }
            }
        }
        
    }
}
