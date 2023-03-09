using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Win_Gameover : BaseUI
{
    public Button btn_replay;
    public Button btn_menu;
    public Text txt_curScore;
    public Text txt_curDiamond;
    public Text txt_bestScore;

    private void Awake()
    {
        btn_menu.onClick.AddListener(() =>
        {
            UIMgr.Ins.Close<UI_Win_Gameover>(true);
            UIMgr.Ins.Close<UI_Win_Game>(true);
            UIMgr.Ins.Open<UI_Win_Start>();
            GameMgr.Ins.ResetGame();
        });
        btn_replay.onClick.AddListener(() =>
        {
            UIMgr.Ins.Close<UI_Win_Gameover>(true);
            UIMgr.Ins.Close<UI_Win_Game>(true);
            //UIMgr.Ins.Open<UI_Win_Game>();
            GameMgr.Ins.ResetGame();
            GameMgr.Ins.InitGame();
        });
    }

    public override void OnView()
    {
        base.OnView();

        txt_curScore.text = $"当前分数：{GameMgr.Ins.score}";
        txt_curDiamond.text = $"当前钻石：{GameMgr.Ins.diamond}";
        txt_bestScore.text = $"最佳：{GameMgr.Ins.GameData.bestScore}";
        if (GameMgr.Ins.score > GameMgr.Ins.GameData.bestScore)
        {
            GameMgr.Ins.GameData.bestScore = GameMgr.Ins.score;
        }
        GameMgr.Ins.GameData.totalDimaond += GameMgr.Ins.diamond;
        GameMgr.Ins.Save();
    }
}
