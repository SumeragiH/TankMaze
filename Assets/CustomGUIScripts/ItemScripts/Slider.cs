using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
public enum E_SliderType
{
    Horizontal,
    Vertical,
}
public class Slider : ItemBase
{
    public float maxNum=100;
    public float minNum=0;
    public float currentNum;
    public E_SliderType E_sliderType;
    public GUIStyle itemThumb;
    public event UnityAction<float> action;
    private float oldNum;
    protected override void DrawOffStyle()
    {
        switch(E_sliderType)
        {
            case E_SliderType.Horizontal:
                currentNum = GUI.HorizontalSlider(itemPos.ItemRect, currentNum, minNum, maxNum);
                break; 
            case E_SliderType.Vertical:
                currentNum = GUI.VerticalSlider(itemPos.ItemRect, currentNum, minNum, maxNum);
                break;
        }
        if (currentNum != oldNum)
        {
            oldNum = currentNum;
            action?.Invoke(currentNum);
        }
        
    }

    protected override void DrawOnStyle()
    {
        switch (E_sliderType)
        {
            case E_SliderType.Horizontal:
                currentNum = GUI.HorizontalSlider(itemPos.ItemRect, currentNum, minNum, maxNum,itemStyle,itemThumb);

                break;
            case E_SliderType.Vertical:
                currentNum = GUI.VerticalSlider(itemPos.ItemRect, currentNum, minNum, maxNum,itemStyle, itemThumb);
                break;
        }
        if (currentNum != oldNum)
        {
            oldNum = currentNum;
            action?.Invoke(currentNum);
        }
    }
}
