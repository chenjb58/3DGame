using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour
{
    private GameObject firework;

    // Start is called before the first frame update
    void Start()
    {
        firework = Instantiate(Resources.Load<GameObject>("Prefabs/FireworkRed"), Vector3.zero, Quaternion.identity);
    }

    
    void OnGUI()
    {
        if(GUI.Button(new Rect(50, 50, 100, 20), "Red"))
        {
            Destroy(firework);
            firework = Instantiate(Resources.Load<GameObject>("Prefabs/FireworkRed"), Vector3.zero, Quaternion.identity);
        }
        if (GUI.Button(new Rect(150, 50, 100, 20), "Green"))
        {
            Destroy(firework);
            firework = Instantiate(Resources.Load<GameObject>("Prefabs/FireworkGreen"), Vector3.zero, Quaternion.identity);
        }
        if (GUI.Button(new Rect(250, 50, 100, 20), "Blue"))
        {
            Destroy(firework);
            firework = Instantiate(Resources.Load<GameObject>("Prefabs/FireworkBlue"), Vector3.zero, Quaternion.identity);
        }
    }
}
