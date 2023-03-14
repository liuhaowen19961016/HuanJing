using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class UI_Win_Game : BaseUI
{
    public Button btn_pause;
    public Text txt_score;
    public Text txt_diamond;
    public SpriteRenderer img_bg;

    private void Awake()
    {
        btn_pause.onClick.AddListener(() =>
        {
            Debug.Log(GameMgr.Ins.isPause);
            GameMgr.Ins.isPause = !GameMgr.Ins.isPause;
            if (GameMgr.Ins.isPause)
            {
                btn_pause.GetComponent<Image>().sprite = Resources.LoadAll<Sprite>("UIs/UI1")[4];
                Time.timeScale = 0;
            }
            else
            {
                btn_pause.GetComponent<Image>().sprite = Resources.LoadAll<Sprite>("UIs/UI2")[0];
                Time.timeScale = 1;
            }
        });

        MsgSystem.AddListener(MsgConst.MoveFinish, OnMoveFinish);
        MsgSystem.AddListener(MsgConst.EatDiamond, OnEatDiamond);
    }

    private void OnMoveFinish()
    {
        txt_score.text = GameMgr.Ins.score.ToString();
    }

    private void OnEatDiamond()
    {
        txt_diamond.text = GameMgr.Ins.diamond.ToString();
    }

    public override void OnView()
    {
        base.OnView();
        img_bg = Camera.main.GetComponentInChildren<SpriteRenderer>();
        img_bg.sprite = GameMgr.Ins.Config.gameBg[Random.Range(0, GameMgr.Ins.Config.gameBg.Length)];
        txt_score.text = GameMgr.Ins.score.ToString();
        txt_diamond.text = GameMgr.Ins.diamond.ToString();
    }

    private void OnDestroy()
    {
        MsgSystem.RemoveListener(MsgConst.MoveFinish, OnMoveFinish);
        MsgSystem.RemoveListener(MsgConst.EatDiamond, OnEatDiamond);
    }
}
