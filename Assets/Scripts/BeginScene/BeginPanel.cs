using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BeginPanel : PanelBase<BeginPanel>
{
    public Button btnSetting;
    public Button btnBegin;
    public Button btnEnd;
    public Button btnRank;

    new void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        base.Start();
        btnBegin.action += Load;//点击开始按钮切换场景
        btnEnd.action += () => {
            EndPanel.Instance.ShowPanel();
            BeginPanel.Instance.HidePanel();
        };//点击结束，使得结束面板激活，此面板失活
        btnSetting.action += () => {

            SettingPanel.Instance.ShowPanel();
            BeginPanel.Instance.HidePanel();
        };//点击设置，使得设置面板激活，此面板失活
        btnRank.action += () => {
            RankPanel.Instance.ShowPanel();
            BeginPanel.Instance.HidePanel();
        };

    }

    private void Load()
    {
        SceneManager.LoadScene("GameScene");
    }
}
