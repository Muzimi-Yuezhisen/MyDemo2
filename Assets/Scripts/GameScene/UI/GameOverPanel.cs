using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPanel : BasePanel
{
    public TextMeshProUGUI txtWin;
    public TextMeshProUGUI txtInfo;
    public TextMeshProUGUI txtMoney;
    public Button btnExit;
    public override void Init()
    {
        btnExit.onClick.AddListener(() =>
        {
            //ÇÐ»»³¡¾°
            UIManager.Instance.HidePanel<GameOverPanel>();
            UIManager.Instance.HidePanel<GamePanel>();

            GameLevelMgr.Instance.ClearInfo();
            SceneManager.LoadScene("BeginScene");
        });
    }

    public void InitInfo(int money, bool isWin)
    {
        txtWin.text = isWin ? "Win" : "Lose";
        txtInfo.text = isWin ? "Get Rewards" : "Get Comfort";
        txtMoney.text = "$ " + money;

        GameDataMgr.Instance.playerData.leftMoney += money;
        GameDataMgr.Instance.SavePlayerData();
    }

    public override void ShowMe()
    {
        base.ShowMe();
        Cursor.lockState = CursorLockMode.None;
    }
}
