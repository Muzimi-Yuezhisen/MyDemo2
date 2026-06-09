using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerObject : MonoBehaviour
{
    public Transform head;
    public Transform firePos;
    public float roundSpeed = 20;

    //当前炮台数据
    private TowerInfo info;
    //攻击对象
    private MonsterObject targetObj;
    private List<MonsterObject> targetObjs;
    //计时
    private float nowTime;

    private Vector3 monsterPos;

    public void InitInfo(TowerInfo info)
    {
        this.info = info;
    }

    // Update is called once per frame
    void Update()
    {
        //单体攻击
        if(info.type == 1)
        {
            if(targetObj == null || targetObj.isDead || 
               Vector3.Distance(this.transform.position,targetObj.transform.position) > info.atkRange)
            {
                //练习的另外一种写法，不是采用范围检测，而是遍历场景上的所有怪物
                targetObj = GameLevelMgr.Instance.FindMonster(this.transform.position, info.atkRange);
            }

            if (targetObj == null) return;

            monsterPos = targetObj.transform.position;
            monsterPos.y = head.position.y;

            head.rotation = Quaternion.Slerp(head.rotation, Quaternion.LookRotation(monsterPos - head.position),
                                            roundSpeed * Time.deltaTime);
            //角度差较小时就可以攻击
            if(Vector3.Angle(head.forward,monsterPos-head.position) < 5 && 
                Time.time - nowTime >= info.offsetTime)
            {
                //没必要再做射线检测
                targetObj.Wound(info.atk);
                //播放音效
                GameDataMgr.Instance.PlaySound("Music/towerShoot");
                //创建特效
                GameObject effObj = Instantiate(Resources.Load<GameObject>(info.eff), firePos.position, firePos.rotation);
                Destroy(effObj, 0.2f);
                nowTime = Time.time;
            }
        }
        else //群体攻击
        {
            targetObjs = GameLevelMgr.Instance.FindMonsters(this.transform.position, info.atkRange);
            if(targetObjs.Count > 0 && Time.time >= nowTime)
            {
                //创建特效
                GameObject effObj = Instantiate(Resources.Load<GameObject>(info.eff), firePos.position, firePos.rotation);
                Destroy(effObj, 0.2f);
                //播放音效
                GameDataMgr.Instance.PlaySound("Music/LaserShoot");

                for (int i = 0; i < targetObjs.Count; i++)
                {
                    targetObjs[i].Wound(info.atk);
                }

                nowTime = Time.time;
            }
        }
    }
}
