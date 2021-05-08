using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMap : MonoBehaviour
{
    GameObject mapSprite;

    private void OnEnable()
    {
        //获得这个房间的围墙 是此房间的父级的第一个子对象，并且渲染设为false
        mapSprite = transform.parent.GetChild(0).gameObject;

        mapSprite.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //如果玩家触碰，将渲染设为true
            mapSprite.SetActive(true);
        }

    }
}
   
