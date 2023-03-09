using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_Win_Shop : BaseUI
{
    public ScrollRect m_sr;
    public Button btn_choose;
    public Button btn_back;
    public Text txt_name;
    public Text txt_diamond;

    private float[] normalizePos;
    private int m_CurIndex;
    private List<Item_Skin> m_SkinItems = new List<Item_Skin>();

    private void Awake()
    {
        btn_back.onClick.AddListener(() =>
        {
            UIMgr.Ins.Close(Const.shopPanelPath, true);
        });
        btn_choose.onClick.AddListener(OnChooseBtn);

        MsgSystem.AddListener(MsgConst.BuySkin, OnBuySkin);
    }

    public override void OnView()
    {
        base.OnView();

        RefreshView();
    }

    private void Start()
    {
        m_CurIndex = 0;
        normalizePos = new float[GameMgr.Ins.Config.skins.Length];
        float tempNormalizePos = 0;
        float normalizePosOffset = 1f / (GameMgr.Ins.Config.skins.Length - 1);
        for (int i = 0; i < normalizePos.Length; i++)
        {
            normalizePos[i] = tempNormalizePos;
            tempNormalizePos += normalizePosOffset;
        }
    }

    private void OnBuySkin()
    {
        RefreshView();
    }

    private void Update()
    {
        m_CurIndex = FindNearlyPosIndex();
        SetScale();
        SetName();
        RefreshChooseBtn();
        if (Input.GetMouseButtonUp(0))
        {
            m_sr.DOHorizontalNormalizedPos(normalizePos[m_CurIndex], 0.3f).OnComplete(() =>
            {

            });
        }
    }

    /// <summary>
    /// 刷新选择按钮
    /// </summary>
    private void RefreshChooseBtn()
    {
        bool isUnlock = GameMgr.Ins.GameData.isUnlock[m_CurIndex];
        if (isUnlock)
        {
            btn_choose.GetComponentInChildren<Text>().text = "使用";
        }
        else
        {
            btn_choose.GetComponentInChildren<Text>().text = $"解锁 {GameMgr.Ins.Config.unlockPrice[m_CurIndex]}";
        }
    }

    /// <summary>
    /// 设置缩放
    /// </summary>
    private void SetScale()
    {
        for (int i = 0; i < m_sr.content.transform.childCount; i++)
        {
            float v = 1;
            if (m_CurIndex != i)
            {
                v = 0.6f;
            }
            if (m_SkinItems.Count >= i + 1)
            {
                m_SkinItems[i].SetScale(v);
            }
        }
    }

    /// <summary>
    /// 设置名字
    /// </summary>
    private void SetName()
    {
        txt_name.text = GameMgr.Ins.Config.skinNames[m_CurIndex];
    }

    /// <summary>
    /// 查找最近的皮肤下标
    /// </summary>
    private int FindNearlyPosIndex()
    {
        float minDist = float.MaxValue;
        int minIndex = -1;
        for (int i = 0; i < normalizePos.Length; i++)
        {
            float tempDist = Mathf.Abs(normalizePos[i] - m_sr.normalizedPosition.x);
            if (tempDist < minDist)
            {
                minDist = tempDist;
                minIndex = i;
            }
        }
        return minIndex;
    }

    /// <summary>
    /// 刷新界面
    /// </summary>
    private void RefreshView()
    {
        txt_diamond.text = GameMgr.Ins.GameData.totalDimaond.ToString();
        for (int i = 0; i < GameMgr.Ins.Config.skins.Length; i++)
        {
            Item_Skin itemSkin = null;
            if (m_SkinItems.Count >= i + 1)
            {
                itemSkin = m_SkinItems[i];
            }
            else
            {
                GameObject go = Instantiate(GameMgr.Ins.Config.go_skin);
                go.transform.SetParent(m_sr.content, false);
                itemSkin = go.GetComponent<Item_Skin>();
            }
            itemSkin.Set(GameMgr.Ins.Config.skins[i]);
            itemSkin.SetGray(!GameMgr.Ins.GameData.isUnlock[i]);
        }
        RefreshChooseBtn();
    }

    private void OnDestroy()
    {
        MsgSystem.RemoveListener(MsgConst.BuySkin, OnBuySkin);
    }

    /// <summary>
    /// 按下选择按钮
    /// </summary>
    private void OnChooseBtn()
    {
        bool isUnlock = GameMgr.Ins.GameData.isUnlock[m_CurIndex];
        if (!isUnlock)
        {
            if (GameMgr.Ins.GameData.totalDimaond >= GameMgr.Ins.Config.unlockPrice[m_CurIndex])
            {
                GameMgr.Ins.CostDiamond(GameMgr.Ins.Config.unlockPrice[m_CurIndex]);
                GameMgr.Ins.UnlockSkin(m_CurIndex);
            }
            else
            {
                Tooltip.Create("钻石不足", transform.position)
                .SetAutoCloseSec(2)
                .SetIsTouchAnyClose(true)
                .Show();
            }
        }
        else
        {
            GameMgr.Ins.ChooseSkin(m_CurIndex);
            UIMgr.Ins.Close(Const.shopPanelPath, true);
        }
    }
}
