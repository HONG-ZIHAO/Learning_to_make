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
        //movetowards�����������ĳ�����ƶ��̶�����ĺ���������������������������ƶ�
        if(target!=null)
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, target.position.y, transform.position.z),speed * Time.deltaTime);
    }
    /// <summary>
    /// ����Room�����н��յ����·����Transform��Ϣ��ֵ��target������ȥ
    /// </summary>
    /// <param name="newTarget">��õ�Transform��Ϣ</param>
    public void ChangeTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
