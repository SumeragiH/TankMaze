using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
public enum E_WindowPos
{
    center,
    up,
    down, 
    left, 
    right,
    leftUp,
    rightUp,
    leftDown,
    rightDown,
}//九宫格九个方向
public enum E_CenterPos
{
    center,
    up,
    down,
    left,
    right,
    leftUp,
    rightUp,
    leftDown,
    rightDown,
}
[System.Serializable]
public class CustomPosBase
{
    //组件可以选择在哪个九宫格
    //计算得到九宫格
    public E_WindowPos E_windowPos=E_WindowPos.center;
    
    

    //选择组件的中心点
    //计算得到组件的中心点位置
    public E_CenterPos E_centerPos=E_CenterPos.center;
    //组件本身的大小
    public float itemWidth = 100;
    public float itemHeight = 50;

    private Rect itemRect;//组件最后得到的坐标
    public Rect ItemRect//只能得到不能修改
    { 
        get 
        {
            //动态获取屏幕的宽高
            float w = Screen.width;
            float h = Screen.height;
            //xy轴的数据
            itemRect.x = WindowPosCulc(w,h).x+CenterPosCulc().x+pos.x;
            itemRect.y = WindowPosCulc(w,h).y+CenterPosCulc().y+pos.y;
            //宽和高的数据
            itemRect.width = itemWidth;
            itemRect.height = itemHeight;

            return itemRect; 
        }
    }
   

    //偏移位置
    public Vector2 pos;
    private Rect WindowPosCulc(float w,float h)
    {
        Rect rect = new Rect();
        switch (E_windowPos)
        {
            case E_WindowPos.center:
                rect.x = w / 2;
                rect.y = h / 2;
                break;
            case E_WindowPos.up:
                rect.x = w / 2;
                rect.y = 0;
                break;
            case E_WindowPos.down:
                rect.x = w / 2;
                rect.y = h;
                break;
            case E_WindowPos.left:
                rect.x = 0;
                rect.y = h / 2;
                break;
            case E_WindowPos.right:
                rect.x = w;
                rect.y = h/2;
                break;
            case E_WindowPos.leftUp:
                rect.x = 0;
                rect.y = 0;
                break;
            case E_WindowPos.rightUp:
                rect.x = w;
                rect.y = 0;
                break;
            case E_WindowPos.leftDown:
                rect.x = 0;
                rect.y = h;
                break;
            case E_WindowPos.rightDown:
                rect.x = w;
                rect.y = h;
                break;
            default:
                break;
        }
        return rect;
    }//传入选择的九宫格位置，返回屏幕九宫格原点
    private Rect CenterPosCulc()//传入选择的空间位置，得到其中心点的坐标
    {
        Rect rect = new Rect();
        switch (E_centerPos)
        {
            case E_CenterPos.center:
                rect.x = -itemWidth/2;
                rect.y = -itemHeight/2;
                break;
            case E_CenterPos.up:
                rect.x = -itemWidth/2;
                rect.y = 0;
                break;
            case E_CenterPos.down:
                rect.x = -itemWidth/2;
                rect.y = -itemHeight;
                break;
            case E_CenterPos.left:
                rect.x = 0;
                rect.y=-itemHeight/2;
                break;
            case E_CenterPos.right:
                rect.x = -itemWidth;
                rect.y = -itemHeight /2;
                break;
            case E_CenterPos.leftUp:
                rect.x = 0;
                rect.y = 0;
                break;
            case E_CenterPos.rightUp:
                rect.x = -itemWidth;
                rect.y = 0;
                break;
            case E_CenterPos.leftDown:
                rect.x = 0;
                rect.y = -itemHeight;
                break;
            case E_CenterPos.rightDown:
                rect.x = -itemWidth;
                rect.y = -itemHeight;
                break;
            default:
                break;
        }
        return rect;
    }


}
