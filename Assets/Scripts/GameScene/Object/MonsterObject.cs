using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterObject : MonoBehaviour
{
    private Animator animator;
    //寻路
    private NavMeshAgent agent;
    private MonsterInfo monsterInfo;

    //当前血量
    private int hp;
    public bool isDead = false;

    //记录上次攻击的时间
    private float lastTime = 0;
    // Start is called before the first frame update
    void Awake()
    {
        animator = this.GetComponent<Animator>();
        agent = this.GetComponent<NavMeshAgent>();
    }

    //初始化
    public void InitInfo(MonsterInfo monsterInfo)
    {
        this.monsterInfo = monsterInfo;
        hp = monsterInfo.hp;
        //给状态机设置动画
        animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(monsterInfo.animator);
        //设置寻路组件上的速度
        agent.speed = monsterInfo.moveSpeed;
        agent.acceleration = monsterInfo.moveSpeed;
        agent.angularSpeed = monsterInfo.roundSpeed;
    }

    //受伤和死亡
    public void Wound(int damage)
    {
        if (isDead) return;

        hp -= damage;
        animator.SetTrigger("Wound");
        if(hp <= 0)
        {
            Dead();
        }
        else
        {
            //饿啊
            GameDataMgr.Instance.PlaySound("Music/monster");
        }
    }

    public void Dead()
    {
        isDead = true;
        //停止寻路
        //agent.isStopped = true;
        agent.enabled = false;
        //死亡动画
        animator.SetBool("Death", true);
        //死亡音效
        GameDataMgr.Instance.PlaySound("Music/monster");
        //得加钱
        GameLevelMgr.Instance.player.AddMoney(10);

    }
    //死亡后移除对象
    public void DeadEvent()
    {
        //GameLevelMgr.Instance.ChangeMonsterNum(-1);
        GameLevelMgr.Instance.RemoveMonster(this);
        Destroy(this.gameObject);
        //检查是否游戏结束
        if (GameLevelMgr.Instance.CheckOver())
        {
            GameOverPanel gameOverPanel = UIManager.Instance.ShowPanel<GameOverPanel>();
            gameOverPanel.InitInfo(GameLevelMgr.Instance.player.money, true);
        }
    }

    //出生动画结束后移动
    public void BornOver()
    {
        agent.SetDestination(MainTowerObject.Instance.transform.position);
        animator.SetBool("Run", true);
    }
    // Update is called once per frame
    void Update()
    {
        //寻路结束，开始攻击
        if (isDead) return;
        animator.SetBool("Run", agent.velocity != Vector3.zero);
        if(Vector3.Distance(this.transform.position,MainTowerObject.Instance.transform.position) <= 5
            && Time.time >= monsterInfo.atkOffset)
        {
            lastTime = Time.time;
            animator.SetTrigger("Attack");
        }
    }

    //伤害检测
    public void AtkEvent()
    {
        Collider[] colliders =  Physics.OverlapSphere(this.transform.position + this.transform.forward + this.transform.up,
                                1, 1 << LayerMask.NameToLayer("MainTower"));
        GameDataMgr.Instance.PlaySound("Music/attack");
        for(int i = 0; i < colliders.Length; i++)
        {
            if(MainTowerObject.Instance.gameObject == colliders[i].gameObject)
            {
                MainTowerObject.Instance.Wound(monsterInfo.atk);
            }
        }
    }
}
