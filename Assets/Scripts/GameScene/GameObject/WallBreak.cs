using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBreak : MonoBehaviour
{
    public int HP=350;
    public int posible=50;
    private Bullet Temp;
    public GameObject Effect;
    private GameObject tempEffect;
    private Vector3 RandomVector3;
    public GameObject[] Props;
    public int Score;
    //设置血量
    //受到碰撞就会触发脚本
    //墙不是触发器

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
            if (Temp.fatherTank.CompareTag("Player"))
            {
                HP = HP - Temp.BulletAtk;
                if (HP <= 0)
                {
                    tempEffect = GameObject.Instantiate(Effect, this.transform.position, this.transform.rotation);
                    int i = Random.Range(0, 101);
                    if (i <= posible)
                    {
                        RandomProp();
                    }
                    GameSceneUI.Instance.AddScore(Score);
                    Destroy(tempEffect, 1f);
                    Destroy(this.gameObject);
                }
            }
        }
    }
    public void RandomProp()
    {
        int i = Random.Range(0,Props.Length);
        RandomVector3 = this.transform.position;
        RandomVector3.y = 0.7f;
        GameObject.Instantiate(Props[i],RandomVector3,this.transform.rotation);
        //原地生成一个奖励物品
    }

}
