using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingPanel : PanelBase<SettingPanel>
{
    public Button btnButton;
    public Button btnBack;
    public Toggle togVoice;
    public Toggle togEffect;
    public Slider sliVoice;
    public Slider sliEffect;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        btnBack.action += () => {
            if (SceneManager.GetActiveScene().name == "BeginScene")
            {
                BeginPanel.Instance.ShowPanel();
                SettingPanel.Instance.HidePanel();
            }
            if (SceneManager.GetActiveScene().name == "GameScene")
            {
                //时间流动
                Time.timeScale = 1;
                SettingPanel.Instance.HidePanel();
            }
           
        };//点击返回，关闭当前界面打开开始界面
        //点击选项，控制音乐与音效的开关
        togEffect.action += TogEffect;
        togVoice.action += TogVoice;
        //滑动条 ，控制音量的大小
        sliVoice.action +=SliVoice;
        sliEffect.action += SliEffect;
        SettingPanel.Instance.HidePanel();
    }

    // Update is called once per frame
    
    public void TogEffect(bool a)
    {

        DateMgr.Instance.ChangeMusicIsEffect(a);
    }
    public void TogVoice(bool a)
    {
        BeginBGM.Instance.ChangeMute(a);
        DateMgr.Instance.ChangeMusicIsVoice(a);
    }

    public void SliEffect(float a)
    {
        DateMgr.Instance.ChangeMusicEffectNum(a);
    }
    public void SliVoice(float a)
    {
        BeginBGM.Instance.ChangeCurrentNum(a/100f);
        DateMgr.Instance.ChangeMusicVoiceNum(a);
    }
    public override void ShowPanel()
    {
        //修改当前面板数据的直接在写，如果是跟存储的数据有关系的，使用DataMgr的API来修改，这样更加符合面向对象的编程思想
        base.ShowPanel();//默认子类调用的都是重写的，那么每次外部通过SettingPanel的类名点出来使用的时候，都是这个方法
        //每次调出这个界面的时候都读取，更新
        MusicData musicData = GameDate.Instance.LoadDate(typeof(MusicData), "music") as MusicData;//直接读取保存的代码
        togEffect.isSel = musicData.isEffect;
        togVoice.isSel = musicData.isVoice;
        sliEffect.currentNum = musicData.EffectNum;
        sliVoice.currentNum = musicData.VoiceNum;
        //每帧GUI都会绘制，而其中的数据在这里赋值

    }

    public override void HidePanel()
    {
        DateMgr.Instance.SaveAllData();
        base.HidePanel();
    }
    
}
