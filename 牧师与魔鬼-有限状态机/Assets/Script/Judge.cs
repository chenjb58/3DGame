using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Judge : MonoBehaviour
{

    public CoastController startCoast;      //右岸
    public CoastController endCoast;        //左岸
    public BoatController boat;             //船

    //实例化的同时同步对象
    public Judge(CoastController start, CoastController end, BoatController b)
    {
        startCoast = start;
        endCoast = end;
        boat = b;
    }

    // 检查游戏是否结束
    public int CheckGameState()
    {
        int start_priest = (startCoast.GetobjectsNumber())[0];
        int start_devil = (startCoast.GetobjectsNumber())[1];
        int end_priest = (endCoast.GetobjectsNumber())[0];
        int end_devil = (endCoast.GetobjectsNumber())[1];

        if (end_priest == 3 && end_devil == 3)
            return 2;
        int[] boat_roles = boat.GetobjectsNumber();
        //船在开始岸
        if (boat.get_State() == 1)
        {
            start_priest += boat_roles[0];
            start_devil += boat_roles[1];
        }
        else //穿在结束岸
        {
            end_priest += boat_roles[0];
            end_devil += boat_roles[1];
        }

        if ((start_priest > 0 && start_priest < start_devil) || (end_priest > 0 && end_priest < end_devil))
            return 1;
        return 0;
    }
}
