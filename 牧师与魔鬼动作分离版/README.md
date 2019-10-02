## **游戏对象与图形基础-作业与练习**

----

### 1、基本操作演练

- **下载天空和skybox， 构建自己的游戏场景**

  从Asset Store下载skybox然后导入

  ![1569915983221](assets/1569915983221.png)

- 制作天空盒

  - 在Assets中右击->Create->Material
  - 将shader改为Skybox/6 sided并把对应的图片放进去

  ![1569916763481](assets/1569916763481.png)

- 创建地图
  - 在对象栏右击->3D Object->Terrain，新建一个地图对象

  - 使用Terrain的各项工具绘制地图, 包括造山，造草，添加细节等等

    ![1569916926740](assets/1569916926740.png)

  - 先画地形

    ![1569917496459](assets/1569917496459.png)

    ![1569917508639](assets/1569917508639.png)

  - 种草种树

    ![1569919009036](assets/1569919009036.png)

    (好丑) 镜头拉远会看不到草跟树游戏对象使用总结：

- 目前学过的有基础掌握的对象主要有：
  - Camera:
    通过Camera来观察游戏世界，看到Camera里投影。
  - Light:
    光源，可以用来照明也可用于添加阴影等效果。
  - Empty：
    空对象多被用于当做载体，例如挂载游戏脚本、成为其他对象的父对象等。
  - Cube、sphere、Capsule等3D Object：
    搭建游戏世界的组成元素，通过设置其Transform等属性来变换它们的位置、形态等。
  - Terrain等:
    Terrain本身是地图，然后又附带了绘制地图的各项工具（造山、造草等）。

----

### 编程实践: 牧师与魔鬼分离版

- 【2019新要求】：设计一个裁判类，当游戏达到结束条件时，通知场景控制器游戏结束

  结构：
  
  ![1570033173922](assets/1570033173922.png)
  
  1. 先把Judge裁判类设计好，其实就是把先前检查游戏结束条件的方法抽出来构成一个类而已，FirstController类调用CheckGameState()方法查看游戏是否结束(0为正在进行，1为失败，2为获胜)
  
  ```c#
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
  ```
  
  2. SSAction类：SSAction是所有动作的基类，ScriptableObject 是不需要绑定 GameObject 对象的可编程基类。
  
  ```cs
  using System.Collections;
  using System.Collections.Generic;
  using UnityEngine;
  using Interfaces;
  
  //动作分离
  public class SSAction : ScriptableObject {
  
      public bool enable = true;
      public bool destroy = false;
  
      public GameObject gameObject;       //动作发出的对象
      public Transform transform;         //对象的transform
      public SSActionCallback CallBack;   //回调函数
  
      public virtual void Start()
      {
          throw new System.NotImplementedException();
      }
  
      public virtual void Update()
      {
          throw new System.NotImplementedException();
      }
  }
  
  //移动动作，继承SSAction
  public class CCMoveToAction : SSAction
  {
      public Vector3 target;         //终点
      public float speed;            //速度
  
      private CCMoveToAction() { }
      //静态函数
      public static CCMoveToAction getAction(Vector3 target, float speed)
      {
          //单例模式
          CCMoveToAction action = ScriptableObject.CreateInstance<CCMoveToAction>();
          action.target = target;
          action.speed = speed;
          return action;
      }
  
      //重写Update函数
      public override void Update()
      {
          //移动
          this.transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
          //到达目的地设置destroy为true，销毁这个动作，回调
          if (transform.position == target)
          {
              destroy = true;
              CallBack.SSActionCallback(this);
          }
      }
  
      public override void Start()
      {
  
      }
  }
  
  //动作列表
  public class CCSequenceAction : SSAction, SSActionCallback
  {
      public List<SSAction> sequence;         //存储一连续的动作
      public int repeat = 1;                  // 1->only do it for once, -1->repeat forever
      public int currentActionIndex = 0;      //当前轮到的动作下标
  
      //获取动作
      public static CCSequenceAction getAction(int repeat, int currentActionIndex, List<SSAction> sequence)
      {
          CCSequenceAction action = ScriptableObject.CreateInstance<CCSequenceAction>();
          action.sequence = sequence;
          action.repeat = repeat;
          action.currentActionIndex = currentActionIndex;
          return action;
      }
  
      //重写Update
      public override void Update()
      {
          if (sequence.Count == 0) return;
          if (currentActionIndex < sequence.Count)
          {
              sequence[currentActionIndex].Update();//调用sequence里当前的做的Update方法
          }
      }
  
      //接口要求重写的函数,传入一个动作进行回调
      public void SSActionCallback(SSAction source)
      {
          source.destroy = false;
          this.currentActionIndex++; //轮到下一个动作
          if (this.currentActionIndex >= sequence.Count)  //超过范围
          {
              this.currentActionIndex = 0;
              if (repeat > 0) repeat--;
              if (repeat == 0)
              {
                  this.destroy = true;
                  this.CallBack.SSActionCallback(this);
              }
          }
      }
  
      public override void Start()
      {
          //对每个动作进行初始化
          foreach (SSAction action in sequence)
          {
              action.gameObject = this.gameObject;
              action.transform = this.transform;
              action.CallBack = this;
              action.Start();
          }
      }
  
  
      void OnDestroy()
      {
          foreach (SSAction action in sequence)
          {
              Destroy(action);        //销毁对象(动作)
          }
      }
  }
  
  //动作管理器
  public class SSActionManager : MonoBehaviour
  {
      private Dictionary<int, SSAction> actions = new Dictionary<int, SSAction>();        //正在进行
      private List<SSAction> waitingToAdd = new List<SSAction>();                         //等待添加
      private List<int> watingToDelete = new List<int>();                                 //等待销毁
  
      protected void Update()
      {
          //等待队列里的动作都添加到actions
          foreach (SSAction ac in waitingToAdd)
          {
              actions[ac.GetInstanceID()] = ac;
          }
          waitingToAdd.Clear();
          //获取每一个动作,检查是否完成
          foreach (KeyValuePair<int, SSAction> kv in actions)
          {
              SSAction ac = kv.Value;
              //动作的destroy属性为true是加入到销毁对列里
              if (ac.destroy)
              {
                  watingToDelete.Add(ac.GetInstanceID());
              }
              else if (ac.enable) 
              {
                  ac.Update();
              }
          }
          //对销毁队列的每一个动作
          foreach (int key in watingToDelete)
          {
              SSAction ac = actions[key];
              actions.Remove(key);
              Destroy(ac);
          }
          watingToDelete.Clear();
      }
  
      //添加动作,传入对象,动作,和回调函数
      public void addAction(GameObject gameObject, SSAction action, SSActionCallback ICallBack)
      {
          action.gameObject = gameObject;
          action.transform = gameObject.transform;
          action.CallBack = ICallBack;
          waitingToAdd.Add(action);
          action.Start();
      }
  }
  ```
  
  ------
  
  3. CCMoveToAction类(SSACtion.cs里)：实现简单的动作，并且管理内存回收以及重写Update()函数实现物体的运动。
  
  ```cs
  //移动动作，继承SSAction
  public class CCMoveToAction : SSAction
  {
      public Vector3 target;         //终点
      public float speed;            //速度
  
      private CCMoveToAction() { }
      //静态函数
      public static CCMoveToAction getAction(Vector3 target, float speed)
      {
          //单例模式
          CCMoveToAction action = ScriptableObject.CreateInstance<CCMoveToAction>();
          action.target = target;
          action.speed = speed;
          return action;
      }
  
      //重写Update函数
      public override void Update()
      {
          //移动
          this.transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
          //到达目的地设置destroy为true，销毁这个动作，回调
          if (transform.position == target)
          {
              destroy = true;
              CallBack.SSActionCallback(this);
          }
      }
  
      public override void Start()
      {
  
      }
  }
  ```
  
  ------
  
  4. CCSequenceAction类(SSAction.cs里)：创建动作执行序列，按要求循环执行保存的动作序列。
  
  ```cs
  //动作列表
  public class CCSequenceAction : SSAction, SSActionCallback
  {
      public List<SSAction> sequence;         //存储一连续的动作
      public int repeat = 1;                  // 1->only do it for once, -1->repeat forever
      public int currentActionIndex = 0;      //当前轮到的动作下标
  
      //获取动作
      public static CCSequenceAction getAction(int repeat, int currentActionIndex, List<SSAction> sequence)
      {
          CCSequenceAction action = ScriptableObject.CreateInstance<CCSequenceAction>();
          action.sequence = sequence;
          action.repeat = repeat;
          action.currentActionIndex = currentActionIndex;
          return action;
      }
  
      //重写Update
      public override void Update()
      {
          if (sequence.Count == 0) return;
          if (currentActionIndex < sequence.Count)
          {
              sequence[currentActionIndex].Update();//调用sequence里当前的做的Update方法
          }
      }
  
      //接口要求重写的函数,传入一个动作进行回调
      public void SSActionCallback(SSAction source)
      {
          source.destroy = false;
          this.currentActionIndex++; //轮到下一个动作
          if (this.currentActionIndex >= sequence.Count)  //超过范围
          {
              this.currentActionIndex = 0;
              if (repeat > 0) repeat--;
              if (repeat == 0)
              {
                  this.destroy = true;
                  this.CallBack.SSActionCallback(this);
              }
          }
      }
  
      public override void Start()
      {
          //对每个动作进行初始化
          foreach (SSAction action in sequence)
          {
              action.gameObject = this.gameObject;
              action.transform = this.transform;
              action.CallBack = this;
              action.Start();
          }
      }
  
  
      void OnDestroy()
      {
          foreach (SSAction action in sequence)
          {
              Destroy(action);        //销毁对象(动作)
          }
      }
  }
  ```
  
  ------
  
  5. SSActionCallback接口：定义了事件处理接口，所有事件管理者都必须实现这个接口来实现事件调度。
  
  ```cs
  public interface SSActionCallback
  {
      void SSActionCallback(SSAction source);
  }
  ```
  
  ------
  
  6. SSActionManager类(SSAction.cs里)：动作管理的基类，使用上述的移动方法，实现游戏对象与动作的绑定，确定回调函数消息的接收对象。
  
  ```cs
  //动作管理器
  public class SSActionManager : MonoBehaviour
  {
      private Dictionary<int, SSAction> actions = new Dictionary<int, SSAction>();        //正在进行
      private List<SSAction> waitingToAdd = new List<SSAction>();                         //等待添加
      private List<int> watingToDelete = new List<int>();                                 //等待销毁
  
      protected void Update()
      {
          //等待队列里的动作都添加到actions
          foreach (SSAction ac in waitingToAdd)
          {
              actions[ac.GetInstanceID()] = ac;
          }
          waitingToAdd.Clear();
          //获取每一个动作,检查是否完成
          foreach (KeyValuePair<int, SSAction> kv in actions)
          {
              SSAction ac = kv.Value;
              //动作的destroy属性为true是加入到销毁对列里
              if (ac.destroy)
              {
                  watingToDelete.Add(ac.GetInstanceID());
              }
              else if (ac.enable) 
              {
                  ac.Update();
              }
          }
          //对销毁队列的每一个动作
          foreach (int key in watingToDelete)
          {
              SSAction ac = actions[key];
              actions.Remove(key);
              Destroy(ac);
          }
          watingToDelete.Clear();
      }
  
      //添加动作,传入对象,动作,和回调函数
      public void addAction(GameObject gameObject, SSAction action, SSActionCallback ICallBack)
      {
          action.gameObject = gameObject;
          action.transform = gameObject.transform;
          action.CallBack = ICallBack;
          waitingToAdd.Add(action);
          action.Start();
      }
  }
  ```
  
  7. FirstSceneActionManager类：当前场景下的动作管理的具体实现，与场景控制基类配合，实现对当前场景的直接管理。挂载到图像中的Main空对象上实现对预制加载的场景的管理。
  
  ```cs
  //挂到空项目上,继承动作管理器和回调接口
  public class FirstSceneActionManager : SSActionManager, SSActionCallback
  {
      public SSActionEventType Complete = SSActionEventType.Completed;    //事件的状态是否完成
  
      //船移动
      public void BoatMove(BoatController Boat)
      {
          Complete = SSActionEventType.Started;
          CCMoveToAction action = CCMoveToAction.getAction(Boat.GetDestination(), Boat.GetMoveSpeed());
          addAction(Boat.GetGameObject(), action, this);  //见SSAcitonManager
          Boat.ChangeState(); //改变船的状态
      }
  
      //人物对象移动
      public void GameObjectsMove(GameObjects GameObject, Vector3 Destination)
      {
          Complete = SSActionEventType.Started;
          //防止撞墙,定义一个中点坐标
          Vector3 CurrentPos = GameObject.GetPosition();
          Vector3 MiddlePos = CurrentPos;
          if (Destination.y > CurrentPos.y)
          {
              MiddlePos.y = Destination.y;
          }
          else
          {
              MiddlePos.x = Destination.x;
          }
          //先移动到中间
          SSAction action1 = CCMoveToAction.getAction(MiddlePos, GameObject.GetMoveSpeed());
          //在移动到目的地
          SSAction action2 = CCMoveToAction.getAction(Destination, GameObject.GetMoveSpeed());
          //CCSequenceAction是SSAction的子类
          SSAction seqAction = CCSequenceAction.getAction(1, 0, new List<SSAction> { action1, action2 });
          //这个类继承了SSActionManager,管理动作
          this.addAction(GameObject.GetGameobject(), seqAction, this);
      }
  
      //回调函数SSActionEventType设置为Completed
      public void SSActionCallback(SSAction source)
      {
          Complete = SSActionEventType.Completed;
      }
  }
  ```
  
  8. FirstController类(挂到空项目上的cs文件)：实现使用动作分离的动作管理:
  
  ```cs
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
  ```
  
  其他函数改动不大

----

## 总结

实现动作分离，并添加裁判类，裁判类再FirstController里用到，用于检查游戏状态是否结束。