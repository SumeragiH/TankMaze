using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawTexture : ItemBase
{
    public Texture texture; 
    public ScaleMode scaleMode=ScaleMode.StretchToFill;
    public bool alphaBlend;

    protected override void DrawOffStyle()
    {
        GUI.DrawTexture(itemPos.ItemRect, texture, scaleMode,alphaBlend);
    }

    protected override void DrawOnStyle()
    {
        GUI.DrawTexture(itemPos.ItemRect, texture, scaleMode, alphaBlend);
    }
}
