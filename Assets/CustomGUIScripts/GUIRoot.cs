using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class GUIRoot : MonoBehaviour
{

    //得到其子对象，然后放入数组中
    private ItemBase[] items;
    //依次调用每一个子对象的绘制方法
    void Start()
    {
        items = GetComponentsInChildren<ItemBase>();//在开始之前先得到所有子对象身上挂载的ItemBase脚本 
    }

    void Update()
    {
        
    }
    private void OnGUI()
    {
        //不在游戏运行的时候（也就是编辑的时候会不断运行）
        if (!Application.isPlaying)
        {
            items = GetComponentsInChildren<ItemBase>();//在开始之前先得到所有子对象身上挂载的ItemBase脚本
        }
        for (int i = 0; i < items.Length; i++)
        {
            items[i].Draw();
        }
    }
}
