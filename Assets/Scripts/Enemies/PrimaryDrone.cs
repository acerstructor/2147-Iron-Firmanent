using UnityEngine;

public class PrimaryDrone : ShooterDrone
{
    private bool _hasReachedDestination = false; // Used to limiting lerp movement;

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

        if (!_hasReachedDestination)
        {
            if (distance > 0f)
            { 
                base.Move();
                return;
            }
            _hasReachedDestination = true;
        }

        transform.Translate(Vector3.down * 0.2f * Time.deltaTime);
    }
}
