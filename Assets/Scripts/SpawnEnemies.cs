using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public GameObject enemyParent;
    public GameObject enemyHolder;

    public List<GameObject> enemies;
    public float rate;
    public int size;

    public Vector3 nextRail;


    bool started;
    // Start is called before the first frame update
    void Start()
    {
        nextRail = Vector3.one;
        for (int i = 0; i < size; i++)
        {
            Vector3 pos = Vector3.zero;
            var o = Instantiate(enemyParent, pos, this.transform.rotation, enemyHolder.transform);
            o.SetActive(false);
            enemies.Add(o);
        }
    }

    IEnumerator SpawnCouroutine()
    {
        started = true;
        yield return new WaitForSecondsRealtime(rate);
        var p = new Vector3(Random.Range(-16, 10), Random.Range(-1, 1), 0);
        Spawn(p);
        started = false;
    }

    void Update()
    {
        if (!started)
            StartCoroutine("SpawnCouroutine");
    }

    void Spawn(Vector3 pos)
    {
        foreach (GameObject o in enemies)
        {
            if (!o.activeInHierarchy)
            {
                o.transform.position = pos;
                o.SetActive(true);
            }
        }
    }
}
