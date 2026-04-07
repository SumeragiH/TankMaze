using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneUI :PanelBase<GameSceneUI>
{
    public Label labScoreCount;
    public Label labTimeCount;
    public DrawTexture texHPCountFr;
    public DrawTexture texHPCountBk;
    public DrawTexture Map;
    public Button btnSet;
    public Button btnBack;

    public int HPWidth;
    [HideInInspector]
    public int nowScore = 0;
    [HideInInspector]
    public float nowTime = 0;
    private int time;//游戏最后数据存储的是int秒
    // Start is called before the first frame update
    new void Start()
    {
        Time.timeScale = 1.0f;
        base.Start();
        btnBack.action += () =>
        {
            Time.timeScale = 0;
            //打开暂停界面，是否退出
            GameSceneEndPanel.Instance.ShowPanel();
            
        };
        btnSet.action += () =>
        {
            Time.timeScale = 0;
            //打开设置界面
            SettingPanel.Instance.ShowPanel();
        };

    }

    // Update is called once per frame
    void Update()
    {
        nowTime += Time.deltaTime;
        time = (int)nowTime;
        //时间换算
        if (time >= 3600)//小时
        {
            labTimeCount.itemCont.text = (time / 3600).ToString() + "时" + ((time % 3600) / 60).ToString() + "分" + ((time % 3600) % 60).ToString() + "秒";
        }
        if (time >= 60 && time < 3600)//分
        {
            labTimeCount.itemCont.text = (time / 60).ToString() + "分" + (time % 60).ToString() + "秒";
        }
        if (time >= 0 && time < 60)
        {
            labTimeCount.itemCont.text = time.ToString() + "秒";
        }
    }
    public void AddScore(int score)
    {
        nowScore += score;
        labScoreCount.itemCont.text = nowScore.ToString();
       
    }
    public void ChangeHp(int maxHP,int currentHp)
    {
        texHPCountFr.itemPos.itemWidth = ((float)currentHp /(float)maxHP) * (float)HPWidth;
    }
    public void UpMaxHP(int upMaxHP,int currentMaxHp)
    {
        //调整最大生命值的UI,长度一起修改
        //最大生命值一次上升50，回血25
        texHPCountBk.itemPos.itemWidth = (float)upMaxHP+(float)currentMaxHp;
        HPWidth= upMaxHP+currentMaxHp;//总血量长度变长

    }
    
}
