using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;

public class FirstSceneController : MonoBehaviour, ISceneController, UserAction
{
    int score = 0;      //分数
    int round = 1;      //回合，设置了3个
    int producedDiskNum = 0;    //每回合已经产生的飞碟数目
    bool start = false;
    IActionManager Manager;
    DiskFactory DF;

    void Awake()
    {
        SSDirector director = SSDirector.getInstance();
        director.currentScenceController = this;
        DF = DiskFactory.DF;
        Manager = this.gameObject.AddComponent<CCPhysicActionManager>() as IActionManager;
    }

    // Use this for initialization
    void Start () {
        
    }

    // Update is called once per frame
    int count = 0;
	void Update () {
        //val帧产生一个飞碟
        int val;
        switch (round)
        {
            case 1:
                val = Random.Range(60, 80);
                break;
            case 2:
                val = Random.Range(45, 60);
                break;
            default:
                val = 40;
                break;
        }
        if(start)
        {
            count++;
            if (count >= val)    //val帧一个飞碟
            {
                count = 0;

                if(DF == null)
                {
                    Debug.LogWarning("DF is NUll!");
                    return;
                }
                producedDiskNum++;
                Disk d = DF.GetDisk(round);
                Manager.MoveDisk(d);
                if (producedDiskNum == 20) //20个飞碟进入下一个回合
                {
                    round++;
                    producedDiskNum = 0;
                }
            }
        }
	}

    public void LoadResources()
    {
            
    }


    public void Hit(Vector3 pos)
    {
        Ray ray = Camera.main.ScreenPointToRay(pos);

        RaycastHit[] hits;
        hits = Physics.RaycastAll(ray);
        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            //根据颜色加分
            if (hit.collider.gameObject.GetComponent<Disk>() != null)
            {
                Color c = hit.collider.gameObject.GetComponent<Renderer>().material.color;
                if (c == Color.yellow) score += 1;
                if (c == Color.red) score += 2;
                if (c == Color.black) score += 3;
               // score += hit.collider.gameObject.GetComponent<Disk>().score;    //不清楚为什么这样分数是0.。。
                //把y更改到阈值以下，工厂会判断为free
                hit.collider.gameObject.transform.position = new Vector3(0, -5, 0);
            }

        }
    }

    public int GetScore()
    {
        return score;
    }
    
    //初始化各变量
    public void Restart()
    {
        score = 0;
        round = 1;
        start = true;
    }
    //到了第三回合要判断是否结束
    public bool RoundStop()
    {
        if (round > 3)
        {
            start = false;
            return Manager.IsAllFinished();//是否还有飞碟留在used列表里
        }
        else return false;
    }
    public int GetRound()
    {
        return round;
    }
}