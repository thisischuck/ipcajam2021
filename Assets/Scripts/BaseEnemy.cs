using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    private bool isNotInCollisionWithPlayer = true;
    public int currentHealth = 3;

    public void Damage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            // AQUI E DESATIVAR ESCUDO
            gameObject.SetActive(false);
        }
    }

    void OnCollisionEnter (Collision collided)
    {
        if(collided.gameObject.tag == "Player")  {
            isNotInCollisionWithPlayer = false;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<BoxCollider>().enabled = false;
            transform.SetParent(collided.gameObject.transform);
            
        }
    }

    public bool IsNotInCollisionWithPlayer() {
        return isNotInCollisionWithPlayer;
    }

}
