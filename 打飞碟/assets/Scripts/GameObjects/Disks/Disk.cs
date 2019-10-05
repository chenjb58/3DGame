using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disk : MonoBehaviour {
    //初始化的位置
    public Vector3 StartPoint { get { return gameObject.transform.position; } set { gameObject.transform.position = value; } }
    public Color color { get { return gameObject.GetComponent<Renderer>().material.color; } set { gameObject.GetComponent<Renderer>().material.color = value; } }
    //初始速度
    public float speed { get;set; }
    //方向
    public Vector3 Direction { get { return Direction; } set { gameObject.transform.Rotate(value); } }
    public int score;   //后面没用到
}
