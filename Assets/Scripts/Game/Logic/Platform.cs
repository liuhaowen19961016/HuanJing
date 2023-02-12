using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public SpriteRenderer[] sr_common;
    private Rigidbody2D rigid;

    protected Vector3 initPosRight = new Vector3(1.09f, 0, 0);
    protected Vector3 initPosLeft = new Vector3(-1.09f, 0, 0);

    private float fallTime = 2;
    private bool beginTimer;
    public bool isFall = false;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();

        MsgSystem.AddListener(MsgConst.PlayerStartMove, OnPlayerStartMove);
    }

    public virtual void Init()
    {
        foreach (var sr in sr_common)
        {
            sr.sprite = GameMgr.Ins.platformSprite;
        }
    }

    private void OnPlayerStartMove()
    {
        if (!beginTimer)
        {
            beginTimer = true;
            TimerMgr.Ins.Register(fallTime, onComplete: () =>
            {
                isFall = true;
                beginTimer = false;
                rigid.gravityScale = 1;
                TimerMgr.Ins.Register(2, onComplete: () =>
                {
                    Put();
                });
            });
        }
    }

    public virtual void Put()
    {
        Spawner.Ins.platformPool.Put(gameObject);
    }

    private void ResetData()
    {
        beginTimer = false;
        rigid.gravityScale = 0;
        isFall = false;
    }

    private void OnDestroy()
    {
        MsgSystem.RemoveListener(MsgConst.PlayerStartMove, OnPlayerStartMove);
    }
}
