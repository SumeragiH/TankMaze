using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public enum E_IsStyle
{ 
    On,
    Off,
}
public abstract class ItemBase:MonoBehaviour
{
    public CustomPosBase itemPos;
    public GUIContent itemCont;
    public GUIStyle itemStyle;
    [Header(" «∑Ò∆Ù”√Style")]
    public E_IsStyle isStyle;

    public void Draw()
    {
        switch (isStyle)
        {
            case E_IsStyle.On:
                DrawOnStyle();
                break;
            case E_IsStyle.Off:
                DrawOffStyle();
                break;

        }   
    }
    protected abstract void DrawOffStyle();
    protected abstract void DrawOnStyle();
}
