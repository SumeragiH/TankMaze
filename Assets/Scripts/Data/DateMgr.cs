using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class DateMgr //单例模式
{
    //不挂载，但是需要继承
    //需要继承用泛型约束，不用挂载使用new来创建对象
    private static DateMgr instance = new DateMgr();
    public static DateMgr Instance => instance;
    public MusicData musicData;
    public RankData rankData;
    private DateMgr()
    {
        // 先初始化，然后通过读取覆盖
        musicData = GameDate.Instance.LoadDate(typeof(MusicData), "music") as MusicData;
        if (musicData == null || musicData.isFirst)
        {
            musicData = new MusicData();
            musicData.isFirst = false;
            musicData.isEffect = false;
            musicData.isVoice = false;
            musicData.EffectNum = 50f;
            musicData.VoiceNum = 50f;
            GameDate.Instance.SaveDate(musicData, "music");
        }

        rankData = GameDate.Instance.LoadDate(typeof(RankData), "rank") as RankData;
        if (rankData == null)
        {
            Debug.Log("没有读取到数据");
            rankData = new RankData();
            rankData.isFirst = false;
            rankData.data = new List<rankItem>();
            rankData.nowMember = 0;

            // 初始化默认排名数据
            rankItem r;
            r.time = 3600;
            r.Name = "神秘";
            r.score = 0;

            for (int i = 0; i < 10; i++)
            {
                rankData.data.Add(r);
            }

            GameDate.Instance.SaveDate(rankData, "rank");
        }
        else if (rankData.data == null || rankData.data.Count == 0) // 添加 Count == 0 的检查
        {
            rankData.data = new List<rankItem>();

            // 添加默认数据
            rankItem r;
            r.time = 3600;
            r.Name = "神秘";
            r.score = 0;

            for (int i = 0; i < 10; i++)
            {
                rankData.data.Add(r);
            }

            // 保存初始化后的数据
            GameDate.Instance.SaveDate(rankData, "rank");
        }
    }
    public void ChangeMusicIsVoice(bool a)
    {
        musicData.isVoice = a;
        GameDate.Instance.SaveDate(musicData, "music");
    }
    public void ChangeMusicIsEffect(bool a)
    {
        musicData.isEffect = a;
        GameDate.Instance.SaveDate(musicData, "music");
    }
    public void ChangeMusicVoiceNum(float a )
    {
        musicData.VoiceNum = a;
        GameDate.Instance.SaveDate(musicData, "music");
    }
    public void ChangeMusicEffectNum(float a)
    {
        musicData.EffectNum = a;
        GameDate.Instance.SaveDate(musicData, "music");
    }


    public void SortRankData()
    {
        rankData.data.Sort(rankSort);
    }
    private int rankSort(rankItem a,rankItem b)
    {
        if (a.time < b.time)
            return -1;
        if(a.time>b.time)
            return 1;
        return 0;
    }
    public void SaveAllData()
    {
        GameDate.Instance.SaveDate(musicData, "music");
        GameDate.Instance.SaveDate(rankData, "rank");
    }

}
