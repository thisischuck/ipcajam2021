using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchipRailSystem : MonoBehaviour
{
    [SerializeField] List<Transform> initialValues = new List<Transform>();
    [SerializeField] Queue<Transform> rail;

    Rigidbody rb;

    float speed = 3f;
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
            rb.velocity = (rail.Peek().position - transform.position).normalized * speed;

            if (Vector3.Distance(transform.position,rail.Peek().position) <= 0.5f)
            {
                RemoveFirstPoint();
                Debug.Log("Remove point");  
            }
            
        }
    }

    void RemoveFirstPoint(){
        rail.Dequeue();
    }
}
