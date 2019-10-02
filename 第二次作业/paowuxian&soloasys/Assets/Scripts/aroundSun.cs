using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aroundSun : MonoBehaviour
{

    public Transform sun;
    public Transform moon;
    public Transform mercury;//水星
    public Transform venus;//金星
    public Transform earth;//地球
    public Transform mars;//火星
    public Transform jupiter;//木星
    public Transform saturn;//土星
    public Transform uranus;//天王星
    public Transform neptune;//海王星

    // Use this for initialization
    void Start()
    {
        sun.position = Vector3.zero;
        mercury.position = new Vector3(1, 0, 0);
        venus.position = new Vector3(2, 1, 0);
        earth.position = new Vector3(4, 2, 0);
        moon.position = new Vector3(3, 2, 0);
        mars.position = new Vector3(6, -2, 0);
        jupiter.position = new Vector3(7, -1, 0);
        saturn.position = new Vector3(8, -3, 0);
        uranus.position = new Vector3(9, 0, 0);
        neptune.position = new Vector3(10, 1, 1);
        mercury.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        venus.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        earth.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        moon.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        mars.localScale = new Vector3(0.15f, 0.15f, 0.15f);
        jupiter.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        saturn.localScale = new Vector3(0.6f, 0.6f, 0.6f);
        uranus.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        neptune.localScale = new Vector3(0.2f, 0.2f, 0.2f);
    }

    // Update is called once per frame
    void Update()
    {
        //different speed, 要选择跟上面位置正交的向量才能达到围着太阳转的感觉，分别是公转跟自转
        mercury.RotateAround(sun.position, new Vector3(0, 1, 1), 20 * Time.deltaTime);
        mercury.Rotate(new Vector3(0, 1, 1) * 5 * Time.deltaTime);

        venus.RotateAround(sun.position, new Vector3(0, 0, 1), 15 * Time.deltaTime);
        venus.Rotate(new Vector3(0, 0, 1) * Time.deltaTime);

        earth.RotateAround(sun.position, Vector3.forward, 10 * Time.deltaTime);
        earth.Rotate(Vector3.forward * 30 * Time.deltaTime);
        moon.transform.RotateAround(earth.position, Vector3.forward, 359 * Time.deltaTime);

        mars.RotateAround(sun.position, new Vector3(1, 3, 0), 9 * Time.deltaTime);
        mars.Rotate(new Vector3(1, 3, 0) * 40 * Time.deltaTime);

        jupiter.RotateAround(sun.position, new Vector3(1, 7, 0), 8 * Time.deltaTime);
        jupiter.Rotate(new Vector3(1, 7, 0) * 30 * Time.deltaTime);

        saturn.RotateAround(sun.position, new Vector3(0, 0, 1), 7 * Time.deltaTime);
        saturn.Rotate(new Vector3(0, 0, 1) * 20 * Time.deltaTime);

        uranus.RotateAround(sun.position, new Vector3(0, 2, 1), 6 * Time.deltaTime);
        uranus.Rotate(new Vector3(0, 2, 1) * 20 * Time.deltaTime);

        neptune.RotateAround(sun.position, new Vector3(0, 1, -1), 5 * Time.deltaTime);
        neptune.Rotate(new Vector3(0, 1, 1) * 30 * Time.deltaTime);
    }
}
