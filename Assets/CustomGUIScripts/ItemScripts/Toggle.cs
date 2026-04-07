using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Toggle : ItemBase
{
    public bool isSel = false;
    private bool oldSel = false;
    public event UnityAction<bool> action;//事件监听，当满足某种条件的时候，就会执行

    public void ChangeSel(bool value)
    {
        isSel = value;
        oldSel = value; // 同时更新oldSel，避免误触发状态变化检测
    }

    protected override void DrawOffStyle()
    {
        isSel = GUI.Toggle(itemPos.ItemRect, isSel, itemCont);
        if (isSel != oldSel)
        { 
            action?.Invoke(isSel);
            oldSel = isSel;
        }
    }

    protected override void DrawOnStyle()
    {
        isSel = GUI.Toggle(itemPos.ItemRect, isSel, itemCont, itemStyle);
        if (isSel != oldSel) 
        { 
            action?.Invoke(isSel);
            oldSel = isSel;
        }
    }
}