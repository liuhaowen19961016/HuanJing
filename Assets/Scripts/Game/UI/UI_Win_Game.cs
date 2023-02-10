using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class UI_Win_Game : MonoBehaviour
{
    public Button btn_pause;
    public Text txt_score;
    public Text txt_diamond;
    public SpriteRenderer img_bg;

    private void Awake()
    {
        gameObject.SetActive(false);

        btn_pause.onClick.AddListener(() =>
        {
            GameMgr.Ins.isPause = !GameMgr.Ins.isPause;
            if (GameMgr.Ins.isPause)
            {
                btn_pause.GetComponent<Image>().sprite = Resources.LoadAll<Sprite>("UIs/UI2")[0];
            }
            else
            {
                btn_pause.GetComponent<Image>().sprite = Resources.LoadAll<Sprite>("UIs/UI1")[4];
            }
        });

        MsgSystem.AddListener(MsgConst.StartGame, StartGame);
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

    private void StartGame()
    {
        gameObject.SetActive(true);

        img_bg.sprite = GameMgr.Ins.config.gameBg[Random.Range(0, GameMgr.Ins.config.gameBg.Length)];
        txt_score.text = GameMgr.Ins.score.ToString();
        txt_diamond.text = GameMgr.Ins.diamond.ToString();
    }

    private void OnDestroy()
    {
        MsgSystem.RemoveListener(MsgConst.StartGame, StartGame);
        MsgSystem.AddListener(MsgConst.MoveFinish, OnMoveFinish);
        MsgSystem.RemoveListener(MsgConst.EatDiamond, OnEatDiamond);
    }
}
