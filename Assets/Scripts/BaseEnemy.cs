using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField] AnimationCurve animationCurve;

    private float curveDeltaTime = 0.0f;
    private bool isNotInCollisionWithPlayer = true;
    public int currentHealth = 3;

    private Transform _initialParent;

    void Start()
    {
        _initialParent = transform.parent;
    }

    void Update()
    {
        if (isNotInCollisionWithPlayer)
        {
            Vector3 currentPosition = transform.position;
            curveDeltaTime += Time.deltaTime;
            currentPosition.y = animationCurve.Evaluate(curveDeltaTime);
            
            transform.position = currentPosition;

            if(curveDeltaTime >= 1f) {
                curveDeltaTime = 0.0f;
            }
        }
    }

    public void Damage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            // AQUI E DESATIVAR ESCUDO
            gameObject.SetActive(false);
        }
    }

    void OnCollisionEnter(Collision collided)
    {
        if (collided.gameObject.tag == "Player" && isNotInCollisionWithPlayer)
        {
            isNotInCollisionWithPlayer = false;
            //GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            //GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<SphereCollider>().enabled = false;
            transform.SetParent(collided.gameObject.transform);

        }
    }

    public bool IsNotInCollisionWithPlayer()
    {
        return isNotInCollisionWithPlayer;
    }


    public void Release()
    {
        StartCoroutine(ReleaseCoroutine());
    }
    IEnumerator ReleaseCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        transform.SetParent(_initialParent);
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<SphereCollider>().enabled = true;
        GetComponent<Rigidbody>().AddForce(transform.forward * 10f, ForceMode.Impulse);

    }
}
