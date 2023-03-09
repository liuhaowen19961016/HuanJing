using UnityEngine;

public class GameRoot : MonoBehaviour
{
    void Awake()
    {
        GameMgr.Ins.LoadData();
        UIMgr.Ins.Init(new Vector2(1080, 1920), true);
    }

    void Start()
    {
        UIMgr.Ins.Show(Const.startPanelPath);
    }
}
