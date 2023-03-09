using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public enum EGameTheme
{
    Grass,
    Winter,
}

public class GameMgr : MonoSingleton<GameMgr>
{
    private const string GameDataPath = "Assets/Data/GameData.xml";

    public GameData GameData { get; set; }//游戏数据
    public GameParam Config { get; set; }//配置
    public Player player;//当前玩家
    public bool playerStartMove;

    public bool isGameover;
    public bool isPause;
    public bool isFirstGame;

    public int score;
    public int diamond;

    /// <summary>
    /// 加载数据
    /// </summary>
    public void LoadData()
    {
        Config = GameParam.Get();
        Load();
    }

    /// <summary>
    /// 初始化游戏（点击开始游戏后）
    /// </summary>
    public void InitGame()
    {
        //初始化游戏数据
        player = null;
        score = 0;
        diamond = 0;
        isPause = false;
        playerStartMove = false;
        isGameover = false;
        //初始化地图数据
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
            DestroyImmediate(player.gameObject);
        }
    }

    /// <summary>
    /// 游戏结束
    /// </summary>
    public void Gameover()
    {
        isGameover = true;
        UIMgr.Ins.Show(Const.gameoverPanelPath);
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
    /// 花费钻石
    /// </summary>
    public void CostDiamond(int cost)
    {
        diamond -= cost;
        Save();
    }

    /// <summary>
    /// 解锁皮肤
    /// </summary>
    public void UnlockSkin(int index)
    {
        GameData.isUnlock[index] = true;
        Save();
        MsgSystem.Dispatch(MsgConst.BuySkin);
    }

    /// <summary>
    /// 选择皮肤
    /// </summary>
    public void ChooseSkin(int index)
    {
        GameData.curSelecteSkin = index;
        Save();
        MsgSystem.Dispatch(MsgConst.ChooseSkin);
    }

    /// <summary>
    /// 保存存档
    /// </summary>
    public void Save()
    {
        FileStream fileStream = new FileStream(GameDataPath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
        StreamWriter streamWriter = new StreamWriter(fileStream, System.Text.Encoding.UTF8);
        XmlSerializer xmlSerializer = new XmlSerializer(GameData.GetType());
        xmlSerializer.Serialize(streamWriter, GameData);
        streamWriter.Close();
        fileStream.Close();
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 加载存档
    /// </summary>
    private void Load()
    {
        if (!File.Exists(GameDataPath))
        {
            GameData = new GameData();
            GameData.Init();
            isFirstGame = true;
            Save();
        }
        else
        {
            FileStream fileStream = new FileStream(GameDataPath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(GameData));
            GameData = (GameData)xmlSerializer.Deserialize(fileStream);
            fileStream.Close();
        }
    }

    /// <summary>
    /// 清空存档
    /// </summary>
    public void ClearData()
    {
        GameData.Init();
        Save();
    }
}
