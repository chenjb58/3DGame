using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//接口定义
namespace Interfaces
{
    //场记
    public interface ISceneController
    {
        void LoadResources();//实现加载资源的功能
    }

    //用户动作
    public interface UserAction
    {
        void MoveBoat();                //移动船
        void ObjectIsClicked(GameObjects characterCtrl);   //人物被点击
        void Restart();                 //重新开始
    }

    //定义枚举类型，事件类型，Started / Completed
    public enum SSActionEventType : int { Started, Completed }

    //回调函数
    public interface SSActionCallback
    {
        void SSActionCallback(SSAction source);
    }
}