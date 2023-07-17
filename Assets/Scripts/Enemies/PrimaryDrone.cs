using UnityEngine;

public class PrimaryDrone : ShooterDrone
{
    public override void Shoot()
    {
        if (!_isShooting) return;

        if (_shootCoolDown > 0f)
        {
            _shootCoolDown -= Time.deltaTime;
            return;
        }

        ObjectPool.Instance.SpawnFromPool("EnemyPrimaryBullet", transform.position, Quaternion.identity);
        _shootCoolDown = _shootCoolDownMax;
    }

    public override void Move()
    {
        var distance = Vector2.Distance(transform.position, _targetPos);
        if (distance > 0) base.Move();


        transform.Translate(Vector3.down * 0.1f * Time.deltaTime);
    }
}
