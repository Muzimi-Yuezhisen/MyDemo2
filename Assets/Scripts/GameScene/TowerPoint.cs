using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPoint : MonoBehaviour
{
    private GameObject towerObj = null;
    public TowerInfo nowTowerInfo = null;

    public List<int> selectIds;

    public void CreateTower(int id)
    {
        TowerInfo towerInfo = GameDataMgr.Instance.towerInfoList[id - 1];
        //没钱直接退
        if (towerInfo.money > GameLevelMgr.Instance.player.money) return;
        GameLevelMgr.Instance.player.AddMoney(-towerInfo.money);

        //创建防御塔
        if(towerObj != null)
        {
            Destroy(towerObj);
            towerObj = null;
        }
        towerObj = Instantiate(Resources.Load<GameObject>(towerInfo.res), this.transform.position, Quaternion.identity);
        towerObj.GetComponent<TowerObject>().InitInfo(towerInfo);
        nowTowerInfo = towerInfo;

        //更新界面
        if(nowTowerInfo.next != 0)
        {
            UIManager.Instance.GetPanel<GamePanel>().UpdateTower(this);
        }
        else
        {
            UIManager.Instance.GetPanel<GamePanel>().UpdateTower(null);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //塔塔满级后不需要再显示UI
        if (nowTowerInfo != null && nowTowerInfo.next == 0) return;
        UIManager.Instance.ShowPanel<GamePanel>().UpdateTower(this);
    }

    private void OnTriggerExit(Collider other)
    {
        //传空就隐藏UI
        UIManager.Instance.ShowPanel<GamePanel>().UpdateTower(null);
    }
}
