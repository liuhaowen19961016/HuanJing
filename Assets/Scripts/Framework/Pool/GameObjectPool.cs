using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏物体对象池
/// </summary>
public class GameObjectPool
{
    private string m_GoKey;//游戏物体唯一key
    private int m_Capacity;//容量
    public GameObject m_Prefab;//预制体
    private Transform m_Parent;//父物体
    public Stack<GameObject> m_GoStack = new Stack<GameObject>();//游戏物体栈

    /// <summary>
    /// 初始化
    /// </summary>
    public void Init(GameObject prefab, int capacity, Transform parent)
    {
        m_GoKey = prefab.name + "[pool]";
        m_Capacity = capacity;
        m_Prefab = prefab;
        m_Parent = parent;
        for (int i = 0; i < capacity; i++)
        {
            Instantiate();
        }
    }

    /// <summary>
    /// 实例化
    /// </summary>
    private GameObject Instantiate(bool isActive = false, bool addToStack = true)
    {
        GameObject go = null;
        go = GameObject.Instantiate(m_Prefab);
        go.transform.SetParent(m_Parent, false);
        go.name = m_GoKey;
        go.SetActive(isActive);
        if (addToStack)
        {
            m_GoStack.Push(go);
        }
        return go;
    }

    /// <summary>
    /// 从池子中取
    /// </summary>
    public GameObject Get()
    {
        GameObject go = null;
        if (m_GoStack.Count <= 0)
        {
            go = Instantiate(addToStack: false);
        }
        else
        {
            go = m_GoStack.Pop();
        }
        return go;
    }

    /// <summary>
    /// 放回池子
    /// </summary>
    public bool Put(GameObject go)
    {
        bool ret = false;
        if (go.name != m_GoKey)
        {
            Debug.LogError($"无法放入对象池，此游戏物体与池子中的游戏物体不是同一个GameObject，\n此游戏物体：{go.name}，池子中游戏物体：{m_Prefab.name}[pool]");
        }
        else
        {
            if (m_Capacity > 0 && m_GoStack.Count >= m_Capacity)
            {
                GameObject.Destroy(go);
            }
            else
            {
                go.SetActive(false);
                m_GoStack.Push(go);
                ret = true;
            }
        }
        return ret;
    }
}
