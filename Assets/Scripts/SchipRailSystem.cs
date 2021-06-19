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
    float distanceFromShip = 0;
    // Start is called before the first frame update
    void Start()
    {
        distanceFromShip = Vector3.Distance(transform.position,Ship.position);
        rb = GetComponent<Rigidbody>();
        rail = new Queue<Transform>();
        foreach (var position in initialValues)
        {
            rail.Enqueue(position);
        }
        rb.velocity = (rail.Peek().position - transform.position).normalized * speed; 
    }

    // Update is called once per frame
    void Update()
    {
        if (rail.Count > 0)
        {
            var CurPos = transform.position + transform.forward*distanceFromShip;
            var ShipPos = new Vector2(CurPos.x,CurPos.z);
            var TargPos = new Vector2(rail.Peek().position.x,rail.Peek().position.z);
            if (Vector2.Distance(ShipPos,TargPos) <= .5f)
            {
                Debug.Log("Remove point");
                RemoveFirstPoint();
                Vector3 targetDirection = rail.Peek().position - Ship.position;
                float angle = Vector3.SignedAngle(targetDirection,Ship.forward,Vector3.up); 
                //fazer um lerp nisto
                transform.RotateAround(Ship.position,Vector3.up,-angle);     
                rb.velocity = (rail.Peek().position - transform.position).normalized * speed;          
            }
            
        }
    }

    void RemoveFirstPoint(){
        rail.Dequeue();
    }
}
