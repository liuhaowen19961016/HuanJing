using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform playerTrans;
    private Vector3 offset;

    private Vector3 velocity;

    private void Awake()
    {
        MsgSystem.AddListener(MsgConst.StartGame, Init);
    }

    private void Init()
    {
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
        offset = playerTrans.position - transform.position;
    }

    private void FixedUpdate()
    {
        if (playerTrans == null || GameMgr.Ins.isGameover)
        {
            return;
        }
        Vector3 targetPos = Vector3.SmoothDamp(transform.position, playerTrans.position - offset, ref velocity, 0.05f);
        if (targetPos.y > transform.position.y)
        {
            transform.position = targetPos;
        }
    }

    private void OnDestroy()
    {
        MsgSystem.RemoveListener(MsgConst.StartGame, Init);
    }
}
