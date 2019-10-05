using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;
using UnityEngine.UI;

//挂在相机的脚本
public class InterfaceGUI : MonoBehaviour {
    UserAction userAction;
    bool isPlaying = false;
    float S;
    int round = 1;
    // Use this for initialization
    void Start () {
        userAction = SSDirector.getInstance().currentScenceController as UserAction;
        S = Time.time;
    }

    private void OnGUI()
    {
        if(!isPlaying) S = Time.time;
        //分数、时间、回合
        string text = "分数: " + userAction.GetScore().ToString() + "  时间:  " + ((int)(Time.time - S)).ToString() + "  回合:  " + round;
        GUI.Label(new Rect(10, 10, Screen.width, 50),text);

        if (!isPlaying && GUI.Button(new Rect(Screen.width / 2 - 75, Screen.height / 2 - 25, 150, 50), "开始游戏"))
        {
            S = Time.time;
            isPlaying = true;
            userAction.Restart();   //点击开始游戏，FirstSceneController开始游戏
        }
        if (isPlaying)
        {
            round = userAction.GetRound();
            if (Input.GetButtonDown("Fire1"))
            {
                //把鼠标点击的位置传给FirstScenceController，处理点击事件
                Vector3 pos = Input.mousePosition;
                userAction.Hit(pos);

            }
            if (round > 3)  //round>3要等待飞碟都用完了
            {
                round = 3;
                if (userAction.RoundStop())
                {
                    isPlaying = false;
                }
            }
        }
    }
}
