using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//using transform.Translate method
public class paowuxian3 : MonoBehaviour
{
    // Start is called before the first frame update
    // initial speed
    public float initSpeed = 10f;
    // two direction
    private float xSpeed, ySpeed;
    // initial angle 
    public float angle = Mathf.PI / 3;
    // gravity s
    private float gravity = 9.8f;
    // Use this for initialization
    void Start()
    {
        xSpeed = initSpeed * Mathf.Cos(angle);
        ySpeed = initSpeed * Mathf.Sin(angle);
    }

    // Update is called once per frame
    void Update()
    {
        //根据斜上抛运动公式
        Vector3 vec = new Vector3(Time.deltaTime * xSpeed, Time.deltaTime * ySpeed, 0);
        //translate进行平移变换
        this.transform.Translate(vec);
        //y方向改变
        ySpeed -= gravity * Time.deltaTime;
    }

}
