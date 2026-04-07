using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneEndPanel : PanelBase<GameSceneEndPanel>
{
    public Button btnApply;
    public Button btnCancel;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        btnApply.action += () =>
        {
            Application.Quit();
        };
        btnCancel.action += () => 
        {
            Time.timeScale = 1;
            GameSceneEndPanel.Instance.HidePanel();
        };
        GameSceneEndPanel.Instance.HidePanel();

    }

   
}
