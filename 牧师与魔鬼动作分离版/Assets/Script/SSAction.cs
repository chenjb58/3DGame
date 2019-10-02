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