using UnityEngine;

public class Test : MonoBehaviour
{
    private GameObjectPool bulletPool;

    private GameObject bullet;

    private void Awake()
    {
        bulletPool = ObjectPoolMgr.Ins.GetOrCreateGameObjectPool(Resources.Load<GameObject>("Prefabs/Bullet"), 10, transform);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            bullet = bulletPool.Get();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            bulletPool.Put(bullet);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            bulletPool.PutAll();
        }
    }
}
