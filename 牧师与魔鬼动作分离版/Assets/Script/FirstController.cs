using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;

//挂在空项目上
public class FirstController : MonoBehaviour, ISceneController, UserAction
{
    InteracteGUI UserGUI;       //玩家GUI
    public CoastController fromCoast;   //右岸
    public CoastController toCoast;     //左岸
    public BoatController boat;         //船
    private GameObjects[] GameObjects;  //人物
    public Judge judge;     //裁判类

    private FirstSceneActionManager FCActionManager;    //动作场景控制器

    void Start()
    {
        FCActionManager = GetComponent<FirstSceneActionManager>();  //初始化
        judge = new Judge(fromCoast, toCoast, boat); //初始化裁判类
    }

    void Awake()    //唤醒,各类的初始化
    {
        SSDirector director = SSDirector.getInstance();
        director.currentScenceController = this;
        UserGUI = gameObject.AddComponent<InteracteGUI>() as InteracteGUI;
        GameObjects = new GameObjects[6];
        LoadResources();

    }

    public void LoadResources()  //ISceneController接口实现的方法,实例化各对象
    {
        fromCoast = new CoastController("from");
        toCoast = new CoastController("to");
        boat = new BoatController();
        GameObject water = Instantiate(Resources.Load("Perfabs/Water", typeof(GameObject)), new Vector3(0, 0.5F, 0), Quaternion.identity, null) as GameObject;
        water.name = "water";
        for (int i = 0; i < 3; i++)
        {
            GameObjects s = new GameObjects("priest");
            s.setName("priest" + i);
            s.setPosition(fromCoast.getEmptyPosition());
            s.getOnCoast(fromCoast);
            fromCoast.getOnCoast(s);
            GameObjects[i] = s;
        }

        for (int i = 0; i < 3; i++)
        {
            GameObjects s = new GameObjects("devil");
            s.setName("devil" + i);
            s.setPosition(fromCoast.getEmptyPosition());
            s.getOnCoast(fromCoast);
            fromCoast.getOnCoast(s);
            GameObjects[i + 3] = s;
        }
    }

    public void ObjectIsClicked(GameObjects Objects)    //人物被点击事件
    {
        if (FCActionManager.Complete == SSActionEventType.Started) return;
        if (Objects.isOnBoat())         //对象在船上
        {
            CoastController whichCoast;
            if (boat.get_State() == -1)
            { // to->-1; from->1
                whichCoast = toCoast;
            }
            else
            {
                whichCoast = fromCoast;
            }

            boat.GetOffBoat(Objects.getName()); //下船,操作船对象
            FCActionManager.GameObjectsMove(Objects,whichCoast.getEmptyPosition()); //移动到空位置
            Objects.getOnCoast(whichCoast); //登陆,操作人物对象
            whichCoast.getOnCoast(Objects); //登陆,操作岸对象

        }
        else  //对象在岸上
        {
            Debug.Log("On Coast!");
            CoastController whichCoast = Objects.getCoastController(); // obejects on coast

            if (boat.getEmptyIndex() == -1)
            {      
                return;
            }

            if (whichCoast.get_State() != boat.get_State())   // boat is not on the side of character
                return;

            whichCoast.getOffCoast(Objects.getName());          //下岸,操作岸对象
            FCActionManager.GameObjectsMove(Objects, boat.getEmptyPosition());      //移动
            Objects.getOnBoat(boat);    //上船,操作人物对象
            boat.GetOnBoat(Objects);    //上船,操作船对象
        }
        // UserGUI.SetState = Check();
        UserGUI.SetState = judge.CheckGameState();
    }

    public void MoveBoat()
    {
        if (FCActionManager.Complete == SSActionEventType.Started || boat.isEmpty()) return;
        FCActionManager.BoatMove(boat);
        //UserGUI.SetState = Check();
        UserGUI.SetState = judge.CheckGameState();
    }

    //有了裁判类这个函数就不用到了
    int Check()
    {   // 0->not finish, 1->lose, 2->win
        int from_priest = 0;
        int from_devil = 0;
        int to_priest = 0;
        int to_devil = 0;

        int[] fromCount = fromCoast.GetobjectsNumber();
        from_priest += fromCount[0];
        from_devil += fromCount[1];

        int[] toCount = toCoast.GetobjectsNumber();
        to_priest += toCount[0];
        to_devil += toCount[1];

        if (to_priest + to_devil == 6)      // win
            return 2;

        int[] boatCount = boat.GetobjectsNumber();
        if (boat.get_State() == -1)
        {   // boat at toCoast
            to_priest += boatCount[0];
            to_devil += boatCount[1];
        }
        else
        {   // boat at fromCoast
            from_priest += boatCount[0];
            from_devil += boatCount[1];
        }
        if (from_priest < from_devil && from_priest > 0)
        {       // lose
            return 1;
        }
        if (to_priest < to_devil && to_priest > 0)
        {
            return 1;
        }
        return 0;           // not finish
    }

    //重置
    public void Restart()
    {
        fromCoast.reset();
        toCoast.reset();
        foreach (GameObjects gameobject in GameObjects)
        {
            gameobject.reset();
        }
        boat.reset();
    }
}