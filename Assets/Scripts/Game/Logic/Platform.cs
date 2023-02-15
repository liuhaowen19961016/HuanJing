using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : PoolObject
{
    public SpriteRenderer[] sr_common;
    private Rigidbody2D rigid;

    protected Vector3 initPosRight = new Vector3(1.09f, 0, 0);
    protected Vector3 initPosLeft = new Vector3(-1.09f, 0, 0);

    private bool startTimer;
    private float fallTime = 2;
    public bool isFall = false;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public virtual void Init()
    {
        foreach (var sr in sr_common)
        {
            sr.sprite = GameMgr.Ins.platformSprite;
        }

        if (GameMgr.Ins.playerStartMove)
        {
            Fall();
        }
    }

    private void Update()
    {
        if (GameMgr.Ins.playerStartMove && !startTimer)
        {
            Fall();
        }
    }

    private void Fall()
    {
        TimerMgr.Ins.Register(fallTime,
                onRegister: () =>
                {
                    startTimer = true;
                },
                onComplete: () =>
                {
                    isFall = true;
                    rigid.gravityScale = 1;
                    TimerMgr.Ins.Register(2, onComplete: () =>
                    {
                        ResetData();
                        Put();
                    });
                });
    }

    public virtual void Put()
    {
        Spawner.Ins.platformPool.Put(gameObject);
    }

    public virtual void ResetData()
    {
        startTimer = false;
        rigid.gravityScale = 0;
        isFall = false;
    }

    public override void Reset()
    {
        TimerMgr.Ins.DisposeAll();
        ResetData();
    }
}
