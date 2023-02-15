using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Win_Gameover : MonoBehaviour
{
    public Button btn_replay;
    public Button btn_menu;
    public Text txt_curScore;
    public Text txt_curDiamond;
    public Text txt_bestScore;

    private void Awake()
    {
        gameObject.SetActive(false);

        btn_menu.onClick.AddListener(() =>
        {
            UIMgr.Ins.gameoverPanel.SetActive(false);
            UIMgr.Ins.gamePanel.SetActive(false);
            UIMgr.Ins.startPanel.SetActive(true);
            GameMgr.Ins.ResetGame();
        });
        btn_replay.onClick.AddListener(() =>
        {
            UIMgr.Ins.gameoverPanel.SetActive(false);
            UIMgr.Ins.gamePanel.SetActive(true);
            GameMgr.Ins.ResetGame();
            GameMgr.Ins.InitGame();
        });
    }

    private void OnEnable()
    {
        txt_curScore.text = $"当前分数：{GameMgr.Ins.score}";
        txt_curDiamond.text = $"当前钻石：{GameMgr.Ins.diamond}";
        txt_bestScore.text = $"最佳：{99999}";
    }
}
