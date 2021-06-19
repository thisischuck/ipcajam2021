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
    private LineRenderer laserLine;
    private float nextFire;

    /**
     * Barrel Roll
     */
    bool doingBarrelRoll = false;
    

    void Start()
    {
        cam = Camera.main;
        laserLine = GetComponent<LineRenderer>();
        gunAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        DoTheBarrelRoll();
        MovementUpdate();

        if(!doingBarrelRoll) {
            LookAtUpdate();
            ShootUpdate();
        }
    }

    void DoTheBarrelRoll() {
        if(doingBarrelRoll == false && Input.GetButtonDown("Jump")) {
            doingBarrelRoll = true;
            StartCoroutine(Rotate(data.barrelRollTime));
        }   
    }
    
      IEnumerator Rotate(float duration)
    {
        Quaternion startRot = transform.rotation;

        float t = 0.0f;
        while ( t  < duration )
        {
            t += Time.deltaTime;
            transform.rotation = startRot * Quaternion.AngleAxis(t / duration * 360f, Vector3.forward);
            yield return null;
        }
        transform.rotation = startRot;
        doingBarrelRoll = false;
    }

    void MovementUpdate() {

        /**
         * Movement 
        */

        var movement = Vector3.zero;
        movement.x += Input.GetAxis("Horizontal");
        movement.y += Input.GetAxis("Vertical");

        transform.Translate(movement * data.speed * Time.deltaTime, Space.Self);

        Vector3 pos = cam.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        transform.position = cam.ViewportToWorldPoint(pos);

    }
    void LookAtUpdate()  {

        /**
         * Rotation
        */
        Vector3 mouse = Input.mousePosition;
        Vector3 mouseWorld = cam.ScreenToWorldPoint(new Vector3(mouse.x,mouse.y,9f));
        Vector3 forward = mouseWorld - transform.position;
        shipModel.transform.rotation = Quaternion.LookRotation(forward, Vector3.up);
    }

    void ShootUpdate()
    {
        if (Input.GetButtonDown("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + data.fireRate;

            StartCoroutine(ShotEffect());
            Vector3 rayOrigin = gunEnd.position;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            laserLine.SetPosition(0, gunEnd.position);

            if (Physics.Raycast(ray, out hit, data.weaponRange))
            {
                laserLine.SetPosition(1, hit.point);

                BaseEnemy health = hit.collider.GetComponent<BaseEnemy>();

                if (health != null)
                {
                    health.Damage(data.gunDamage);
                }

                /*if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * data.hitForce);
                }*/
            }
            else
            {
                laserLine.SetPosition(1, rayOrigin + (cam.transform.forward * data.weaponRange));
            }
        }
    }

    private IEnumerator ShotEffect()
    {
        gunAudio.Play();

        laserLine.enabled = true;

        yield return shotDuration;
        laserLine.enabled = false;
    }
}
