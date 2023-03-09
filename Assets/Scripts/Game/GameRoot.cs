using UnityEngine;

public class GameRoot : MonoBehaviour
{
    void Awake()
    {
        GameMgr.Ins.LoadData();
        UIMgr.Ins.SetCanvasScaler(new Vector2(1080, 1920), true);
    }

    void Start()
    {
        UIMgr.Ins.Open<UI_Win_Start>();
    }
}
