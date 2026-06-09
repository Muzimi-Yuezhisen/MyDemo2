using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ChooseHeroPanel : BasePanel
{
    //左右切换按键
    public Button btnLeft;
    public Button btnRight;

    //解锁人物按键
    public Button btnUnlock;
    public TextMeshProUGUI txtUnlock;

    //开始和返回
    public Button btnStart;
    public Button btnReturn;

    //显示的余额
    public TextMeshProUGUI txtMoney;

    //显示的角色名字
    public TextMeshProUGUI txtName;

    //创建角色预设体的位置
    private Transform heroPos;

    //当前场景的人物、数据和索引
    private GameObject heroObj;
    private RoleInfo nowRoleData;
    private int nowIndex = 0;
    public override void Init()
    {
        heroPos = GameObject.Find("HeroPos").transform;

        //更新UI上的玩家金币
        txtMoney.text = GameDataMgr.Instance.playerData.leftMoney.ToString();

        btnLeft.onClick.AddListener(() =>
        {
            --nowIndex;
            if (nowIndex < 0) nowIndex = GameDataMgr.Instance.roleInfoList.Count - 1;

            ChangeModel();
        });

        btnRight.onClick.AddListener(() =>
        {
            ++nowIndex;
            if (nowIndex >= GameDataMgr.Instance.roleInfoList.Count) nowIndex = 0;

            ChangeModel();
        });

        btnStart.onClick.AddListener(() =>
        {
            //保存角色信息
            GameDataMgr.Instance.nowSelRole = nowRoleData;
            //切换场景
            UIManager.Instance.HidePanel<ChooseHeroPanel>();
            UIManager.Instance.ShowPanel<ChooseScenePanel>();
        });

        btnUnlock.onClick.AddListener(() =>
        {
            //购买角色
            PlayerData data = GameDataMgr.Instance.playerData;
            if(data.leftMoney >= nowRoleData.lockMoney)
            {
                data.leftMoney -= nowRoleData.lockMoney;
                txtMoney.text = data.leftMoney.ToString();
                //记录已购买角色
                data.buyedRoles.Add(nowRoleData.id);
                //保存
                GameDataMgr.Instance.SavePlayerData();
                UpdateLockBtn();
                //print("购买成功");
                UIManager.Instance.ShowPanel<TipPanel>().ChangeInfo("Successfully purchased");
            }
            else
            {
                //提示没钱
                //print("购买失败");
                UIManager.Instance.ShowPanel<TipPanel>().ChangeInfo("Insufficient coins");
            }
        });

        btnReturn.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<ChooseHeroPanel>();
            Camera.main.GetComponent<CameraAnimator>().TurnRight(() =>
            {
                UIManager.Instance.ShowPanel<BeginPanel>();
            });
        });

        ChangeModel();
    }

    /// <summary>
    /// 更新界面上的角色模型
    /// </summary>
    private void ChangeModel()
    {
        if(heroObj != null)
        {
            Destroy(heroObj);
            heroObj = null;
        }

        //读取信息
        nowRoleData = GameDataMgr.Instance.roleInfoList[nowIndex];
        heroObj = Instantiate(Resources.Load<GameObject>(nowRoleData.res),heroPos.position,heroPos.rotation);
        Destroy(heroObj.GetComponent<PlayerObject>());
        UpdateLockBtn();
        txtName.text = nowRoleData.tips;
    }

    private void UpdateLockBtn()
    {
        //若角色未解锁，则显示解锁、隐藏开始按钮
        if(nowRoleData.lockMoney > 0 && !GameDataMgr.Instance.playerData.buyedRoles.Contains(nowRoleData.id))
        {
            btnUnlock.gameObject.SetActive(true);
            txtUnlock.text = "$ " + nowRoleData.lockMoney;
            btnStart.gameObject.SetActive(false);
        }
        else
        {
            btnUnlock.gameObject.SetActive(false);
            btnStart.gameObject.SetActive(true);
        }
    }

    public override void HideMe(UnityAction callback)
    {
        base.HideMe(callback);
        if(heroObj != null)
        {
            DestroyImmediate(heroObj);
            heroObj = null;
        }
    }
}
