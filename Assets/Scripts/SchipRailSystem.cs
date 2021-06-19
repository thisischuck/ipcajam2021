using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchipRailSystem : MonoBehaviour
{
    [SerializeField] List<Transform> initialValues = new List<Transform>();
    [SerializeField] Queue<Transform> rail;
    [SerializeField] Transform Ship;
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

            rb.velocity = (rail.Peek().position - transform.position).normalized * speed;

            if (Vector3.Distance(Ship.position,rail.Peek().position) <= 1.5f)
            {
                Debug.Log("Remove point");
                RemoveFirstPoint();
                Vector3 targetDirection = rail.Peek().position - Ship.position;
                float angle = Vector3.SignedAngle(targetDirection,Ship.forward,Vector3.up); 
                //fazer um lerp nisto
                transform.RotateAround(Ship.position,Vector3.up,-angle);               
            }
            
        }
    }

    void RemoveFirstPoint(){
        rail.Dequeue();
    }
}
