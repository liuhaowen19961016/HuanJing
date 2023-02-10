using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGroup : Platform
{
    public SpriteRenderer[] sr_barriar;
    public GameObject barriar;
    public bool randomDir;

    public override void Init()
    {
        base.Init();

        foreach (var sr in sr_barriar)
        {
            sr.sprite = GameMgr.Ins.GetRandomSprite_Barriar();
        }

        if (randomDir && barriar != null)
        {
            bool isRight = Random.Range(0, 2) == 0;
            if (isRight)
            {
                barriar.transform.localPosition = initPosRight;
            }
            else
            {
                barriar.transform.localPosition = initPosLeft;
            }
        }
    }

    public override void Put()
    {
      if(randomDir)
        {
            Spawner.Ins.platformGroup1Pool.Put(gameObject);
        }
        else 
        {
            Spawner.Ins.platformGroup2Pool.Put(gameObject);
        }
    }
}
