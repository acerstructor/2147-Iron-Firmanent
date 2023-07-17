using UnityEngine;

public class BulletManager : Singleton<BulletManager>
{
    public Bullet[] _bullets;

    protected override void Awake()
    {
        base.Awake();
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