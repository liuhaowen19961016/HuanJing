using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    public Rigidbody2D rigid;//刚体组件

    private GameObject go_curPlatform;
    private bool isPressRight;//是否按下屏幕右边
    private bool canMove;//能否移动

    private GameObjectPool dieEffectPool;

    private void Awake()
    {
        Init();

        dieEffectPool = ObjectPoolMgr.Ins.GetOrCreateGameObjectPool(GameMgr.Ins.config.go_dieEffect, 10, transform);
    }

    private void Init()
    {
        rigid.gravityScale = 0;
        canMove = true;
    }

    private void Update()
    {
        if (transform.position.y - Camera.main.transform.position.y < -6)
        {
            Debug.Log("平台落下");
            Gameover();
        }
        if (go_curPlatform != null && go_curPlatform.GetComponent<Platform>() != null && go_curPlatform.GetComponent<Platform>().isFall)
        {
            rigid.gravityScale = 1;
            GameMgr.Ins.isGameover = true;
            canMove = false;
        }
        if (Input.GetMouseButtonDown(0) && canMove && !GameMgr.Ins.isPause && !GameMgr.Ins.isGameover && !UIUtils.IsPointOverUI())
        {
            canMove = false;
            Vector2 mousePos = Input.mousePosition;
            isPressRight = mousePos.x >= Screen.width / 2;
            MoveNext();
        }
    }

    /// <summary>
    /// 移动到下一个位置
    /// </summary>
    private void MoveNext()
    {
        Vector3 curPlatformPos = go_curPlatform.transform.position;
        Vector3 nextPos = Vector3.zero;
        if (isPressRight)
        {
            transform.localScale = new Vector3(1, 1, 1);
            nextPos = curPlatformPos + new Vector3(GameMgr.Ins.config.platformOffset.x, GameMgr.Ins.config.platformOffset.y);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
            nextPos = curPlatformPos + new Vector3(-GameMgr.Ins.config.platformOffset.x, GameMgr.Ins.config.platformOffset.y);
        }
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(nextPos + new Vector3(0, 0.8f, 0), 0.05f).SetEase(Ease.Linear));
        sequence.Append(transform.DOMove(nextPos + new Vector3(0, 0.6f, 0), 0.01f).SetEase(Ease.Linear));
        sequence.OnComplete(() =>
        {
            FinishMove();
        });
    }

    /// <summary>
    /// 结束移动
    /// </summary>
    private void FinishMove()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position - transform.up * 0.5f, -transform.up, 0.1f);
        //跳空
        if (hitInfo.collider == null)
        {
            Debug.Log("跳空");
            Gameover();
        }
        //障碍
        else if (hitInfo.collider != null && hitInfo.collider.tag.Equals("Barriar"))
        {
            GameObject effect = dieEffectPool.Get();
            TimerMgr.Ins.Register(0.5f, onComplete: () =>
              {
                  dieEffectPool.Put(effect);
              });
            effect.transform.position = transform.position;
            effect.SetActive(true);
            Destroy(gameObject);
            Gameover();
            Debug.Log("障碍");
        }
        else
        {
            Debug.Log("正常移动");
            canMove = true;
            GameMgr.Ins.score++;
            MsgSystem.Dispatch(MsgConst.PlayerStartMove);
            MsgSystem.Dispatch(MsgConst.MoveFinish);
        }
    }

    private void Gameover()
    {
        rigid.gravityScale = 1;
        canMove = false;
        MsgSystem.Dispatch(MsgConst.Gameover);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Platform")
        {
            go_curPlatform = collision.gameObject;
        }
        if (collision.tag == "Diamond")
        {
            Spawner.Ins.diamondPool.Put(collision.gameObject);
            GameMgr.Ins.EatDiamond();
        }
    }
}
