using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Win_Start : BaseUI
{
    public Button btn_start;
    public Button btn_shop;
    public Button btn_rank;
    public Button btn_sound;
    public Button btn_clearData;
    public Image img_skin;

    private void Awake()
    {
        btn_start.onClick.AddListener(() =>
        {
            UIMgr.Ins.Close<UI_Win_Start>();
            GameMgr.Ins.InitGame();
        });
        btn_shop.onClick.AddListener(() =>
        {
            UIMgr.Ins.Open<UI_Win_Shop>();
        });
        btn_rank.onClick.AddListener(() =>
        {

        });
        btn_sound.onClick.AddListener(() =>
        {

        });
        btn_clearData.onClick.AddListener(() =>
        {
            GameMgr.Ins.ClearData();
            RefreshView();
        });

        MsgSystem.AddListener(MsgConst.ChooseSkin, OnChooseSkin);
    }

    public override void OnView()
    {
        base.OnView();

        RefreshView();
    }

    private void OnChooseSkin()
    {
        RefreshView();
    }

    /// <summary>
    /// 刷新界面
    /// </summary>
    private void RefreshView()
    {
        img_skin.sprite = GameMgr.Ins.Config.skins[GameMgr.Ins.GameData.curSelecteSkin];
    }

    private void OnDestroy()
    {
        MsgSystem.RemoveListener(MsgConst.ChooseSkin, OnChooseSkin);
    }
}
