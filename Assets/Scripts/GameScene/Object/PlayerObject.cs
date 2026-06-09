using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class PlayerObject : MonoBehaviour
{
    public int atk;
    public int money;
    private float rotateSpeed = 50;

    //角色的动画状态机
    private Animator animator;

    //枪械的发射位置
    public Transform gunFirePos;


    public void InitPlayerInfo(int atk,int money)
    {
        this.atk = atk;
        this.money = money;
        UpdateMoney();
    }
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //移动逻辑
        animator.SetFloat("VSpeed", Input.GetAxis("Vertical"));
        animator.SetFloat("HSpeed", Input.GetAxis("Horizontal"));
        //旋转逻辑
        this.transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            animator.SetLayerWeight(1, 1);
        }else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            animator.SetLayerWeight(1, 0);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            animator.SetTrigger("Roll");
        }

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack");
        }
    }

    /// <summary>
    /// 处理刀的攻击检测
    /// </summary>
    public void KnifeEvent()
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position + this.transform.forward + this.transform.up,
                                                     1, 1 << LayerMask.NameToLayer("Monster"));

        //播放音效
        GameDataMgr.Instance.PlaySound("Music/knife");
        //执行碰撞逻辑
        for (int i = 0; i < colliders.Length; i++)
        {
            MonsterObject monster = colliders[i].gameObject.GetComponent<MonsterObject>();
            if (monster != null && !monster.isDead)
            {
                monster.Wound(this.atk);
                break;
            }
        }
    }
    //public void KnifeEvent()
    //{
    //    Debug.Log("1. 挥刀判定开始执行");

    //    Vector3 center = this.transform.position + this.transform.forward + this.transform.up;
    //    Collider[] colliders = Physics.OverlapSphere(center, 1, 1 << LayerMask.NameToLayer("Monster"));

    //    Debug.Log($"2. 扫描到了 {colliders.Length} 个 Monster 层的碰撞体");

    //    for (int i = 0; i < colliders.Length; i++)
    //    {
    //        MonsterObject monster = colliders[i].gameObject.GetComponent<MonsterObject>();
    //        if (monster != null)
    //        {
    //            Debug.Log("3. 成功获取到 MonsterObject，准备扣血");
    //            monster.Wound(this.atk);
    //            break;
    //        }
    //        else
    //        {
    //            Debug.LogWarning($"获取 MonsterObject 失败！碰到的是：{colliders[i].gameObject.name}");
    //        }
    //    }
    //}

    public void ShootEvent()
    {
        RaycastHit[] hits = Physics.RaycastAll(new Ray(gunFirePos.position, this.transform.forward), 1000, 1 << LayerMask.NameToLayer("Monster"));

        //播放音效
        GameDataMgr.Instance.PlaySound("Music/shoot");
        for (int i = 0; i < hits.Length; i++)
        {
            MonsterObject monster = hits[i].collider.gameObject.GetComponent<MonsterObject>();
            if (monster != null && !monster.isDead)
            {
                //播放射击命中特效
                GameObject effObj = Instantiate(Resources.Load<GameObject>(GameDataMgr.Instance.nowSelRole.hitEff));
                effObj.transform.position = hits[i].point;
                effObj.transform.rotation = Quaternion.LookRotation(hits[i].normal);
                Destroy(effObj, 1);

                monster.Wound(this.atk);
                break;
            }
        }
    }

    //更新界面上的金币
    public void UpdateMoney()
    {
        UIManager.Instance.GetPanel<GamePanel>().UpdateMoney(money);
    }

    //得加钱
    public void AddMoney(int money)
    {
        this.money += money;
        UpdateMoney();
    }
}
