using System.Collections.Generic;
using Unity.VisualScripting;

[System.Serializable]
public class RankData
{
    public bool isFirst;
    public List<rankItem> data;
    public int nowMember=0;

    // 添加构造函数确保data被初始化
    public RankData()
    {
        data = new List<rankItem>();
    }
    //确保RankData有无参构造函数，能够构造RankData，并且保证data也能够初始化
}

[System.Serializable]
public struct rankItem
{
    public int time;
    public string Name;
    public int score;
}
