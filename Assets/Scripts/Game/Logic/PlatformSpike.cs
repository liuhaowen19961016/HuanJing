using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpike : Platform
{
    public GameObject spikePlatform;

    public override void Init()
    {
        base.Init();
    }

    public void InitSpike(bool isRight)
    {
        if (isRight)
        {
            spikePlatform.transform.localPosition = initPosRight;
        }
        else
        {
            spikePlatform.transform.localPosition = initPosLeft;
        }
    }

    public override void Put()
    {
        Spawner.Ins.platformGroup3Pool.Put(gameObject);
    }
}
