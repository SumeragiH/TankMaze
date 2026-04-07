using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
public enum E_TankMode
{
    alert,//巡逻
    fight,//
    escape,
}
public class EnemyTank : TankBase
{
    public E_TankMode mode = E_TankMode.alert;
    public GameObject healthEffect;
    public TankBase Player;
    public GameObject[] props;
    public int posible=70;
    [Header("射击相关")]//子弹弹速慢，但是没有下坠，伤害低
    public GameObject Head;
    public GameObject bullet;
    public ShootPoint[] shootPoints;
    public float AimDistance;
    public float FightDistance;
    public float BackDistance;
    public float gapTime;
    private float lastFireTime = 0f; // 初始化为0，确保第一次可以立即开火
    [Header("移动相关")]
    public GameObject[] movePoints;
    public GameObject targetPoint;
    public GameObject Body;
    public float lastMoveTime;
    [Header("防止卡死的间隔检测时间")]
    public float moveCheckTime;//
    [Header("血条")]
    public Texture HPFr;
    public Texture HPBk;
    private Rect HPFrRect;
    private Rect HPBkRect;
    public ScaleMode HPFrScaleMode;
    public ScaleMode HPBkScaleMode;
    private float BloodShowTime=0f;
    public float BloodShowDistance;


    private GameObject healthTemp;
    private AudioSource audioSource;
    private Vector3 RandomVector3;
    private Bullet Temp;
    private GameObject tempEffect;
    private void Start()
    {
        GetShootPoints();
    }
    public override void Fire()//开火只执行开火，瞄准由模式来确定
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
                    script.fatherTank = this;
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

    // Update is called once per frame
    void Update()
    {
        switch (mode)
        {
            case E_TankMode.alert://在移动点之间行走，如果玩家距离比较近，才会执行瞄准
                alert();
                break;
            case E_TankMode.fight://在警戒的基础上加上开火
                fight();
                break;
            case E_TankMode.escape://暂时不写
                escape();
                break;
            default:
                break;
        }
    }
   
    public void ChangeHP(int hp)
    {
        currentHp += hp;
        if (currentHp >= maxHp)
        {
            currentHp = maxHp; 
        }
        if (healthEffect != null && hp > 0)//回血触发回血特效
        {
            healthTemp = GameObject.Instantiate(healthEffect, this.transform.position, this.transform.rotation);
            audioSource = healthTemp.GetComponent<AudioSource>();
            audioSource.volume = DateMgr.Instance.musicData.EffectNum;
            audioSource.mute = DateMgr.Instance.musicData.isEffect;
            audioSource.Play();
            Destroy(healthTemp, 1f);
        }
        else
        {
            if (currentHp<=0)
            {
                //得到分数奖励
                GameSceneUI.Instance.AddScore(30);
                //随机掉落物
                int i = UnityEngine.Random.Range(0, 101);
                if (i<=posible)
                {
                    int j = UnityEngine.Random.Range(0, props.Length);
                    RandomVector3 = this.transform.position;
                    RandomVector3.y = 0.7f;
                    GameObject.Instantiate(props[j], RandomVector3, this.transform.rotation);
                }
                Dead();
            }
        }
       
    }
  



    public void alert()
    {
        Move();
        if (Vector3.Distance(this.transform.position,Player.transform.position)<AimDistance)
        {
            Aim();
        }
        if (Vector3.Distance(this.transform.position, Player.transform.position) < FightDistance)
        {
            mode=E_TankMode.fight; //进入战斗状态
        }

    }
    public void fight()
    {
        Move();
        Aim();
        Fire();
        if (Vector3.Distance(this.transform.position, Player.transform.position) > BackDistance)
        {
            mode = E_TankMode.alert; //变回正常状态
        }
    }
    public void escape()
    {
        
    }

    public void Move()
    {
        //身体看向自己的目标点
        Body.transform.LookAt(targetPoint.transform);
        //头是用来瞄准
        this.transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        if (Vector3.Distance(this.gameObject.transform.position, targetPoint.transform.position) < 0.1f || Time.time - lastMoveTime > moveCheckTime)//如果到达了目标点就换一个目标点
        {
            RandomPos();
            lastMoveTime = Time.time;
        }

    }
    public void Aim()
    {
        if (Player != null)
        {
            Head.transform.LookAt(Player.transform);
        }
    }
    public void RandomPos()
    {
        if (movePoints.Length == 0)
            return;
        targetPoint = movePoints[UnityEngine.Random.Range(0, movePoints.Length)];
    }
    public void GetShootPoints()
    {
        shootPoints = this.GetComponentsInChildren<ShootPoint>();
    }




    // 触发器碰撞检测
    private void OnCollisionEnter(Collision collision)
    {
        ProcessCollision(collision.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        ProcessCollision(other.gameObject);
    }

    private void ProcessCollision(GameObject gameObject)
    {
        Temp = gameObject.GetComponent<Bullet>();

        if (Temp != null)
        {
            if (Temp.fatherTank == null)
            {
                return;
            }
            if (Temp.fatherTank.CompareTag("Player"))
            {
                ChangeHP(-Temp.BulletAtk);
                BloodShowTime = 3f;
            }
        }
    }

    //血条显示(通过代码写出来，而不是用GUI拖一个)
    private void OnGUI()
    {
        if (Vector3.Distance(this.transform.position,Player.transform.position)>BloodShowDistance)
        {
            return;
        }
        if(BloodShowTime>0)
        {
            BloodShowTime -= Time.deltaTime;
            //1.把怪物当前的位置转换为屏幕位置
            Vector3 screenPos = Camera.main.WorldToScreenPoint(this.transform.position);
            //2.屏幕坐标变成GUI坐标
            screenPos.x -= 100;
            screenPos.y = Screen.height - screenPos.y - 100;
          
            HPBkRect.x = screenPos.x;
            HPBkRect.y = screenPos.y;
            HPBkRect.height = 23;
            HPBkRect.width = 150;
            GUI.DrawTexture(HPBkRect, HPBk,HPBkScaleMode,true);

            HPFrRect.x = screenPos.x;
            HPFrRect.y = screenPos.y;
            HPFrRect.height = 23;
            HPFrRect.width = 150 * ((float)currentHp / maxHp);
            GUI.DrawTexture(HPFrRect, HPFr,HPFrScaleMode,true);



        }
    }
}
