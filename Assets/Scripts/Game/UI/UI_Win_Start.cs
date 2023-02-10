using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Win_Start : MonoBehaviour
{
    public Button btn_start;
    public Button btn_shop;
    public Button btn_rank;
    public Button btn_sound;

    private void Awake()
    {
        btn_start.onClick.AddListener(() =>
        {
            GameMgr.Ins.InitGame();
            gameObject.SetActive(false);
        });
        btn_shop.onClick.AddListener(() =>
        {

        });
        btn_rank.onClick.AddListener(() =>
        {

        });
        btn_sound.onClick.AddListener(() =>
        {

        });
    }

    GameObjectPool pool_p1;
    GameObjectPool pool_p2;
    List<GameObject> list_p1 = new List<GameObject>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            pool_p1 = ObjectPoolMgr.Ins.GetOrCreateGameObjectPool(Resources.Load<GameObject>("Prefabs/Platform"), 10, transform);
            pool_p2 = ObjectPoolMgr.Ins.GetOrCreateGameObjectPool(Resources.Load<GameObject>("Prefabs/Platform_Group1"), 10, transform);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            for (int i = 0; i < 11; i++)
            {
                GameObject go = pool_p1.Get();
                go.SetActive(true);
                list_p1.Add(go);
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            foreach (var temp in list_p1)
            {
                bool ret = pool_p2.Put(temp);
                Debug.Log(ret);
            }
            list_p1.Clear();
        }
    }
}
