using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataMgr
{
    private static GameDataMgr instance = new GameDataMgr();
    public static GameDataMgr Instance => instance;

    //音乐音效数据
    public MusicData musicData;
    //角色数据
    public List<RoleInfo> roleInfoList;
    //玩家数据
    public PlayerData playerData;
    //记录选择的角色，之后可以在游戏场景创建
    public RoleInfo nowSelRole;
    //场景数据
    public List<SceneInfo> sceneInfoList;
    //怪物数据
    public List<MonsterInfo> monsterInfoList;
    //防御塔数据
    public List<TowerInfo> towerInfoList;
    private GameDataMgr()
    {
        //初始化默认数据
        musicData = JsonMgr.Instance.LoadData<MusicData>("MusicData");
        roleInfoList = JsonMgr.Instance.LoadData<List<RoleInfo>>("RoleInfo");
        playerData = JsonMgr.Instance.LoadData<PlayerData>("PlayerData");
        sceneInfoList = JsonMgr.Instance.LoadData<List<SceneInfo>>("SceneInfo");
        monsterInfoList = JsonMgr.Instance.LoadData<List<MonsterInfo>>("MonsterInfo");
        towerInfoList = JsonMgr.Instance.LoadData<List<TowerInfo>>("TowerInfo");
    }

    public void SaveMusicData()
    {
        JsonMgr.Instance.SaveData(musicData,"MusicData");
    }

    public void SavePlayerData()
    {
        JsonMgr.Instance.SaveData(playerData, "PlayerData");
    }

    //播放音效
    public void PlaySound(string resName)
    {
        GameObject musicObj = new GameObject();
        AudioSource audioSource = musicObj.AddComponent<AudioSource>();
        audioSource.clip = Resources.Load<AudioClip>(resName);
        audioSource.volume = musicData.soundValue;
        audioSource.mute = !musicData.isSoundOn;
        audioSource.Play();
        GameObject.Destroy(musicObj, 2);
    }
}
