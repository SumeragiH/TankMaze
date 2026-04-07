using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum E_RewardItem
{
    HP,
    MaxHP,
    Def,
    Atk
}
public class RewardItem : MonoBehaviour
{
    public E_RewardItem e_rewardItem;
    private PlayerTank playerTank;
    public int UpHP=200;
    public int UpMaxHP=50;
    public int UpDef;
    public int UpAtk;
    public int Score=25;
    public GameObject effect;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))//只有玩家碰撞才会进行交互
        {
            //触发奖励的音效和特效
            GameObject eff = GameObject.Instantiate(effect, this.transform.position, this.transform.rotation);
            //音效的大小取决于设置
            AudioSource audioSource = eff.GetComponent<AudioSource>();
            audioSource.mute = DateMgr.Instance.musicData.isEffect;
            audioSource.volume = DateMgr.Instance.musicData.EffectNum;
            Destroy(eff, 0.7f);

            //得到碰撞的玩家脚本
            //调用玩家脚本的API
            playerTank = other.GetComponent<PlayerTank>();
            //得分
            GameSceneUI.Instance.AddScore(Score);
            switch (e_rewardItem)
            {
                case E_RewardItem.HP:
                    playerTank.ChangeHP(UpHP);
                    break;
                case E_RewardItem.MaxHP:
                    playerTank.ChangeMaxHP(UpMaxHP);
                    playerTank.ChangeHP(25);
                    break;
                case E_RewardItem.Def:
                    playerTank.ChangeDef(UpDef);
                    break;
                case E_RewardItem.Atk:
                    playerTank.ChangeAtk(UpAtk);
                    break;
                default:
                    break;
            }
            Destroy(this.gameObject);
        }
    }

}
