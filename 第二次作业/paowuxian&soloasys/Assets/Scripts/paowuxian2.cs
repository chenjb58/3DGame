using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//using Vector.lerp , most from paowuxian1.cs
public class paowuxian2 : MonoBehaviour
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

    void Start()
    {
        // phisical formula
        xSpeed = initSpeed * Mathf.Cos(angle);
        ySpeed = initSpeed * Mathf.Sin(angle);
    }

    // Update is called once per frame
    void Update()
    {
        //motion vector per deltaTime
        Vector3 vec = new Vector3(Time.deltaTime * xSpeed, Time.deltaTime * ySpeed, 0);
        //change positon, only there is diffrent from paowuxian1.cs,
        //Linearly interpolates between two vectors.
        transform.position = Vector3.Lerp(transform.position, transform.position + vec, 1);
        //ySpeed is affected by gravityss
        ySpeed -= gravity * Time.deltaTime;
    }
}
