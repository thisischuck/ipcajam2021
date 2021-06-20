using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MenuController : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject HUD;
    public VolumeProfile camera;
    public bool playing;
    public float scale;
    DepthOfField a;

    public List<GameObject> objects;
    // Start is called before the first frame update
    void Start()
    {
        a = (DepthOfField)camera.components.Find(x => x.GetType() == typeof(DepthOfField));
        a.focusDistance.Override(0.1f);
    }

    public void ClickPlay()
    {
        playing = !playing;
        MainMenu.SetActive(!playing);
        HUD.SetActive(playing);

        foreach (GameObject o in objects)
        {
            o.SetActive(playing);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (playing)
            a.focusDistance.Override(Mathf.Lerp((float)a.focusDistance, 10, scale));
        else
            a.focusDistance.Override(Mathf.Lerp((float)a.focusDistance, 0.1f, scale));
    }
}
