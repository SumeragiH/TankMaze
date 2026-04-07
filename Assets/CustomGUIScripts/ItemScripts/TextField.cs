using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TextField : ItemBase
{
    public event UnityAction<string> action;

    private string oldText;
    protected override void DrawOffStyle()
    {
        itemCont.text = GUI.TextField(itemPos.ItemRect, itemCont.text);
        if (oldText != itemCont.text)
        {
            oldText = itemCont.text;
            action?.Invoke(itemCont.text);
        }
    }

    protected override void DrawOnStyle()
    {
        itemCont.text = GUI.TextField(itemPos.ItemRect, itemCont.text, itemStyle);
        if (oldText != itemCont.text)
        {
            oldText = itemCont.text;
            action?.Invoke(itemCont.text);
        }
    }
}
