using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Vuforia;

public class button : MonoBehaviour, IVirtualButtonEventHandler
{
    // Start is called before the first frame update
    public VirtualButtonBehaviour[] vbs;
    public GameObject cub;
    public GameObject but;
    public Color[] colors;
    public int color_index;

    void Start()
    {
        vbs = GetComponentsInChildren<VirtualButtonBehaviour>();
        for (int i = 0; i < vbs.Length; i++)
        {
            vbs[i].RegisterEventHandler(this);
        }
        color_index = 1;
        colors = new Color[4];
        colors[0] = Color.white;
        colors[1] = Color.red;
        colors[2] = Color.blue;
        colors[3] = Color.green;
        cub = GameObject.Find("ImageTarget/Cube");
        but = GameObject.Find("ImageTarget/VirtualButton/Plane");
    }

    // Update is called once per frame
    void Update()
    {
       
       
    }

    public void OnButtonPressed(VirtualButtonBehaviour vb)
    {
        but.GetComponent<Renderer>().material.color = Color.red;
        if (color_index == 4) color_index = 0;
        cub.GetComponent<Renderer>().material.color = colors[color_index++];
    }

    public void OnButtonReleased(VirtualButtonBehaviour vb)
    {
        but.GetComponent<Renderer>().material.color = Color.white;
    }
}
