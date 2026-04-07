using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPanel : PanelBase<EndPanel>
{
    public Button btnYes;
    public Button btnNo;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        btnYes.action += () => 
        {
            Application.Quit();      
        };
        btnNo.action += () =>//Ñ¡ÔñÈ¡Ïû
        {
            BeginPanel.Instance.ShowPanel();
            EndPanel.Instance.HidePanel();
        };
        EndPanel.Instance.HidePanel() ;
    }

   
}
