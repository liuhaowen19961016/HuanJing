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
            sr.sprite = Spawner.Ins.platformSprite;
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

    TimerTask task1;
    TimerTask task2;
    private void Fall()
    {
        task1 = TimerMgr.Ins.Register(fallTime,
                onRegister: () =>
                {
                    startTimer = true;
                },
                onComplete: () =>
                {
                    isFall = true;
                    rigid.gravityScale = 1;
                    task2 = TimerMgr.Ins.Register(2, onComplete: () =>
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
        if (task1 != null)
        {
            task1.Dispose();
        }
        if (task2 != null)
        {
            task2.Dispose();
        }
    }

    public override void Reset()
    {
        ResetData();
    }
}
