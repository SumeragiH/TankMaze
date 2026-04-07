using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLosePanel : PanelBase<GameLosePanel>
{
    public Button btnBackGame;
    public Button btnBackTitle;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        btnBackGame.action += () =>
        {
            Time.timeScale = 1.0f;
            SceneManager.LoadScene("GameScene");
        };
        btnBackTitle.action += () =>
        {
            Time.timeScale = 1.0f;
            SceneManager.LoadScene("BeginScene");
        };
        this.HidePanel();

    }



}
