using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//船的控件脚本
public class BoatController
{
    readonly GameObject boat; //对象
    readonly Vector3 fromPosition = new Vector3(5, 1, 0);   //船的起始位置
    readonly Vector3 toPosition = new Vector3(-5, 1, 0);    //船的目标位置
    readonly Vector3[] from_positions;                      //船上的两个空位置坐标(起点处)     
    readonly Vector3[] to_positions;                        //船上的两个空位置坐标(终点处)

    int State; // to->-1; from->1,表示船在开始端还是终点端
    GameObjects[] passenger = new GameObjects[2];       //船上的人物，最多2个，至少1个才可移动
    int Speed = 10;             //移动速度
    int MovingState = -1; // Move = 1;Not Move = -1;

    public BoatController()
    {
        State = 1;
        MovingState = -1;
        
        from_positions = new Vector3[] { new Vector3(4.5F, 1.5F, 0), new Vector3(5.5F, 1.5F, 0) };
        to_positions = new Vector3[] { new Vector3(-5.5F, 1.5F, 0), new Vector3(-4.5F, 1.5F, 0) };

        boat = Object.Instantiate(Resources.Load("Perfabs/Boat", typeof(GameObject)), fromPosition, Quaternion.identity, null) as GameObject;
        boat.name = "boat";
        boat.AddComponent(typeof(ClickGUI));        //添加ClickGUI动作, 在SSAction里有定义
    }

    //获取船上空的位置，看passenger[i]哪个为null即可，返回下标i。若船满则返回-1
    public int getEmptyIndex()
    {
        for (int i = 0; i < passenger.Length; i++)
        {
            if (passenger[i] == null)
            {
                return i;
            }
        }
        return -1;
    }

    //判断船是否空，空的话返回true，不能开船
    public bool isEmpty()
    {
        for (int i = 0; i < passenger.Length; i++)
        {
            if (passenger[i] != null)
            {
                return false;
            }
        }
        return true;
    }

    //获取船上一个空位置坐标
    public Vector3 getEmptyPosition()
    {
        Vector3 pos;
        int emptyIndex = getEmptyIndex();
        if (State == -1)
        {
            pos = to_positions[emptyIndex];
        }
        else
        {
            pos = from_positions[emptyIndex];
        }
        return pos;
    }

    //上船，传入一个GameObject，把passenger中的空位置给这个GameObeject
    public void GetOnBoat(GameObjects ObjectControl)
    {
        int index = getEmptyIndex();
        passenger[index] = ObjectControl;
    }

    //下船，传入人物的name，把passenger中的改人物的位置置为空，以GameObject返回这个人物，否则返回null
    public GameObjects GetOffBoat(string passenger_name)
    {
        for (int i = 0; i < passenger.Length; i++)
        {
            if (passenger[i] != null && passenger[i].getName() == passenger_name)
            {
                GameObjects charactorCtrl = passenger[i];
                passenger[i] = null;
                return charactorCtrl;
            }
        }
        Debug.Log("Cant find passenger in boat: " + passenger_name);
        return null;
    }

    //获得这艘船的对象
    public GameObject GetGameObject()
    {
        return boat;
    }

    //改变状态
    public void ChangeState()
    {
        State = -State;
    }

    public int get_State()
    { // to->-1; from->1
        return State;
    }

    //返回魔鬼和牧师分别的人数，数组
    public int[] GetobjectsNumber()
    {
        int[] count = { 0, 0 };// [0]->priest, [1]->devil
        for (int i = 0; i < passenger.Length; i++)
        {
            if (passenger[i] == null)
                continue;
            if (passenger[i].getType() == 0)
            {
                count[0]++;
            }
            else
            {
                count[1]++;
            }
        }
        return count;
    }

    public Vector3 GetDestination()
    {
        if (State == 1) return toPosition;
        else return fromPosition;
    }

    public int GetMoveSpeed()
    {
        return Speed;
    }

    public void reset()
    {
        State = 1;
        boat.transform.position = fromPosition;
        passenger = new GameObjects[2];
        MovingState = -1;
    }

    //is Moving = 1;Not Move = -1;
    public int GetMovingState()
    {
        return MovingState;
    }

    //改变船的移动状态
    public void ChangeMovingstate()
    {
        MovingState = -MovingState;
    }
}