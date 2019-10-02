using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;

//挂到空项目上,继承动作管理器和回调接口
public class FirstSceneActionManager : SSActionManager, SSActionCallback
{
    public SSActionEventType Complete = SSActionEventType.Completed;    //事件的状态是否完成

    //船移动
    public void BoatMove(BoatController Boat)
    {
        Complete = SSActionEventType.Started;
        CCMoveToAction action = CCMoveToAction.getAction(Boat.GetDestination(), Boat.GetMoveSpeed());
        addAction(Boat.GetGameObject(), action, this);  //见SSAcitonManager
        Boat.ChangeState(); //改变船的状态
    }

    //人物对象移动
    public void GameObjectsMove(GameObjects GameObject, Vector3 Destination)
    {
        Complete = SSActionEventType.Started;
        //防止撞墙,定义一个中点坐标
        Vector3 CurrentPos = GameObject.GetPosition();
        Vector3 MiddlePos = CurrentPos;
        if (Destination.y > CurrentPos.y)
        {
            MiddlePos.y = Destination.y;
        }
        else
        {
            MiddlePos.x = Destination.x;
        }
        //先移动到中间
        SSAction action1 = CCMoveToAction.getAction(MiddlePos, GameObject.GetMoveSpeed());
        //在移动到目的地
        SSAction action2 = CCMoveToAction.getAction(Destination, GameObject.GetMoveSpeed());
        //CCSequenceAction是SSAction的子类
        SSAction seqAction = CCSequenceAction.getAction(1, 0, new List<SSAction> { action1, action2 });
        //这个类继承了SSActionManager,管理动作
        this.addAction(GameObject.GetGameobject(), seqAction, this);
    }

    //回调函数SSActionEventType设置为Completed
    public void SSActionCallback(SSAction source)
    {
        Complete = SSActionEventType.Completed;
    }
}