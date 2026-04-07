using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginBGM : MonoBehaviour
{
    private static BeginBGM instance;
    private BeginBGM() { }
    public static BeginBGM Instance => instance;

    private AudioSource beginBGM;
    private void Awake()
    {
        instance = this;//外界得到这个控制单例
        beginBGM = this.GetComponent<AudioSource>();//这个单例要控制的脚本
        //一开始就设置音量
        beginBGM.mute = DateMgr.Instance.musicData.isVoice;
        beginBGM.volume = DateMgr.Instance.musicData.VoiceNum/100f;
    }
    public void ChangeMute(bool a)//传入0~1，存储的是0~100
    {
        beginBGM.mute = a;
    }
    public void ChangeCurrentNum(float a)
    {
        beginBGM.volume = a;
    }
}
