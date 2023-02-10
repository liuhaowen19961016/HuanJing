using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class GameParam : ScriptableObject
{
    public static GameParam Get()
    {
        return Resources.Load<GameParam>("Configs/GameParam");
    }

    public Sprite[] gameBg;//游戏背景
    public Vector3 initSpawnPos_platform = new Vector2(0, -2.4f);//初始化生成的平台位置
    public Vector3 initSpawnPos_player = new Vector2(0, -1.8f);//初始化生成的人物位置
    public Vector3 platformOffset = new Vector3(0.554f, 0.645f, 0);
    public GameObject go_platform;//平台预制体
    public GameObject go_platformGruop1;//平台组1预制体
    public GameObject go_platformGruop2;//平台组2预制体
    public GameObject go_platformSpike;//平台钉子预制体
    public GameObject go_dieEffect;//死亡特效预制体
    public GameObject go_diamond;//钻石预制体
    public GameObject go_player;//玩家预制体
    public int groupCountMax = 6;//每个平台组最大数量
    public int groupCountMin = 3;//每个平台组最小数量
    public Sprite[] grassPlatform;
    public Sprite[] winterPlatform;
    public Sprite[] grassPlatform_barriar;
    public Sprite[] winterPlatform_barriar;
}
