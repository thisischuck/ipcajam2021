using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerManager : MonoBehaviour
{
    [SerializeField] public List<Transform> railPoints;
    [SerializeField] Module module;
    // Start is called before the first frame update

    private void OnTriggerEnter(Collider other) {
        Debug.Log("Trigger" + gameObject.name);
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player");
            if (module.railPoints.Count == 0)
            {
                module.railPoints = railPoints;
            }
        }
    }
}
