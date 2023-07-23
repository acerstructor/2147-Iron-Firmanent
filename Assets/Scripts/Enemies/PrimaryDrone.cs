using UnityEngine;

public class PrimaryDrone : ShooterDrone
{
    public override void Shoot()
    {
        if (!_shootingEnabled) return;

        if (_shootCoolDown > 0f)
        {
            _shootCoolDown -= Time.deltaTime;
            return;
        }

        AudioManager.Instance.PlaySoundEffect("EnemyShoot", SfxType.SHOOT);

        _shooting = true;
        ObjectPool.Instance.SpawnFromPool("EnemyPrimaryBullet", transform.position, Quaternion.identity);
        _shootCoolDown = _shootCoolDownMax;
    }
}
