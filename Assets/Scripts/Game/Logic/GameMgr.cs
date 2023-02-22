using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EGameTheme
{
    Grass,
    Winter,
}

public class GameMgr : MonoSingleton<GameMgr>
{
    public Player player;
    private EGameTheme gameTheme;//主题
    public Sprite platformSprite { get; set; }//平台图
    public GameParam config { get; set; }//配置
    public bool playerStartMove;

    public bool isGameover;
    public bool isPause;

    public int score;
    public int diamond;

    private void Awake()
    {
        MsgSystem.AddListener(MsgConst.Gameover, Gameover);
    }

    private void OnDestroy()
    {
        MsgSystem.RemoveListener(MsgConst.Gameover, Gameover);
    }

    /// <summary>
    /// 初始化游戏
    /// </summary>
    public void InitGame()
    {
        config = GameParam.Get();

        //游戏数据
        player = null;
        score = 0;
        diamond = 0;
        isPause = false;
        playerStartMove = false;
        isGameover = false;
        gameTheme = (EGameTheme)Random.Range(0, 2);
        platformSprite = GetRandomSprite_Common();
        //地图数据
        Spawner.Ins.InitMap();

        MsgSystem.Dispatch(MsgConst.StartGame);
    }

    /// <summary>
    /// 重置游戏
    /// </summary>
    public void ResetGame()
    {
        Camera.main.GetComponent<CameraFollow>().Reset();
        Spawner.Ins.platformPool.PutAll();
        Spawner.Ins.platformGroup1Pool.PutAll();
        Spawner.Ins.platformGroup2Pool.PutAll();
        Spawner.Ins.platformGroup3Pool.PutAll();
        Spawner.Ins.dieEffectPool.PutAll();
        Spawner.Ins.diamondPool.PutAll();
        if (player != null)
        {
            Destroy(player.gameObject);
        }
    }

    /// <summary>
    /// 游戏结束
    /// </summary>
    private void Gameover()
    {
        UIMgr.Ins.gameoverPanel.SetActive(true);
    }

    /// <summary>
    /// 吃到钻石
    /// </summary>
    public void EatDiamond()
    {
        diamond++;
        MsgSystem.Dispatch(MsgConst.EatDiamond);
    }

    /// <summary>
    /// 得到随机的图（普通）
    /// </summary>
    private Sprite GetRandomSprite_Common()
    {
        switch (gameTheme)
        {
            case EGameTheme.Grass:
                return config.grassPlatform[Random.Range(0, config.grassPlatform.Length)];
            case EGameTheme.Winter:
                return config.winterPlatform[Random.Range(0, config.winterPlatform.Length)];
            default:
                return null;
        }
    }

    /// <summary>
    /// 得到随机的图（障碍）
    /// </summary>
    public Sprite GetRandomSprite_Barriar()
    {
        switch (gameTheme)
        {
            case EGameTheme.Grass:
                return config.grassPlatform_barriar[Random.Range(0, config.grassPlatform_barriar.Length)];
            case EGameTheme.Winter:
                return config.winterPlatform_barriar[Random.Range(0, config.winterPlatform_barriar.Length)];
            default:
                return null;
        }
    }
}
