using System.Collections.Generic;
using UnityEngine;

public class BulletManager : Singleton<BulletManager>
{
    public Bullet[] _bullets;

    private void Start()
    {
        _bullets = Resources.FindObjectsOfTypeAll<Bullet>();
        
    }
    private void Update()
    {
        foreach (var bullet in _bullets)
        {
            if (!bullet.gameObject.activeInHierarchy) continue;

            bullet.Move();
        }
    }
}