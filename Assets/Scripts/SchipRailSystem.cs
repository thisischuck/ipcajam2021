using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchipRailSystem : MonoBehaviour
{
    [SerializeField] Queue<Transform> rail;
    [SerializeField] Transform Ship;
    [SerializeField] ModuleManager moduleManager;
    Rigidbody rb;
    float speed = 3f;
    float distanceFromShip = 0;
    // Start is called before the first frame update
    void Start()
    {
        rail = new Queue<Transform>();
        foreach (var railPoint in moduleManager.nextModulePoints())
        {
            rail.Enqueue(railPoint);
        }

        distanceFromShip = Vector3.Distance(transform.position,Ship.position);
        transform.Translate((rail.Peek().position - transform.position).normalized * speed * Time.deltaTime, Space.World); 
    }

    //fazer logica dos forks
    void Update()
    {
        if (rail.Count > 0)
        {
            if (rail.Count == 1)
            {
                moduleManager.GenerateRandomModule();
                foreach (var railPoint in moduleManager.nextModulePoints())
                {
                    rail.Enqueue(railPoint);
                }
            }
            var CurPos = transform.position + transform.forward*distanceFromShip;
            var ShipPos = new Vector2(CurPos.x,CurPos.z);
            var TargPos = new Vector2(rail.Peek().position.x,rail.Peek().position.z);
            if (Vector2.Distance(ShipPos,TargPos) <= 1f)
            {
                Debug.Log("Remove point");
                RemoveFirstPoint();
                Vector3 targetDirection = rail.Peek().position - Ship.position;
                float angle = Vector3.SignedAngle(targetDirection,Ship.forward,Vector3.up); 
                //fazer um lerp nisto
                transform.RotateAround(Ship.position,Vector3.up,-angle);     
            }
            transform.Translate((rail.Peek().position - transform.position).normalized * speed * Time.deltaTime, Space.World);        
            
        }
    }

    void RemoveFirstPoint(){
        rail.Dequeue();
    }

    //call module manager to give the queue of railpoints 
    //corresponding to the side of the fork the player chose
    private void OnTriggerEnter(Collider other) {
    
    }
}
