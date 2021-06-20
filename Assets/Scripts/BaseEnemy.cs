using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField]
    GameObject mesh;

    [SerializeField]
    AnimationCurve animationCurve;

    private float curveDeltaTime = 0.0f;

    private bool isNotInCollisionWithPlayer = true;



    [SerializeField]
    float damageToShip = 5f;


    [SerializeField]
    int currentHealth = 3;

    private bool isSheildActive = true;

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
            currentPosition.y += animationCurve.Evaluate(curveDeltaTime);

            transform.position = currentPosition;

            if (curveDeltaTime >= 1f)
            {
                curveDeltaTime = 0.0f;
            }
        }
    }

    public void Damage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            isSheildActive = false;
            mesh.GetComponent<Renderer>().material.color = Color.red;
        }
    }

    public float DamageTheShipValue()
    {
        return damageToShip;
    }

    void OnCollisionEnter(Collision collided)
    {
        if (collided.gameObject.tag == "Player" && isNotInCollisionWithPlayer)
        {
            isNotInCollisionWithPlayer = false;

            if (isSheildActive)
            {
                var ship = collided.collider.transform.parent.GetComponent<SpaceShipController>();
                if (ship)
                {
                    ship.RemoveLife(damageToShip * 5);
                    ExplodeVirus();
                }
            }
            else
            {
                //GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                //GetComponent<Rigidbody>().isKinematic = true;
                GetComponent<SphereCollider>().enabled = false;
                transform.SetParent(collided.gameObject.transform);
            }
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

        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }

    void ExplodeVirus()
    {
        ParticleSystem exp = GetComponent<ParticleSystem>();
        exp.Play();
        GetComponent<AudioSource>().Play();
        GetComponent<Rigidbody>().AddForce(transform.forward * 10f, ForceMode.Impulse);
        //Destroy(gameObject, exp.main.duration);
        gameObject.SetActive(false);
    }
}
