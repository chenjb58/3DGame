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