using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipController : MonoBehaviour
{

    /**
     * Movement
     */

    [SerializeField]
    SpaceShipParams data;

    [SerializeField]
    GameObject shipModel;
    private Camera cam;

    /**
     *  SHOOTING
     */


    [SerializeField]
    Transform gunEnd;

    private WaitForSeconds shotDuration = new WaitForSeconds(0.07f);

    private AudioSource gunAudio;
    private float nextFire;

    /**
     * Barrel Roll
     */

    bool doingBarrelRoll = false;


    /**
     * LIFE SYSTEM
     */
    float nextLifeRecount = 1f;
    float lifeRecountPeriod = 1f;


    void Start()
    {
        cam = Camera.main;
        gunAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        DoTheBarrelRoll();
        MovementUpdate();

        if (!doingBarrelRoll)
        {
            LookAtUpdate();
            ShootUpdate();
        }

        LifeUpdate();
    }

    void LifeUpdate()
    {
        Debug.Log(data.shipLife);
        if (Time.time > nextLifeRecount)
        {
            nextLifeRecount += lifeRecountPeriod;

            // 1 - COUNT ENEMIES GLUED TO SHIP
            var enemies = GetEnymiesGluedToShip();

            // 2 - if bigger than 0 remove X for each enemy
            if (enemies.Length > 0)
            {
                foreach (var enemy in GetEnymiesGluedToShip())
                {
                    RemoveLife(enemy.DamageTheShipValue());
                }
            }
            else
            {
                // 3 - else add life to th ship
                AddLife(data.lifeRecovery);
            }
        }
    }

    void DoTheBarrelRoll()
    {
        if (doingBarrelRoll == false && Input.GetButtonDown("Jump"))
        {
            doingBarrelRoll = true;
            StartCoroutine(Rotate(data.barrelRollTime));
            ReleaseTheGluedVirus();
        }
    }

    BaseEnemy[] GetEnymiesGluedToShip()
    {
        return GetComponentsInChildren<BaseEnemy>();
    }

    void ReleaseTheGluedVirus()
    {
        foreach (var enemy in GetEnymiesGluedToShip())
        {
            enemy.Release();
        }
    }

    IEnumerator Rotate(float duration)
    {
        Quaternion startRot = transform.rotation;

        float t = 0.0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            transform.rotation = startRot * Quaternion.AngleAxis(t / duration * 360f, Vector3.forward);
            yield return null;
        }
        transform.rotation = startRot;
        doingBarrelRoll = false;
    }

    void MovementUpdate()
    {

        /**
         * Movement 
        */

        var movement = Vector3.zero;
        movement.x += Input.GetAxis("Horizontal");
        movement.y += Input.GetAxis("Vertical");

        //transform.Translate(movement * data.speed * Time.deltaTime, Space.Self);
        Vector3 velocity = new Vector3(movement.x * data.speed, movement.y * data.speed, 0f);
        GetComponent<Rigidbody>().velocity = velocity;


        Vector3 pos = cam.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        transform.position = cam.ViewportToWorldPoint(pos);

    }
    void LookAtUpdate()
    {

        /**
         * Rotation
        */
        Vector3 mouse = Input.mousePosition;
        Vector3 mouseWorld = cam.ScreenToWorldPoint(new Vector3(mouse.x, mouse.y, 9f));
        Vector3 forward = mouseWorld - transform.position;
        transform.rotation = Quaternion.LookRotation(forward, Vector3.up);
    }

    void ShootUpdate()
    {
        if (Input.GetButtonDown("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + data.fireRate;

            gunAudio.Play();
            Vector3 rayOrigin = gunEnd.position;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, data.weaponRange))
            {
                GameObject bullet = BulletPooling.SharedInstance.GetPooledObject();
                if (bullet != null)
                {
                    Debug.Log("A");
                    bullet.transform.position = gunEnd.transform.position;
                    bullet.transform.rotation = gunEnd.transform.rotation;
                    bullet.GetComponent<BulletController>().SetGunDamage(data.gunDamage);
                    bullet.GetComponent<BulletController>().RestartCountdown();
                    bullet.SetActive(true);

                    var dir = (hit.point - gunEnd.transform.position).normalized;
                    bullet.GetComponent<Rigidbody>().velocity = dir * data.bulletSpeed;
                }
            }
            else
            {
                // TODO ADD SOMETHING HERE ? IS THIS EVEN POSSIBLE 
            }
        }
    }


    public void RemoveLife(float totalDamage)
    {
        data.shipLife -= totalDamage;
        if (data.shipLife < 0f)
        {
            // TODO YOU LOSE 
        }
    }
    public void AddLife(float lifeRecovery)
    {
        data.shipLife += lifeRecovery;
        if (data.shipLife > data.maxShipLife)
        {
            data.shipLife = data.maxShipLife;
        }
    }
}
