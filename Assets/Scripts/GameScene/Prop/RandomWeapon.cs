using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomWeapon: MonoBehaviour
{
    public GameObject[] RandomWeapons;
    public GameObject[] bullets;
    public AudioSource audioSource;
    public GameObject effect;
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            //更换坦克的武器 
            PlayerTank playerTank = other.GetComponent<PlayerTank>();
            //随机一个武器给玩家
            int i = Random.Range(0,RandomWeapons.Length);
            playerTank.ChangeWeapon(RandomWeapons[i], bullets[i]);
            //播放奖励的音效
            if (audioSource != null)
                audioSource.mute = DateMgr.Instance.musicData.isEffect;
                audioSource.volume = DateMgr.Instance.musicData.EffectNum;
                audioSource.Play();
            //播放奖励的特效
            if (effect != null)
            {
                GameObject e = GameObject.Instantiate(effect, this.transform.position, this.transform.rotation);
                Destroy(e, 1.5f);
            }
                
            //清除自己
            Destroy(this.gameObject);
        }

    }
}
