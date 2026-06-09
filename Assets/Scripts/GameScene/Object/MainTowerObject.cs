using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainTowerObject : MonoBehaviour
{
    private static MainTowerObject instance;
    public static MainTowerObject Instance => instance;
    private int hp;
    private int maxHp;
    private bool isDead;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHp(int hp, int maxHp)
    {
        this.hp = hp;
        this.maxHp = maxHp;
        //¸üÐÂUI
        UIManager.Instance.GetPanel<GamePanel>().UpdateTowerHp(hp, maxHp);
    }

    public void Wound(int damage)
    {
        if (isDead) return;
        hp -= damage;
        if(hp <= 0)
        {
            hp = 0;
            isDead = true;
            //Game Over
            GameOverPanel gameOverPanel = UIManager.Instance.ShowPanel<GameOverPanel>();
            gameOverPanel.InitInfo((int)(GameLevelMgr.Instance.player.money * 0.5f), false);
        }
        UpdateHp(hp, maxHp);
    }
    private void OnDestroy()
    {
        instance = null;
    }
}
