using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleManager : MonoBehaviour
{
    [SerializeField] List<Module> modulePrefabs;
    // Start is called before the first frame update
    Queue<Module> modules;

    void Start()
    {
        modules = new Queue<Module>();
        foreach (var module in modulePrefabs)
        {
            modules.Enqueue(module);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateRandomModule(){
        //meter o novo modulo na posicao certa
        var randModule = Instantiate(modulePrefabs[Random.Range(0,2)].gameObject,modules.Peek().transform.position,modules.Peek().transform.rotation);
        modules.Enqueue(randModule.GetComponent<Module>());
    }
    public Queue<Transform> nextModulePoints(){
        var railPoints = new Queue<Transform>();
        foreach (var railPoint in modules.Peek().railPoints)
        {
            railPoints.Enqueue(railPoint);
        }
        modules.Dequeue();
        return railPoints;
    }
}
