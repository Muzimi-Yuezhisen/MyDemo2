using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevelMgr
{
    private static GameLevelMgr instance = new GameLevelMgr();
    public static GameLevelMgr Instance => instance;
    private GameLevelMgr()
    {

    }

    //玩家脚本
    public PlayerObject player;
    //所有的出怪点
    private List<MonsterPoint> points = new List<MonsterPoint>();
    //记录怪物波数
    private int nowWaveNum = 0;
    private int maxWaveNum = 0;

    //记录场景上的怪物数量 在生成每个怪的时候++,死亡时--
    //private int nowMonsterNum = 0;

    //记录场景上怪物的列表
    private List<MonsterObject> monsterList = new List<MonsterObject>();

    public void InitInfo(SceneInfo info)
    {
        //显示UI
        UIManager.Instance.ShowPanel<GamePanel>();
        //创建玩家
        RoleInfo roleInfo = GameDataMgr.Instance.nowSelRole;
        Transform heroBornPos = GameObject.Find("HeroBornPos").transform;
        GameObject heroObj = GameObject.Instantiate(Resources.Load<GameObject>(roleInfo.res), heroBornPos.position, heroBornPos.rotation);
        player = heroObj.GetComponent<PlayerObject>();
        player.InitPlayerInfo(roleInfo.atk, info.money);
        //摄像机视角跟随
        Camera.main.GetComponent<CameraMove>().SetTatget(heroObj.transform);
        //初始化防御塔的血量
        MainTowerObject.Instance.UpdateHp(info.towerHp, info.towerHp);
    }

    //提供给外部，记录出怪点
    public void AddMonsterPoint(MonsterPoint monsterPoint)
    {
        points.Add(monsterPoint);
    }

    public void UpdateMaxNum(int num)
    {
        maxWaveNum += num;
        nowWaveNum = maxWaveNum;
        UIManager.Instance.GetPanel<GamePanel>().UpdateWaveNum(nowWaveNum, maxWaveNum);
    }

    public void ChangeNowWaveNum(int num)
    {
        nowWaveNum -= num;
        UIManager.Instance.GetPanel<GamePanel>().UpdateWaveNum(nowWaveNum, maxWaveNum);
    }
    public bool CheckOver()
    {
        for(int i = 0; i < points.Count; i++)
        {
            if (!points[i].CheckOver()) return false;
        }
        if (monsterList.Count > 0) return false;
        Debug.Log("Success");
        return true;
    }

    //public void ChangeMonsterNum(int num)
    //{
    //    nowMonsterNum += num;
    //}
    public void AddMonster(MonsterObject monsterObject)
    {
        monsterList.Add(monsterObject);
    }

    public void RemoveMonster(MonsterObject monsterObject)
    {
        monsterList.Remove(monsterObject);
    }

    //寻找范围内的怪物
    public MonsterObject FindMonster(Vector3 pos,int range)
    {
        for(int i = 0; i < monsterList.Count; i++)
        {
            if (!monsterList[i].isDead && Vector3.Distance(pos, monsterList[i].transform.position) <= range)
            {
                return monsterList[i];
            }
        }
        return null;
    }

    public List<MonsterObject> FindMonsters(Vector3 pos,int range)
    {
        List<MonsterObject> list = new List<MonsterObject>();
        for(int i = 0; i < monsterList.Count; i++)
        {
            if (!monsterList[i].isDead && Vector3.Distance(pos, monsterList[i].transform.position) <= range)
            {
                list.Add(monsterList[i]);
            }
        }
        return list;
    }

    //清空所有数据，准备下次开始游戏
    public void ClearInfo()
    {
        points.Clear();
        monsterList.Clear();
        nowWaveNum = maxWaveNum = 0;
        player = null;
    }
}
