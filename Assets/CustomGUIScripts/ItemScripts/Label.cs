using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Label : ItemBase
{
    protected override void DrawOnStyle()
    {
        GUI.Label(itemPos.ItemRect,itemCont,itemStyle);
    }

    protected override void DrawOffStyle()
    {
        GUI.Label(itemPos.ItemRect, itemCont);
    }

   
}
