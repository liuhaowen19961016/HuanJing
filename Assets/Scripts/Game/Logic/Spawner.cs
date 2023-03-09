using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoSingleton<Spawner>
{
    public EGameTheme gameTheme { get; set; }//游戏主题
    public Sprite platformSprite { get; set; }//平台图
    private Vector3 nextSpawnPos;//下一个生成位置
    private int curGroupRemainCount;//当前组剩余数量
    private bool isRight;//是否向右生成
    private bool inSpikeSpawn;//是否在生成钉子
    private Vector3 nextSpawnPos_spikePlatform;//钉子后的平台位置

    //对象池
    public GameObjectPool platformPool;
    public GameObjectPool platformGroup1Pool;
    public GameObjectPool platformGroup2Pool;
    public GameObjectPool platformGroup3Pool;
    public GameObjectPool diamondPool;
    public GameObjectPool dieEffectPool;

    private void Awake()
    {
        MsgSystem.AddListener(MsgConst.MoveFinish, DecidePath);
    }

    /// <summary>
    /// 初始化地图
    /// </summary>
    public void InitMap()
    {
        platformPool = ObjectPoolMgr.Ins.GetOrCreateGameObjectPool(GameMgr.Ins.Config.go_platform, 10, transform);
        platformGroup1Pool = ObjectPoolMgr.Ins.GetOrCreateGameObjectPool(GameMgr.Ins.Config.go_platformGruop1, 10, transform);
        platformGroup2Pool = ObjectPoolMgr.Ins.GetOrCreateGameObjectPool(GameMgr.Ins.Config.go_platformGruop2, 10, transform);
        platformGroup3Pool = ObjectPoolMgr.Ins.GetOrCreateGameObjectPool(GameMgr.Ins.Config.go_platformSpike, 10, transform);
        diamondPool = ObjectPoolMgr.Ins.GetOrCreateGameObjectPool(GameMgr.Ins.Config.go_diamond, 10, transform);
        dieEffectPool = ObjectPoolMgr.Ins.GetOrCreateGameObjectPool(GameMgr.Ins.Config.go_dieEffect, 10, transform);

        InitData();

        int initCount = curGroupRemainCount;
        for (int i = 0; i < initCount; i++)
        {
            DecidePath();
        }
        SpawnPlayer();
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    private void InitData()
    {
        gameTheme = (EGameTheme)Random.Range(0, 2);
        platformSprite = GetRandomSprite_Common();
        inSpikeSpawn = false;
        isRight = true;
        nextSpawnPos = GameMgr.Ins.Config.initSpawnPos_platform;
        curGroupRemainCount = 5;//初始化生成5个平台
    }

    /// <summary>
    /// 判断路径方式
    /// </summary>
    private void DecidePath()
    {
        Debug.Log("DecidePath");
        curGroupRemainCount--;
        if (curGroupRemainCount < 0)
        {
            ReDecide();
        }
        SpawnPlatform();
    }

    private void ReDecide()
    {
        curGroupRemainCount = GetGroupCount();
        isRight = !isRight;
    }

    /// <summary>
    /// 生成平台
    /// </summary>
    private void SpawnPlatform()
    {
        if (inSpikeSpawn)
        {
            if (curGroupRemainCount > 0)
            {
                //普通
                GameObject platform1 = platformPool.Get();
                platform1.transform.SetParent(transform, false);
                platform1.transform.position = nextSpawnPos;
                platform1.GetComponent<Platform>().Init();
                platform1.gameObject.SetActive(true);
                if (isRight)
                {
                    nextSpawnPos += new Vector3(GameMgr.Ins.Config.platformOffset.x, GameMgr.Ins.Config.platformOffset.y);
                }
                else
                {
                    nextSpawnPos += new Vector3(-GameMgr.Ins.Config.platformOffset.x, GameMgr.Ins.Config.platformOffset.y);
                }

                //钉子
                GameObject platform2 = platformPool.Get();
                platform2.transform.SetParent(transform, false);
                platform2.transform.position = nextSpawnPos_spikePlatform;
                platform2.GetComponent<Platform>().Init();
                platform2.gameObject.SetActive(true);
                if (isRight)
                {
                    nextSpawnPos_spikePlatform += new Vector3(-GameMgr.Ins.Config.platformOffset.x, GameMgr.Ins.Config.platformOffset.y);
                }
                else
                {
                    nextSpawnPos_spikePlatform += new Vector3(GameMgr.Ins.Config.platformOffset.x, GameMgr.Ins.Config.platformOffset.y);
                }
            }
            else
            {
                inSpikeSpawn = false;
                DecidePath();
            }
            return;
        }

        GameObject go = null;
        //最后一个生成特殊平台
        if (curGroupRemainCount == 0)
        {
            int ranNum = Random.Range(0, 7);
            //通用
            if (ranNum == 0 || ranNum == 1 || ranNum == 2)
            {
                go = platformPool.Get();
                go.transform.SetParent(transform, false);
                go.transform.position = nextSpawnPos;
                go.GetComponent<Platform>().Init();
                go.SetActive(true);
            }
            //组合1（1-2 or 2-1）
            else if (ranNum == 3 || ranNum == 4)
            {
                go = platformGroup1Pool.Get();
                go.transform.SetParent(transform, false);
                go.transform.position = nextSpawnPos;
                go.GetComponent<PlatformGroup>().Init();
                go.SetActive(true);
            }
            //组合2（1-2-1）
            else if (ranNum == 5)
            {
                go = platformGroup2Pool.Get();
                go.transform.SetParent(transform, false);
                go.transform.position = nextSpawnPos;
                go.GetComponent<PlatformGroup>().Init();
                go.SetActive(true);
            }
            //钉子
            else if (ranNum == 6)
            {
                go = platformGroup3Pool.Get();
                go.transform.SetParent(transform, false);
                go.transform.position = nextSpawnPos;
                go.GetComponent<PlatformSpike>().Init();
                go.GetComponent<PlatformSpike>().InitSpike(!isRight);
                go.SetActive(true);

                curGroupRemainCount = Random.Range(1, 5);
                inSpikeSpawn = true;
                Vector3 curSpikePlatformPos = go.GetComponent<PlatformSpike>().spikePlatform.transform.position;
                if (isRight)
                {
                    nextSpawnPos_spikePlatform = curSpikePlatformPos + new Vector3(-GameMgr.Ins.Config.platformOffset.x, GameMgr.Ins.Config.platformOffset.y);
                }
                else
                {
                    nextSpawnPos_spikePlatform = curSpikePlatformPos + new Vector3(GameMgr.Ins.Config.platformOffset.x, GameMgr.Ins.Config.platformOffset.y);
                }
            }
        }
        else
        {
            go = platformPool.Get();
            go.transform.SetParent(transform, false);
            go.transform.position = nextSpawnPos;
            go.GetComponent<Platform>().Init();
            go.SetActive(true);

            if (Random.Range(0, 10) <= 3)
            {
                GameObject diamond = diamondPool.Get();
                diamond.transform.position = go.transform.position + Vector3.up * 0.5f;
                diamond.SetActive(true);
            }
        }
        if (isRight)
        {
            nextSpawnPos += new Vector3(GameMgr.Ins.Config.platformOffset.x, GameMgr.Ins.Config.platformOffset.y);
        }
        else
        {
            nextSpawnPos += new Vector3(-GameMgr.Ins.Config.platformOffset.x, GameMgr.Ins.Config.platformOffset.y);
        }
    }

    /// <summary>
    /// 生成玩家
    /// </summary>
    private void SpawnPlayer()
    {
        GameObject go = Instantiate(GameMgr.Ins.Config.go_player);
        go.transform.position = GameMgr.Ins.Config.initSpawnPos_player;
        GameMgr.Ins.player = go.GetComponent<Player>();
    }

    /// <summary>
    /// 得到随机组数量
    /// </summary>
    private int GetGroupCount()
    {
        return Random.Range(GameMgr.Ins.Config.groupCountMin, GameMgr.Ins.Config.groupCountMax + 1);
    }

    /// <summary>
    /// 得到随机的图（普通）
    /// </summary>
    private Sprite GetRandomSprite_Common()
    {
        switch (gameTheme)
        {
            case EGameTheme.Grass:
                return GameMgr.Ins.Config.grassPlatform[Random.Range(0, GameMgr.Ins.Config.grassPlatform.Length)];
            case EGameTheme.Winter:
                return GameMgr.Ins.Config.winterPlatform[Random.Range(0, GameMgr.Ins.Config.winterPlatform.Length)];
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
                return GameMgr.Ins.Config.grassPlatform_barriar[Random.Range(0, GameMgr.Ins.Config.grassPlatform_barriar.Length)];
            case EGameTheme.Winter:
                return GameMgr.Ins.Config.winterPlatform_barriar[Random.Range(0, GameMgr.Ins.Config.winterPlatform_barriar.Length)];
            default:
                return null;
        }
    }

    private void OnDestroy()
    {
        MsgSystem.RemoveListener(MsgConst.MoveFinish, DecidePath);
    }
}
