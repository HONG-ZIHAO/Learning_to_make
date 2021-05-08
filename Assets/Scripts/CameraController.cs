using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    private void Awake()
    {
        instance = this;
    }

    public float speed;

    public Transform target;
   

    void Update()
    {
        //movetowards这个函数是向某方向移动固定距离的函数，在这里用来控制摄像机的移动
        if(target!=null)
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, target.position.y, transform.position.z),speed * Time.deltaTime);
    }
    /// <summary>
    /// 将从Room代码中接收到的新房间的Transform信息赋值给target变量中去
    /// </summary>
    /// <param name="newTarget">获得的Transform信息</param>
    public void ChangeTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
