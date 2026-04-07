using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleMgr : MonoBehaviour
{
    //得到要控制的Toggle
    public Toggle[] toggles;
    private Toggle frontToggle;

    private void Start()
    {
        SetDefault();

        //记录上一次选择为true的toggle
        for (int i = 0; i < toggles.Length; i++)
        {
            Toggle toggle = toggles[i];

            //遍历toggle，为Toggle添加事件监听
            toggle.action += (value) =>
            {
                if (value)//如果isSel被选中
                {
                    //修改其他的选项
                    for (int j = 0; j < toggles.Length; j++)
                    {
                        if (toggles[j] != toggle)
                        {
                            toggles[j].ChangeSel(false);
                        }
                    }
                    frontToggle = toggle;
                }
                else if (frontToggle == toggle)//没选中，判断这个没选中的是不是上一次选中的
                {
                    toggle.ChangeSel(true);
                }
            };
        }
    }

    // 确保至少有一个toggle被选中
    private void SetDefault()
    {
        bool anySelected = false;
        foreach (Toggle toggle in toggles)
        {
            if (toggle.isSel)
            {
                anySelected = true;
                frontToggle = toggle;
                break;
            }
        }

        // 如果没有被选中的，选中第一个
        if (!anySelected && toggles.Length > 0)
        {
            toggles[0].ChangeSel(true);
            frontToggle = toggles[0];
        }
    }
}