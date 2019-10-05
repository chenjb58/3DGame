using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//飞行的动作，由上个游戏的CCMoveToAction改动过来的
public class CCFlyAction : SSAction
{
    //速度分解为x和y方向，y方向初始为0
    public float speedx;
    public float speedy = 0;

    private CCFlyAction() { }
    //单例模式，给定初始的x方向速度
    public static CCFlyAction getAction(float speedx)
    {
        CCFlyAction action = CreateInstance<CCFlyAction>();
        action.speedx = speedx;
        return action;
    }

    //运动
    public override void Update()
    {
        // dx = vt
        float deltax = speedx * Time.deltaTime;
        // dy = yt + 0.5at^2
        float deltay = -speedy * Time.deltaTime + (float)-0.5 * 10 * Time.deltaTime * Time.deltaTime;
        this.transform.position += new Vector3(deltax, deltay, 0);
        // y' = y + at
        speedy += 10 * Time.deltaTime;
        //当飞碟的位置的y坐标小于阈值，认为消失在用户视野中，执行销毁
        if (transform.position.y <= 1)
        {
            destroy = true;
            CallBack.SSActionCallback(this);
        }
    }

    public override void Start()
    {

    }
}