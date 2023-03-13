using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public Text txt;

    private void Start()
    {
        MsgSystem.AddListener<int>(MsgConst.StartGame, Test2);
        MsgSystem.RemoveListener(MsgConst.EatDiamond, Test1);
    }

    private void Update()
    {

    }

    private void Test1()
    {

    }

    private void Test2(int a)
    {

    }
}
