using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMgr : MonoBehaviour
{
    public static UIMgr Ins;

    public GameObject startPanel;
    public GameObject gamePanel;
    public GameObject gameoverPanel;

    private void Awake()
    {
        Ins = this;
    }
}
