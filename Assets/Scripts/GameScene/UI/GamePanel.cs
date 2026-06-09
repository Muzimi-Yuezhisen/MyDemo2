using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePanel : BasePanel
{
    public Image imgHp;
    public TextMeshProUGUI txtHp;

    public TextMeshProUGUI txtWave;
    public TextMeshProUGUI txtMoney;

    private TowerPoint nowSelTowerPoint;

    //血条控件的宽度
    public float hpWidth = 500;

    public Button btnReturn;

    //防御塔界面的父对象，控制显影
    public Transform botTrans;
    //塔塔
    public List<TowerBtn> towerBtns = new List<TowerBtn>();
    //检测玩家是否位于造塔区域内
    private bool checkPoint = false;
    public override void Init()
    {
        btnReturn.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<GamePanel>();
            SceneManager.LoadScene("BeginScene");
        });
        botTrans.gameObject.SetActive(false);
        //锁定鼠标
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void UpdateTowerHp(int hp, int maxHP)
    {
        txtHp.text = hp + "/" + maxHP;
        (imgHp.transform as RectTransform).sizeDelta = new Vector2((float)hp / maxHP * hpWidth, 50);
    }

    public void UpdateWaveNum(int nowNum, int maxNum)
    {
        txtWave.text = nowNum + "/" + maxNum;
    }

    public void UpdateMoney(int money)
    {
        txtMoney.text = money.ToString();
    }

    //更新防御塔UI
    public void UpdateTower(TowerPoint towerPoint)
    {       
        nowSelTowerPoint = towerPoint;

        if(nowSelTowerPoint == null)
        {
            checkPoint = false;
            botTrans.gameObject.SetActive(false);
        }
        else
        {
            checkPoint = true;
            botTrans.gameObject.SetActive(true);
            //没有造过塔
            if (nowSelTowerPoint.nowTowerInfo == null)
            {
                for (int i = 0; i < towerBtns.Count; i++)
                {
                    towerBtns[i].gameObject.SetActive(true);
                    towerBtns[i].InitInfo(nowSelTowerPoint.selectIds[i], "Number" + (i + 1));
                }
            }
            else
            {
                //已经选择过塔，现在只需要展示选择塔的升级信息
                for (int i = 0; i < towerBtns.Count; i++)
                {
                    towerBtns[i].gameObject.SetActive(false);
                }
                towerBtns[1].gameObject.SetActive(true);
                towerBtns[1].InitInfo(nowSelTowerPoint.nowTowerInfo.next, "Space");
            }
        }

    }

    protected override void Update()
    {
        base.Update();

        if (!checkPoint) return;
        //没造过塔，就检测123造塔
        if(nowSelTowerPoint.nowTowerInfo == null)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                nowSelTowerPoint.CreateTower(nowSelTowerPoint.selectIds[0]);
            }else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                nowSelTowerPoint.CreateTower(nowSelTowerPoint.selectIds[1]);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                nowSelTowerPoint.CreateTower(nowSelTowerPoint.selectIds[2]);
            }
        }
        else //造过就检测空格，升级
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                nowSelTowerPoint.CreateTower(nowSelTowerPoint.nowTowerInfo.next);
            }
        }
    }
}
