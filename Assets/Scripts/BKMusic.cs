using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BKMusic : MonoBehaviour
{
    private static BKMusic instance;
    public static BKMusic Instance => instance;

    private AudioSource audio;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        //得到音频脚本
        audio = this.GetComponent<AudioSource>();
        //设置音乐和音效
        MusicData musicData = GameDataMgr.Instance.musicData;
        SetMusicOn(musicData.isMusicOn);
        ChangeMusicValue(musicData.musicValue);
    }

    public void SetMusicOn(bool isOpen)
    {
        audio.mute = !isOpen;
    }

    public void ChangeMusicValue(float value)
    {
        audio.volume = value;
    }
}
