using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : ItemBase
{
    public event UnityAction action;
    protected override void DrawOffStyle()
    {
        if (GUI.Button(itemPos.ItemRect, itemCont))
        {
            action?.Invoke();
        }
    }

    protected override void DrawOnStyle()
    {
        if (GUI.Button(itemPos.ItemRect, itemCont, itemStyle))
        { 
            action?.Invoke();
        }

    }
}
