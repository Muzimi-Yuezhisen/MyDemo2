using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 造塔的统一UI控件
/// </summary>
public class TowerBtn : MonoBehaviour
{
    public Image imgPic;
    public TextMeshProUGUI txtTip;
    public TextMeshProUGUI txtMoney;

    public void InitInfo(int id, string inputStr)
    {
        TowerInfo info = GameDataMgr.Instance.towerInfoList[id - 1];
        imgPic.sprite = Resources.Load<Sprite>(info.imgRes);
        txtMoney.text = "$:" + info.money;
        txtTip.text = inputStr;
        //钱不够直接显示金钱不足
        if (info.money > GameLevelMgr.Instance.player.money) txtMoney.text = "poor";
    }
}
