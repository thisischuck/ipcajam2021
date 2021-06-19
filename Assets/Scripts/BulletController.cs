using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private int _gunDamage = 1;
    private float _lifeTime = 5f;
    private float _lifeStartTime = 0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeSinceLevelLoad > (_lifeStartTime + _lifeTime)) {
            gameObject.SetActive(false);
        }
    }

    public void SetGunDamage(int gunDamage) {
        _gunDamage = gunDamage;
    }

    void OnCollisionEnter(Collision collision)
    {

        BaseEnemy health = collision.collider.GetComponent<BaseEnemy>();

        if (health != null)
        {
            health.Damage(_gunDamage);
        }


        gameObject.SetActive(false);
    }

    public void RestartCountdown() {
        _lifeStartTime = Time.timeSinceLevelLoad;
    }
}
