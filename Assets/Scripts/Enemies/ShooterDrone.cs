using UnityEngine;

public class ShooterDrone : Drone
{
    [SerializeField] protected AnimationCurve _curve;
    [SerializeField] protected float _targetPosY;
    [SerializeField] protected float _shootCoolDownMax;
        
    protected float _shootCoolDown = 0f;
    protected bool _isShooting;
    protected Vector3 _targetPos;
    protected Vector3 _currentPos;

    protected float _current;

    private void OnEnable()
    {
        _isShooting = false;
        _currentPos = transform.position;
        _targetPos = new Vector3(transform.position.x, _targetPosY, 0f);
    }

    public virtual void Shoot()
    {
        if (!_isShooting) return;

        if (_shootCoolDown > 0f)
        {
            _shootCoolDown -= Time.deltaTime;
            return;
        }

        ObjectPool.Instance.SpawnFromPool("EnemyLockedBullet", transform.position, Quaternion.identity);
        _shootCoolDown = _shootCoolDownMax;
     }

    public override void Move()
    { 
        _current = Mathf.MoveTowards(_current, 1f, _moveSpeed * Time.deltaTime);
        Vector3 currentPos = Vector3.Lerp(_currentPos, _targetPos, _curve.Evaluate(_current));
        transform.position = currentPos;

        var destination = Vector2.Distance(currentPos, _targetPos);

        if (destination <= 0)
        {
            _isShooting = true;
        }
    }

    public override void Die()
    {
        _isShooting = false;
        base.Die();
    }
    public override void Deactivate()
    {
        // Reset
        _shootCoolDown = 0f;
        base.Deactivate();
    }
}
