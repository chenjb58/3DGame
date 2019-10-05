﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interfaces
{
    public interface ISceneController
    {
        void LoadResources();
    }

    public interface UserAction
    {
        void Hit(Vector3 pos);      //鼠标点击
        void Restart();             //开始和重新开始都一样
        int GetScore();             
        bool RoundStop();           //最后的回合结束
        int GetRound();             
    }   

    public enum SSActionEventType : int { Started, Completed }

    public interface SSActionCallback
    {
        void SSActionCallback(SSAction source);
    }
}