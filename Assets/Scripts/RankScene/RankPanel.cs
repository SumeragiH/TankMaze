using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using Unity.VisualScripting;
using UnityEngine;

public class RankPanel : PanelBase<RankPanel>
{
    public Button btnBack;
    //这里是控制的面板
    //面板里的数据在RankData脚本中
    private List<Label> Ranks=new List<Label>();
    private List<Label> Names=new List<Label>();
    private List<Label> Scores=new List<Label>();
    private List<Label> Times = new List<Label>();
    
    new void Start()
    {
        base.Start();//使得这个Rank成为一个单例模式
        //如果是第一次，那么添加Rank的数据
        //之后都是读取和修改
        for (int i = 1; i < 11; i++)
        {
            //存储要控制的面板
            Ranks.Add(this.transform.Find("Rank/Rank"+i).GetComponent<Label>());
            Names.Add(this.transform.Find("Name/Name (" + i + ")").GetComponent<Label>());
            Scores.Add(this.transform.Find("Score/Score (" + i + ")").GetComponent<Label>());
            Times.Add(this.transform.Find("Time/Time (" + i + ")").GetComponent<Label>());
        }
        btnBack.action += () =>
        {
            HidePanel();
            BeginPanel.Instance.ShowPanel();
        };
        HidePanel();
    }

    public override void ShowPanel()//按照读取的数据进行更新
    {
        //读取
        RankData rank=DateMgr.Instance.rankData;
        int NowMembers = DateMgr.Instance.rankData.nowMember;
        //排序
        DateMgr.Instance.SortRankData();
        //赋值
        for (int i = 0; i < 10; i++)
        {
            //Rank不动
            //其他的进行赋值操作
            Names[i].itemCont.text=rank.data[i].Name;
            Scores[i].itemCont.text = rank.data[i].score.ToString();
            if (rank.data[i].time>=3600)
            {
                Times[i].itemCont.text = (rank.data[i].time/3600).ToString()+"时"+((rank.data[i].time % 3600)/60).ToString()+"分"+ ((rank.data[i].time % 3600) % 60).ToString()+"秒";
            }
            if (rank.data[i].time >= 60 && rank.data[i].time < 3600)
            {
                Times[i].itemCont.text = (rank.data[i].time / 60).ToString() + "分" + (rank.data[i].time%60).ToString()+"秒";
            }
            if (rank.data[i].time >= 0 && rank.data[i].time < 60)
            {
                Times[i].itemCont.text = (rank.data[i].time).ToString() + "秒";
            }

        }
        //打印
        base.ShowPanel();
    }
    public override void HidePanel() 
    {
        base.HidePanel();
        DateMgr.Instance.SaveAllData() ;//每次退出都存储数据
    }

    
}
