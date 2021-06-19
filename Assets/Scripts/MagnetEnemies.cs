using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetEnemies : MonoBehaviour
{
    public float speed;
  public float magnetDistance;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, magnetDistance);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            // do whathever you need here to determine if an object is a coin
            // Here I assume that all the coins will be tagged as coin
            if (hitColliders[i].tag == "Enemy")
            {

                var be = hitColliders[i].GetComponent<BaseEnemy>();
                if(be.IsNotInCollisionWithPlayer()) {
                    Transform virus = hitColliders[i].transform;
                    virus.position = Vector3.MoveTowards(virus.position, transform.position, speed * Time.deltaTime);
                }
            }
        }
    }
}
