using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public List<Transform> railPoints;

    private void Start() {
        if (gameObject.tag == "Fork")
        {
            railPoints = new List<Transform>();
        }
    }
}
