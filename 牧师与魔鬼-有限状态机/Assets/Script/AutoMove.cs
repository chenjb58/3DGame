using Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class AutoMove
{
    public static AutoMove autoMove = new AutoMove();
    public FirstController firstScene;
    private int fromDevilNum;
    private int fromPriestNum;
    private int BoatCoast; // -1 -> left, 1 -> right.
    //P：船运载一个牧师
    //D：船运载一个恶魔
    //PP：船运载两个牧师
    //DD：船运载两个恶魔
    //PD：船运载一个牧师，一个恶魔
    private enum Boataction {empty, P, D, PP, DD, PD }
    private bool isFinished = true;     //动作是否执行完成
    private Boataction nextState;
    //count执行步骤(0-4)
    //0：初始 1：上了一个人 2：上了2个人 3：移动船 4：下船
    private int count = 0;
    //num表示船上的人数
    private int num = 0;

    private AutoMove() { }

    public void move()
    {
        if (isFinished)
        {
            isFinished = false;
            int[] fromCount = firstScene.fromCoast.GetobjectsNumber();
            fromPriestNum = fromCount[0];
            fromDevilNum = fromCount[1];
            BoatCoast = firstScene.boat.get_State();
            if (count == 0)
            {
                nextState = getNext();
                if ((int)nextState >= 3)
                {
                    num = 2;
                }
                else if ((int)nextState > 0) num = 1;
                else num = 0;
                count++;
            }
            Debug.Log("next state is " + nextState);
            //根据状态自动操作
            DoAction();
            Debug.Log("count:" + count);
            Debug.Log("num:" + num);
        }
    }

    //根据nextState执行三种动作:魔鬼上船、牧师上船、移动船
    //按顺序调用各种模拟点击动作。
    private void DoAction()
    {
        if (count == 1 && num != 0)
        {
            if (nextState == Boataction.D)
            {
                devilOnBoat();
            }
            else if (nextState == Boataction.DD)
            {
                devilOnBoat();
            }
            else if (nextState == Boataction.P)
            {
                priestOnBoat();
            }
            else if (nextState == Boataction.PP)
            {
                priestOnBoat();
            }
            else if (nextState == Boataction.PD)
            {
                priestOnBoat();
            }
            count++;
        }
        else if (num == 2 && count == 2)
        {
            if (nextState == Boataction.DD)
            {
                devilOnBoat();
            }
            else if (nextState == Boataction.PP)
            {
                priestOnBoat();
            }
            else if (nextState == Boataction.PD)
            {
                devilOnBoat();
            }
            count++;
        }
        else if((num == 1 && count == 2) || (num == 2 && count == 3) || (num == 0 && count == 1))
        {
            firstScene.MoveBoat();
            count++;
        }
        else if ((num == 1 && count >= 3) || (num == 2 && count >= 4) || (num == 0 && count>=2))
        {
            GetOffBoat();
        }
        isFinished = true;
    }

    //下船
    private void GetOffBoat()
    {
        //根据开始岸的情况选择下船
        if((fromPriestNum == 0 && fromDevilNum == 2) || (fromPriestNum == 0 && fromDevilNum == 0))
        {
            if (firstScene.boat.get_State() == -1) //对岸
            {
                foreach (var x in firstScene.boat.passenger)
                {
                    if (x != null)
                    {
                        firstScene.ObjectIsClicked(x);
                        break;
                    }
                }
                if (firstScene.boat.isEmpty()) count = 0;
            }
            else count = 0;
        }
        else if (((fromPriestNum == 0 && fromDevilNum == 1)) && firstScene.boat.get_State() == 1)
        {
            count = 0;
        }
        else
        {
            foreach (var x in firstScene.boat.passenger)
            {
                if (x != null && x.getType() == 1)
                {
                    firstScene.ObjectIsClicked(x);
                    count = 0;
                    break;
                }
            }
            if (count != 0)
            {
                foreach (var x in firstScene.boat.passenger)
                {
                    if (x != null)
                    {
                        firstScene.ObjectIsClicked(x);
                        count = 0;
                        break;
                    }
                }
            }
        }
    }

    //牧师上船
    private void priestOnBoat()
    {
        //开始岸
        if(BoatCoast == 1)
        {
            foreach(var x in firstScene.fromCoast.passengerPlaner)
            {
                if (x!=null && x.getType() == 0)
                {
                    firstScene.ObjectIsClicked(x);
                    return;
                }
            }
        }
        else //对岸
        {
            foreach (var x in firstScene.toCoast.passengerPlaner)
            {
                if (x != null && x.getType() == 0)
                {
                    firstScene.ObjectIsClicked(x);
                    return;
                }
            }
        }
    }

    //魔鬼上船
    private void devilOnBoat()
    {
        //开始岸
        if (BoatCoast == 1)
        {
            foreach (var x in firstScene.fromCoast.passengerPlaner)
            {
                if (x != null && x.getType() == 1)
                {
                    firstScene.ObjectIsClicked(x);
                    return;
                }
            }
        }
        else//对岸
        {
            foreach (var x in firstScene.toCoast.passengerPlaner)
            {
                if (x != null && x.getType() == 1)
                {
                    firstScene.ObjectIsClicked(x);
                    return;
                }
            }
        }
    }

    //根据状态图设置nextState
    private Boataction getNext()
    {
        Boataction next = Boataction.empty;
        if (BoatCoast == 1)
        {
            if (fromDevilNum == 3 && fromPriestNum == 3)//3P3DB
            {
                next = Boataction.PD;
            }
            else if (fromDevilNum == 2 && fromPriestNum == 3)//3P2DB
            {
                next = Boataction.DD;
            }
            else if (fromDevilNum == 1 && fromPriestNum == 3)//3P1DB
            {
                next = Boataction.PP;
            }
            else if (fromDevilNum == 2 && fromPriestNum == 2)//2P2DB
            {
                next = Boataction.PP;
            }
            else if (fromDevilNum == 3 && fromPriestNum == 0)//3DB
            {
                next = Boataction.DD;
            }
            else if (fromDevilNum == 1 && fromPriestNum == 1)//1P1DB
            {
                next = Boataction.PD;
            }
            else if (fromDevilNum == 2 && fromPriestNum == 0)//2DB
            {
                next = Boataction.D;
            }
            else if (fromDevilNum == 1 && fromPriestNum == 2)//2P1DB
            {
                next = Boataction.P;
            }
            else if (fromDevilNum == 2 && fromPriestNum == 1)//1P2DB
            {
                next = Boataction.P;
            }
            else if (fromDevilNum == 1 && fromPriestNum == 0)//1DB
            {
                next = Boataction.D;
            }
            else if(fromDevilNum == 3 && fromPriestNum == 2)//2P3DB
            {
                next = Boataction.D;
            }
            else next = Boataction.empty;
        }
        else
        {
            if (fromDevilNum == 2 && fromPriestNum == 2)//2P2D
            {
                next = Boataction.empty;
            }
            else if (fromDevilNum == 1 && fromPriestNum == 3)//3P1D
            {
                next = Boataction.empty;
            }
            else if (fromDevilNum == 2 && fromPriestNum == 3)//3P2D
            {
                next = Boataction.D;
            }
            else if (fromDevilNum == 0 && fromPriestNum == 3)//3P
            {
                next = Boataction.empty;
            }
            else if (fromDevilNum == 1 && fromPriestNum == 1)//1P1D
            {
                next = Boataction.D;
            }
            else if (fromDevilNum == 2 && fromPriestNum == 0)//2D
            {
                next = Boataction.D;
            }
            else if (fromDevilNum == 1 && fromPriestNum == 0)//1D
            {
                next = Boataction.empty;
            }
            else next = Boataction.empty;
        }
        return next;
    }

    public void restart()
    {
        count = 0;
        num = 0;
    }
}
