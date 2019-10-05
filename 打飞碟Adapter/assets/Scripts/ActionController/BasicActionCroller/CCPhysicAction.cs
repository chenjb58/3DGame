using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCPhysicAction : SSAction
{
    public float speedx;
    // Use this for initialization
    public override void Start()
    {
        //添加刚体组件
        if (!this.gameObject.GetComponent<Rigidbody>())
        {
            this.gameObject.AddComponent<Rigidbody>();
        }
        //添加重力
        this.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * 9.8f * 0.6f, ForceMode.Acceleration);
        //设置初速度
        this.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(speedx, 0, 0), ForceMode.VelocityChange);
    }

    private CCPhysicAction()
    {

    }
    public static CCPhysicAction getAction(float speedx)
    {
        CCPhysicAction action = CreateInstance<CCPhysicAction>();
        action.speedx = speedx;
        return action;
    }

    // Update is called once per frame
    override public void Update()
    {
        if (transform.position.y <= 3)
        {
            Destroy(this.gameObject.GetComponent<Rigidbody>());//如果不移除刚体属性会导致前面添加的速度属性累积。
            destroy = true;
            CallBack.SSActionCallback(this);
        }
    }
}
