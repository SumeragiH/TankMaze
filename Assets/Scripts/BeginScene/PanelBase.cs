using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelBase<T> : MonoBehaviour where T:class
{
    //所有继承这个类的，都可以调用其中的方法，使得面板激活与失活

    //单例模式 
    private static T instance;
    public static T Instance => instance;

    protected void Start()
    {
        instance = this as T;
    }


    public virtual void ShowPanel()
    {
        this.gameObject.SetActive(true);
    }

    public virtual void HidePanel()
    {
        this.gameObject.SetActive(false);
    }
}
