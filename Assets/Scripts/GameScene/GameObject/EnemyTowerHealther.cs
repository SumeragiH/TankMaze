using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTowerHealther : MonoBehaviour
{
    private float lastFireTime = 0f;
    public TankBase[] tanks;//要治疗的坦克
    public float gapTime;
    public float distance;//治疗范围
    public int healthHP;//治疗量
    public GameObject healthEffect;
    private GameObject Temp;
    private AudioSource audioSource;
    //转动一圈治疗一次
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //循环，每次都计算坦克距离，如果小于设定值就回血
        if (Time.time - lastFireTime > gapTime)
        {
            if (tanks != null)
            {
                foreach (TankBase tank in tanks)
                {
                    if (tank != null)
                    {
                        if (Vector3.Distance(this.transform.position, tank.transform.position) < distance)
                        {
                            (tank as EnemyTank).ChangeHP(120);
                            Temp = GameObject.Instantiate(healthEffect, this.transform.position, this.transform.rotation);
                            audioSource = Temp.GetComponent<AudioSource>();
                            audioSource.volume = DateMgr.Instance.musicData.EffectNum;
                            audioSource.mute = DateMgr.Instance.musicData.isEffect;
                            audioSource.Play();
                            Destroy(Temp, 1f);
                        }
                        lastFireTime = Time.time;
                    }
                 
                }
               
            }
          
        }
        //坦克中写回血特效，
        
    }
}
