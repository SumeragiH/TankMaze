using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyTower : TankBase
{
    public GameObject bullet;
    public ShootPoint[] shootPoints;
    private float lastFireTime = 0f;
    public TankBase tank;
    public float gapTime;
    private Bullet temp;
    public int minBulletSpeed;
    public int maxBulletSpeed;
    public TankBase player;
    private void Start()
    {
       
        tank = this;
        shootPoints = this.gameObject.GetComponentsInChildren<ShootPoint>();
           
    }
    private void Update()
    {
        if (Vector3.Distance(this.transform.position, player.transform.position) < 16)
        {
            Fire();
        }  
    }
    public override void Fire()
    {
        if (Time.timeScale == 0)
        {
            return;//暂停的时候没法开枪
        }
        if (Time.time - lastFireTime < gapTime)
        {
            return; // 未到间隔，不执行开火
        }
        else
        {
            if (shootPoints != null)//防止没有武器的时候开枪报错
            {
                for (int i = 0; i < shootPoints.Length; i++)
                {
                    GameObject b = GameObject.Instantiate(bullet, shootPoints[i].transform.position, shootPoints[i].transform.rotation);
                    //控制子弹做什么
                    Bullet script = b.GetComponent<Bullet>();
                    script.fatherTank = tank;
                    script.InitializeBullet();
                    //子弹速度随机
                    int j = UnityEngine.Random.Range(minBulletSpeed, maxBulletSpeed);
                    script.MoveSpeed = j;
                    //得到枪口的开火音效，然后播放,音效大小来自于设置
                    AudioSource shootVoice = shootPoints[i].GetComponent<AudioSource>();
                    shootVoice.mute = DateMgr.Instance.musicData.isEffect;
                    shootVoice.volume = DateMgr.Instance.musicData.EffectNum;
                    shootVoice.Play();
                    Destroy(b, 10f);
                }

            }
        }
        lastFireTime = Time.time;
    }
    public override void Injured(TankBase other)
    {
        //不会受伤
    }
    public override void Dead()
    {
        //不会死亡
    }

}
