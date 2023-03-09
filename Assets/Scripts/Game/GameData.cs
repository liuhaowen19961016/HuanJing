using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    //public Dictionary<string, int> skinStateDict = new Dictionary<string, int>();//<皮肤下标，状态>（0：未购买 1：已购买 2：正使用）
    public int totalDimaond;//钻石数
    public int bestScore;//最大分数
    public bool[] isUnlock;//是否解锁
    public int curSelecteSkin;//选择的皮肤下标
    public bool musicIsOn;//音效是否打开

    public void Init()
    {
        musicIsOn = true;
        totalDimaond = 10;
        bestScore = 0;
        curSelecteSkin = 0;
        isUnlock = new bool[GameMgr.Ins.Config.skins.Length];
        isUnlock[curSelecteSkin] = true;
    }
}