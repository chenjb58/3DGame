using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Mygame;

public class UserGUI : MonoBehaviour {
	private UserAction action;
    //status为0表示游戏还在进行，为1表示失败，为2表示获胜
	public int status = 0;
	GUIStyle style;
	GUIStyle buttonStyle;

	void Start() {
        //实例化，设置字体参数、按钮格式
		action = Director.getInstance ().currentSceneController as UserAction;

		style = new GUIStyle();
		style.fontSize = 40;
		style.alignment = TextAnchor.MiddleCenter;

		buttonStyle = new GUIStyle("button");
		buttonStyle.fontSize = 30;
	}
	void OnGUI() {
        //失败，创建按钮 
		if (status == 1) {
			GUI.Label(new Rect(Screen.width/2-50, Screen.height/2-85, 100, 50), "游戏结束!", style);
			if (GUI.Button(new Rect(Screen.width/2-70, Screen.height/2, 140, 70), "重新开始", buttonStyle)) {
				status = 0;
				action.restart ();
			}
		} else if(status == 2) {//获胜，创建按钮
			GUI.Label(new Rect(Screen.width/2-50, Screen.height/2-85, 100, 50), "您获胜了!", style);
			if (GUI.Button(new Rect(Screen.width/2-70, Screen.height/2, 140, 70), "重新开始", buttonStyle)) {
				status = 0;
				action.restart ();
			}
		}
	}
}
