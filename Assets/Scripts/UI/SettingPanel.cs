using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : BasePanel
{
    public Button btnClose;
    public Toggle togMusic;
    public Toggle togSound;
    public Slider sliderMusic;
    public Slider sliderSound;

    public override void Init()
    {
        //根据存储的数据初始化面板数据
        MusicData data = GameDataMgr.Instance.musicData;
        togMusic.isOn = data.isMusicOn;
        togSound.isOn = data.isSoundOn;
        sliderMusic.value = data.musicValue;
        sliderSound.value = data.soundValue;

        btnClose.onClick.AddListener(() =>
        {
            GameDataMgr.Instance.SaveMusicData();
            //关闭设置面板
            UIManager.Instance.HidePanel<SettingPanel>();
        });

        togMusic.onValueChanged.AddListener((v) =>
        {
            //开启/关闭音量
            BKMusic.Instance.SetMusicOn(v);
            GameDataMgr.Instance.musicData.isMusicOn = v;
        });

        togSound.onValueChanged.AddListener((v) =>
        {
            GameDataMgr.Instance.musicData.isSoundOn = v;
        });

        sliderMusic.onValueChanged.AddListener((v) =>
        {
            BKMusic.Instance.ChangeMusicValue(v);
            GameDataMgr.Instance.musicData.musicValue = v;
        });

        sliderSound.onValueChanged.AddListener((v) =>
        {
            GameDataMgr.Instance.musicData.soundValue = v;
        });
    }
}
