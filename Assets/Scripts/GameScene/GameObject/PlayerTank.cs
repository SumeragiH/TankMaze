using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerTank : TankBase
{
    public GameObject Head;
    public GameObject Pk;
    public Weapons weapon;
    private Bullet Temp;

    [Header("炮管角度限制")]
    public float minCannonAngle = -15f;  
    public float maxCannonAngle = 30f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }
    // Update is called once per frame
    void Update()
    {
        //前进与后退WS
        this.transform.Translate(Input.GetAxis("Vertical")*Vector3.forward*moveSpeed*Time.deltaTime);
        //左右身体旋转AD
        this.transform.Rotate(Input.GetAxis("Horizontal") * Vector3.up * bodyRotateSpeed * Time.deltaTime);
        //头旋转
        Head.transform.Rotate(Input.GetAxis("Mouse X")*Vector3.up*headRotateSpeed*Time.deltaTime);
        //头旋转的角度限制
        LimitCannonAngle();
        //炮管抬起
        //炮管旋转的角度限制
        Pk.transform.Rotate(Input.GetAxis("Mouse Y") * Vector3.left * PkRotateSpeed * Time.deltaTime);

        //鼠标左键或者长按控制射击
        if (Input.GetMouseButtonDown(0)||Input.GetMouseButton(0))
        {
            Fire();
        }
        if (currentHp<=0)
        {
            Dead();
        }
    }
    private void LimitCannonAngle()
    {
        // 获取炮管当前的欧拉角
        Vector3 currentRotation = Pk.transform.localEulerAngles;

        // 将角度转换为-180到180的范围
        float angleX = currentRotation.x;
        if (angleX > 180f)
        {
            angleX -= 360f;
        }


        angleX = Mathf.Clamp(angleX, minCannonAngle, maxCannonAngle);

        // 应用限制后的角度
        Pk.transform.localEulerAngles = new Vector3(angleX, currentRotation.y, currentRotation.z);
    }
    public override void Fire()
    {
        weapon.Fire();
    }
    public override void Dead()
    {
        Time.timeScale = 0f;
        //不能删除这个玩家，因为玩家身上有主摄像机
        //只能打开失败的面板然后遮住玩家
        GameLosePanel.Instance.ShowPanel();
    }
    public override void Injured(TankBase other)
    {
        base.Injured(other);
        //更新UI的血条，
        GameSceneUI.Instance.ChangeHp(maxHp,currentHp);
    }
    public void ChangeWeapon(GameObject w,GameObject b)
    {
        weapon.ChangeWeapon(w,b);
    }
    //对奖励物品的反应
    public void ChangeHP(int hp)
    {
        //生命值不能超过最大生命值
        //改变UI
        currentHp += hp;
        if (currentHp>maxHp)
        {
            currentHp = maxHp;
        }
        GameSceneUI.Instance.ChangeHp(maxHp,currentHp);
    }
    public void ChangeMaxHP(int maxHP)//提升的最大血量
    {
        if (this.maxHp + maxHP<=1200)
        {
            //当小于限制的时候才可以增加
            GameSceneUI.Instance.UpMaxHP(maxHP, this.maxHp);
            this.maxHp += maxHP;
        }
    }
    public void ChangeDef(int def)
    {
        this.def += def;
        if (this.def>50)
        {
            this.def = 50;
        }
    }
    public void ChangeAtk(int atk)
    {
        this.atk += atk;//攻击力无上限
    }
    // 物理碰撞检测
    private void OnCollisionEnter(Collision collision)
    {
        ProcessCollision(collision.gameObject);
    }

    // 触发器碰撞检测
    private void OnTriggerEnter(Collider other)
    {
        ProcessCollision(other.gameObject);
    }

    // 统一的碰撞处理逻辑
    private void ProcessCollision(GameObject collidedObject)
    {
        Temp = collidedObject.GetComponent<Bullet>();

        if (Temp != null)
        {
            if (Temp.fatherTank == null)
            {
                return;
            }
            if (Temp.fatherTank.CompareTag("Enemy"))//如果是敌人的子弹进行碰撞
            {
                if (Temp.BulletAtk - def <= 0)
                {
                    return;//没能击穿护甲
                }
                else
                {
                    currentHp = currentHp - Temp.BulletAtk - def;
                }
                if (currentHp<=0)
                {
                    currentHp=0;
                    Dead();
                }
                GameSceneUI.Instance.ChangeHp(maxHp,currentHp);

            }
        }
    }
}
