using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    public GameObject bullet;
    public GameObject weapon;
    //获取武器的发射位置
    public ShootPoint[] shootPoints;
    public TankBase Tank;//武器的拥有者
    public float gapTime;
    private float lastFireTime = 0f; // 初始化为0，确保第一次可以立即开火

    // Start is called before the first frame update
    void Start()
    {
        //一开始的时候获得射击点的位置
        //当获得武器的时候也调用获得射击点的方法
        GetShootPoints();
    }

    public void Fire()
    {
        if (weapon == null)
        {
            return;
        }
        if (Time.timeScale==0)
        {
            return ;//暂停的时候没法开枪
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
                    script.fatherTank = Tank;
                    script.InitializeBullet();
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
    public void GetShootPoints()
    {
        if (weapon!=null)
        {
            shootPoints = weapon.GetComponentsInChildren<ShootPoint>();
            //得到开火间隔
            if (weapon.CompareTag("MG"))
            {
                gapTime = 0.15f;
            }
            else
            {
                gapTime = 0.6f;
            }
        }
        
    }
    public void ChangeWeapon(GameObject w,GameObject b)
    {
        //把之前的移除
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        weapon = GameObject.Instantiate(w,this.transform.position,this.transform.rotation);
        weapon.transform.SetParent(this.transform);
        bullet = b;
        GetShootPoints();
        lastFireTime = 0f;
    }
}
