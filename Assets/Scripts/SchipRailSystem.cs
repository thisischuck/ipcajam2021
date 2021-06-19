using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchipRailSystem : MonoBehaviour
{
    [SerializeField] List<Transform> initialValues = new List<Transform>();
    [SerializeField] Queue<Transform> rail;

    Rigidbody rb;

    float speed = 0;

    float maxSpeed = 2f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rail = new Queue<Transform>();
        foreach (var position in initialValues)
        {
            rail.Enqueue(position);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (rail.Count > 0)
        {
            //fazer o lerp em funçao do tempo
            if (Mathf.Abs(speed) < maxSpeed)
            {
                Debug.Log((rail.Peek().position - transform.position).normalized);
                rb.AddForce((rail.Peek().position - transform.position).normalized,ForceMode.Impulse);
            }
            else
            {
                rb.velocity = rb.velocity - rb.velocity/2;
            }
            speed = rb.velocity.x + rb.velocity.y + rb.velocity.z;
            //Debug.Log(rb.velocity + " : " + speed);
            //transform.position = Vector3.Lerp(transform.position, rail.Peek().position,.5f);
            //transform.position = rail.Peek().position;
            //Debug.Log(rail.Count);
            //Debug.Log(transform.position + "  " + rail.Peek().position + "  " + Vector3.Distance(transform.position,rail.Peek().position));
            if (Vector3.Distance(transform.position,rail.Peek().position) <= 0.5f)
            {
                rb.velocity = Vector3.zero;
                RemoveFirstPoint();
                Debug.Log("Remove point");
                
            }
            
        }
    }

    void RemoveFirstPoint(){
        rail.Dequeue();
    }
}
