using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_Bullet
{
    Cannon,
    CannonHuge,
    MG,
    Plasma,
}
public class Bullet : MonoBehaviour
{
    public TankBase fatherTank;//创建子弹的对象，由武器告诉子弹
    public float MoveSpeed=10;
    public GameObject effectObj;
    public int BulletAtk;
    public E_Bullet e_Bullet;
    public void InitializeBullet()
    {
        //子弹伤害=父对象的基础伤害+子弹种类的伤害
        switch (e_Bullet)
        {
            case E_Bullet.Cannon:
                BulletAtk = fatherTank.atk + 20;
                break;
            case E_Bullet.CannonHuge:
                BulletAtk = fatherTank.atk - 20;
                break;
            case E_Bullet.MG:
                BulletAtk = fatherTank.atk-30;
                break;
            case E_Bullet.Plasma:
                BulletAtk = fatherTank.atk+30;
                break;
        }
    }

    void Update()
    {
        this.transform.Translate(Vector3.forward*MoveSpeed*Time.deltaTime);//子弹运动
    }
    private void OnTriggerEnter(Collider other)
    {
        // 先检查 fatherTank 是否为空
        if (fatherTank == null)
        {
            // 如果fatherTank为空，只检查墙壁和地面的碰撞
            if (other.CompareTag("Wall") || other.CompareTag("Ground"))
            {
                CreateEffectAndDestroy();
            }
            return;
        }

        // fatherTank不为空时，执行完整的碰撞检测
        if (other.CompareTag("Wall") || other.CompareTag("Ground") ||
            (fatherTank.CompareTag("Player") && other.CompareTag("Enemy")) ||
            (other.CompareTag("Player") && fatherTank.CompareTag("Enemy")))
        {
            CreateEffectAndDestroy();
        }
    }

    // 提取公共方法，避免代码重复
    private void CreateEffectAndDestroy()
    {
        if (effectObj != null)
        {
            //碰到物体的时候创建特效
            GameObject eff = GameObject.Instantiate(effectObj, this.transform.position, this.transform.rotation);
            //调整特效音量
            AudioSource a = eff.GetComponent<AudioSource>();
            a.mute = DateMgr.Instance.musicData.isEffect;
            a.volume = DateMgr.Instance.musicData.EffectNum;
            Destroy(eff, 0.7f);
        }
        Destroy(this.gameObject); //移除 
    }
}
