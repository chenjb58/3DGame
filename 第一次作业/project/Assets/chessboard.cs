using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chessboard : MonoBehaviour {
	private int cnt = 0;
	//map[i,j] = 0表示还没下， =1表示“X”的一方， =2表示”O"的一方
	private int[,] map = new int[3, 3];

	//初始化
	private void Start()
	{
		reset();
	}
	
	void OnGUI()
	{
		int res = Check();
		//X获胜
		if (res == 1)
		{
			GUI.Label(new Rect(40, 120, 90, 50), "X方赢了!");
		}
		//O获胜
		else if (res == 2)
		{
			GUI.Label(new Rect(40, 120, 90, 50), "O方赢了!");
		}
		//平局
		else if (cnt == 9)
		{
			GUI.Label(new Rect(40, 120, 100, 50), "平局！请重新开始！");
		}
		else
		{
			for (int i = 0; i < 3; i += 1)
			{
				for (int j = 0; j < 3; j += 1)
				{
					//没有被点击的矩形
					if (map[i, j] == 0)
					{
						//创建矩形，显示空字符串
						if (GUI.Button(new Rect(i * 50, j * 50, 50, 50), ""))
						{
							if (cnt%2 == 0) map[i, j] = 1;
							else map[i, j] = 2;
							cnt = cnt + 1;
						}
					}
					if (map[i, j] == 1)
					{
						GUI.Button(new Rect(i * 50, j * 50, 50, 50), "X");
					}
					if (map[i, j] == 2)
					{
						GUI.Button(new Rect(i * 50, j * 50, 50, 50), "O");
					}
				}
			}
		}
		if (GUI.Button(new Rect(25, 150, 100, 50), "reset"))
		{
			reset();
		}
	}

	//cnt=0,map[i,j] = 0
	private void reset()
	{
		for (int i = 0; i < 3; i += 1)
			for (int j = 0; j < 3; j += 1)
				map[i, j] = 0;
		
		cnt = 0;
	}

	//如果有一条线上都是同一个玩家的，返回序号，否则返回0
	private int Check()
	{
		//横向
		for (int i = 0; i < 3; ++i)
			if (map[i, 0] != 0 && map[i, 0] == map[i, 1] && map[i, 1] == map[i, 2])
				return map[i, 0];

		//纵向
		for (int j = 0; j < 3; ++j)
			if (map[0, j] != 0 && map[0, j] == map[1, j] && map[1, j] == map[2, j])
				return map[0, j];

		//对角  
		if (map[1, 1] != 0 &&map[0, 0] == map[1, 1] && map[1, 1] == map[2, 2] 
		    ||map[0, 2] == map[1, 1] && map[1, 1] == map[2, 0])
		{
			return map[1, 1];
		}
		return 0;
	}
}
