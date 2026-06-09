using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPoint : MonoBehaviour
{
    //最大波数
    public int maxWave;
    //每波数量
    public int monsterNumOneWave;
    private int nowNum;

    //每波怪物形态不同 不同id在配置表中配置
    public List<int> monsterIDs;
    private int nowID;

    //怪物创建的间隔时间
    public float createOffsetTime;
    //不同波的间隔时间
    public float delayTime;
    //第一波的间隔时间
    public float firstDelayTime;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("CreateWave", firstDelayTime);
        GameLevelMgr.Instance.AddMonsterPoint(this);
        GameLevelMgr.Instance.UpdateMaxNum(maxWave);
    }

    /// <summary>
    /// 每次创建一波
    /// </summary>
    private void CreateWave()
    {
        nowID = monsterIDs[Random.Range(0, monsterIDs.Count)];
        nowNum = monsterNumOneWave;
        CreateMonster();
        --maxWave;
        GameLevelMgr.Instance.ChangeNowWaveNum(1);
    }

    private void CreateMonster()
    {
        //表中是从1开始配置的
        MonsterInfo monsterInfo = GameDataMgr.Instance.monsterInfoList[nowID - 1];
        GameObject obj = Instantiate(Resources.Load<GameObject>(monsterInfo.res), this.transform.position, Quaternion.identity);
        MonsterObject monsterObject = obj.AddComponent<MonsterObject>();
        monsterObject.InitInfo(monsterInfo);
        //GameLevelMgr.Instance.ChangeMonsterNum(1);
        GameLevelMgr.Instance.AddMonster(monsterObject);

        nowNum--;
        if(nowNum == 0)
        {
            if(maxWave > 0)
            {
                Invoke("CreateWave", delayTime);
            }
        }
        else
        {
            Invoke("CreateMonster", createOffsetTime);
        }
    }

    /// <summary>
    ///该位置是否结束出怪
    /// </summary>
    /// <returns></returns>
    public bool CheckOver()
    {
        return nowNum == 0 && maxWave == 0;
    }
}
