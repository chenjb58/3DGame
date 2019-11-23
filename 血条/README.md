## 血条



### IMGUI

用HorizontalScollabel显示血条即可

```csharp
using UnityEngine;

public class IMGUI : MonoBehaviour
{
    // 当前血量
    private float currentFlood = 0.5f;
    // 增/减后血量
    private float clickFlood;
    // 点击1次增加/减少的血量
    public float oneClickFlood;

    private Rect floodBar;
    private Rect addFlood;
    private Rect substractFlood;

    void Start()
    {
        //血条区域
        floodBar = new Rect(50, 50, 200, 20);
        //加血按钮区域  
        addFlood = new Rect(105, 80, 40, 20);
        //减血按钮区域
        substractFlood = new Rect(155, 80, 40, 20);
        clickFlood = currentFlood;
        oneClickFlood = 0.1f;
    }

    void OnGUI()
    {
        if (GUI.Button(addFlood, "加血"))
        {
            clickFlood = clickFlood + oneClickFlood > 1.0f ? 1.0f : clickFlood + oneClickFlood;
        }
        if (GUI.Button(substractFlood, "减血"))
        {
            clickFlood = clickFlood - oneClickFlood < 0.0f ? 0.0f : clickFlood - oneClickFlood;
        }

        //插值计算currentFlood值，以实现血条值平滑变化
        currentFlood = Mathf.Lerp(currentFlood, clickFlood, 0.05f);

        // 用水平滚动条的宽度作为血条的显示值
        GUI.HorizontalScrollbar(floodBar, 0.0f, currentFlood, 0.0f, 1.0f);
    }
}
```

效果

![image-20191123160731536](%E8%A1%80%E6%9D%A1.assets/image-20191123160731536.png)

点击加血减血就可以改变滑动条的进度。加减量可改变oneClickBlood的值。

### UGUI

使用两个Canvas分别作为外框和血量。

![image-20191123155610728](C:%5CUsers%5C51054%5CAppData%5CRoaming%5CTypora%5Ctypora-user-images%5Cimage-20191123155610728.png)

如果要使血条一直跟随主相机不要随人物转动而转动需要添加以下脚本

```csharp
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    void Update()
    {
        this.transform.LookAt(Camera.main.transform.position);
    }
}

```

### 两种实现的优缺点

- **IMGUI**

  - **优点**

    比较灵活，给游戏编程传统， 在修改模型，渲染模型这样的经典游戏循环编程模式中，在渲染阶段之后，绘制 UI 界面无可挑剔 ， 既避免了 UI 元素保持在屏幕最前端，又有最佳的执行效率 。

  - **缺点**

    效率较低、难以调试等。

- **UGUI**

  - **优点**

     UGUI是一种所见即所得的设计工具，代码开发门槛较低，并且支持多模式、多摄像机渲染，UI 元素与游戏场景融为一体的交互，是一种面向对象的编程模式 

  - **缺点**

    灵活性降低

### 预设的使用方法

将完成好的对象拖进Asset文件夹

![image-20191123160300095](%E8%A1%80%E6%9D%A1.assets/image-20191123160300095.png)

之后用以下语句就可在脚本中使用预制

```csharp
OBJ_NAME obj = Instantiate(Resources.Load("Prefabs/PREFAB_NAME", typeof(OBJ_NAME)), Vector3.zero, Quaternion.identity, null) as OBJ_NAME;
```

OBJ_NAME、 PREFAB_NAME指代对象名和预制名