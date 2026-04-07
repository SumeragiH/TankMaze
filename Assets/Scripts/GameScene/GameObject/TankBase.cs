using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TankBase :MonoBehaviour
{
    public int atk;
    public int def;
    public int maxHp;
    public int currentHp;
    public float moveSpeed;
    public float bodyRotateSpeed;
    public float headRotateSpeed;
    public float PkRotateSpeed;

    public GameObject deadEffect;

    public abstract void Fire();
    /// <summary>
    /// 播放死亡的特效和音效后移除物体
    /// </summary>
    public virtual void Dead()
    {
        if (deadEffect != null)
        {
            GameObject effect = Instantiate(deadEffect, this.transform.position, this.transform.rotation);
            AudioSource audioSource = effect.GetComponent<AudioSource>();
            audioSource.volume = DateMgr.Instance.musicData.EffectNum;
            audioSource.mute = DateMgr.Instance.musicData.isEffect;
            Destroy(effect,1.3f);
        }
        Destroy(this.gameObject);
    }

    public virtual void Injured(TankBase other)
    {
        if (other.atk - def >= 0)
        {
            currentHp = currentHp - (other.atk - def);
        }
        if (currentHp <= 0)
        {
            currentHp = 0;
            this.Dead();
        }
    }
}
