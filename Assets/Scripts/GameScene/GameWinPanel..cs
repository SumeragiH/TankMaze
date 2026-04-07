using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameWinPanel : PanelBase<GameWinPanel>
{
    public Button btnApply;
    public TextField tfName;
    // Start is called before the first frame update
   new void Start()
    {
        base.Start();
        btnApply.action += () => 
        {
            Time.timeScale = 1.0f;
            rankItem r = new rankItem();
            r.time =(int)GameSceneUI.Instance.nowTime;
            r.score = (int)(GameSceneUI.Instance.nowScore);
            r.Name = string.IsNullOrEmpty(tfName.itemCont.text) ? "匿名玩家" : tfName.itemCont.text;
            if (DateMgr.Instance.rankData.data == null)
            {
                Debug.LogError("rankData.data 为 null，初始化列表");
            }
            if (DateMgr.Instance.rankData.nowMember < 10)
            {
                DateMgr.Instance.rankData.data[DateMgr.Instance.rankData.nowMember] = r;
                DateMgr.Instance.rankData.nowMember += 1;
            }
            else//添加数据，然后排序，然后删除10以后的数据，然后存储
            {
                DateMgr.Instance.rankData.data.Add(r);
                DateMgr.Instance.SortRankData();
                DateMgr.Instance.rankData.data.RemoveRange(10, DateMgr.Instance.rankData.data.Count - 10);
            }

                DateMgr.Instance.SaveAllData();
            SceneManager.LoadScene("BeginScene");
        };
        this.HidePanel();
    }

}
