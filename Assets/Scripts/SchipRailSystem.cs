using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchipRailSystem : MonoBehaviour
{
    [SerializeField] List<Transform> initialValues = new List<Transform>();
    [SerializeField] Queue<Transform> rail;
    // Start is called before the first frame update
    void Start()
    {
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
            transform.position = Vector3.Lerp(transform.position, rail.Peek().position,.5f);
            //transform.position = rail.Peek().position;
            Debug.Log(rail.Count);
            Debug.Log(transform.position + "  " + rail.Peek().position + "  " + Vector3.Distance(transform.position,rail.Peek().position));
            if (Vector3.Distance(transform.position,rail.Peek().position) <= 0.1f)
            {
                RemoveFirstPoint();
                
            }
            
        }
    }

    void RemoveFirstPoint(){
        rail.Dequeue();
    }
}
