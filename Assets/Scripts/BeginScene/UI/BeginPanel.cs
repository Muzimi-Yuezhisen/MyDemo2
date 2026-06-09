using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeginPanel : BasePanel
{
    public Button buttonStart;
    public Button buttonSetting;
    public Button buttonAbout;
    public Button buttonExit;
    public override void Init()
    {
        buttonStart.onClick.AddListener(() =>
        {
            //播放动画
            Camera.main.GetComponent<CameraAnimator>().TurnLeft(() =>
            {
                //显示选角面板
                UIManager.Instance.ShowPanel<ChooseHeroPanel>();
                //隐藏开始面板
                UIManager.Instance.HidePanel<BeginPanel>();
            });
        });

        buttonSetting.onClick.AddListener(() =>
        {
            //GameObject settingPanel = GameObject.Instantiate(Resources.Load<SettingPanel>("UI/" + "SettingPanel"));
            UIManager.Instance.ShowPanel<SettingPanel>();
        });

        buttonAbout.onClick.AddListener(() =>
        {

        });
        
        buttonExit.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }

}
