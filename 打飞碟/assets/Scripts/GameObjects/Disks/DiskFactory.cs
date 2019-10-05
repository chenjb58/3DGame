using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskFactory { 
    public GameObject diskPrefab;
    public static DiskFactory DF = new DiskFactory();

    private Dictionary<int, Disk> used = new Dictionary<int, Disk>();//used是用来保存正在使用的飞碟 
    private List<Disk> free = new List<Disk>();//free是用来保存未激活的飞碟 

    private DiskFactory()
    {
        diskPrefab = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/disk"));//获取预制的游戏对象
        diskPrefab.AddComponent<Disk>();
        diskPrefab.SetActive(false);
    }

    //更新used和free列表里的飞碟
    public void FreeDisk()
    {
        foreach (Disk x in used.Values)
        {
            if (!x.gameObject.activeSelf)
            {
                free.Add(x);
                used.Remove(x.GetInstanceID());
                return;
            }
        }
    }

    //获取一个free的飞碟
    public Disk GetDisk(int round)  
    {
        FreeDisk();
        GameObject newDisk = null;
        Disk disk;
        if (free.Count > 0)
        {
            //从之前生产的Disk中拿出可用的
            newDisk = free[0].gameObject;
            free.Remove(free[0]);
        }
        else
        {
            //克隆预制对象，生产新Disk
            newDisk = GameObject.Instantiate<GameObject>(diskPrefab, Vector3.zero, Quaternion.identity);
        }
        newDisk.SetActive(true);
        disk = newDisk.AddComponent<Disk>();    //添加属性脚本

        int initCase;
        initCase = Random.Range(0, 5);
        /** 
         * 根据回合数来生成相应的飞碟,难度逐渐增加。
         */
        float initSpeed;
        if (round == 1)
        {
            initSpeed = Random.Range(40, 60);
        }
        else if (round == 2)
        {
            initSpeed = Random.Range(60, 90);
        }
        else {
            initSpeed = Random.Range(90, 120);
        } 
        //根据initCase的不同设计飞碟的颜色、初速度、初始化位置等
        switch (initCase)  
        {  
             
            case 0:  
                {  
                    disk.color = Color.yellow;  
                    disk.speed = initSpeed;  
                    float RanX = UnityEngine.Random.Range(-1f, 1f) < 0 ? -1 : 1;  
                    disk.Direction = new Vector3(RanX, 1, 0);
                    disk.StartPoint = new Vector3(Random.Range(-130, -110), Random.Range(30,90), Random.Range(110,140));
                    break;  
                }  
            case 1:  
                {  
                    disk.color = Color.red;  
                    disk.speed = initSpeed + 10;  
                    float RanX = UnityEngine.Random.Range(-1f, 1f) < 0 ? -1 : 1;  
                    disk.Direction = new Vector3(RanX, 1, 0);
                    disk.StartPoint = new Vector3(Random.Range(-130, -110), Random.Range(30, 80), Random.Range(110, 130));
                    break;  
                }  
            case 2:  
                {  
                    disk.color = Color.black;  
                    disk.speed = initSpeed + 15;  
                    float RanX = UnityEngine.Random.Range(-1f, 1f) < 0 ? -1 : 1;  
                    disk.Direction = new Vector3(RanX, 1, 0);
                    disk.StartPoint = new Vector3(Random.Range(-130,-110), Random.Range(30, 70), Random.Range(90, 120));
                    break;  
                }
            case 3:
                {
                    disk.color = Color.yellow;
                    disk.speed = -initSpeed;
                    float RanX = UnityEngine.Random.Range(-1f, 1f) < 0 ? -1 : 1;
                    disk.Direction = new Vector3(RanX, 1, 0);
                    disk.StartPoint = new Vector3(Random.Range(130, 110), Random.Range(30, 90), Random.Range(110, 140));
                    break;
                }
            case 4:
                {
                    disk.color = Color.red;
                    disk.speed = -initSpeed - 10;
                    float RanX = UnityEngine.Random.Range(-1f, 1f) < 0 ? -1 : 1;
                    disk.Direction = new Vector3(RanX, 1, 0);
                    disk.StartPoint = new Vector3(Random.Range(130, 110), Random.Range(30, 80), Random.Range(110, 130));
                    break;
                }
            case 5:
                {
                    disk.color = Color.black;
                    disk.speed = -initSpeed - 15;
                    float RanX = UnityEngine.Random.Range(-1f, 1f) < 0 ? -1 : 1;
                    disk.Direction = new Vector3(RanX, 1, 0);
                    disk.StartPoint = new Vector3(Random.Range(130, 110), Random.Range(30, 70), Random.Range(90, 120));
                    break;
                }
        }

        if (disk.color == Color.black) disk.score = 3;
        else if (disk.color == Color.red) disk.score = 2;
        else disk.score = 1;
        used.Add(disk.GetInstanceID(), disk); //添加到使用队列里
        disk.name = disk.GetInstanceID().ToString();
        return disk;  
    }
}
