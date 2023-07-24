using UnityEngine;

public class ShooterDrone : Drone
{
    [SerializeField] protected AnimationCurve _curve;
    [SerializeField] protected float _targetPosY;
    [SerializeField] protected float _shootCoolDownMax;

    [SerializeField] protected float _shootingDuration;
    [SerializeField] protected float _sprintBreakDuration;
    [SerializeField] protected float _slowSpeed;

    protected float _shootCoolDown = 2f;
    protected bool _shootingEnabled;
    protected Vector3 _targetPos;
    protected Vector3 _currentPos;

    private bool _hasReachedDestination = false;

    protected float _current;

    private static readonly int Flying = Animator.StringToHash("Flying");
    private static readonly int sprinting = Animator.StringToHash("Sprinting");
    private static readonly int SprintBreak = Animator.StringToHash("SprintBreak");
    private static readonly int Shooting = Animator.StringToHash("Shooting");

    protected bool _shooting, _sprinting, _sprintBreak;

    private void OnEnable()
    {
        _hasReachedDestination = false;
        _currentHealth = _maxHealth;
        _isMoving = true;
        _shootingEnabled = false;
        _sprinting = true;
        _currentPos = transform.position;
        _targetPos = new Vector3(_currentPos.x, _targetPosY, 0f);
    }

    private void OnDisable()
    {
        _hasReachedDestination = false;
        _current = 0f;
    }

    protected override int GetState()
    {
        if (Time.time < _lockedTill) return _currentState;

        // Priorities
        if (_shooting) return LockState(Shooting, _shootingDuration);
        if (_sprintBreak) return LockState(SprintBreak, _sprintBreakDuration);
        if (_sprinting) return sprinting;
        return Flying;

        int LockState(int s, float t)
        {
            _lockedTill = Time.time + t;
            return s;
        }
    }

    public virtual void Shoot()
    {
        if (!_shootingEnabled) return;

        if (_shootCoolDown > 0f)
        {
            _shootCoolDown -= Time.deltaTime;
            return;
        }

        AudioManager.Instance.PlaySoundEffect("EnemyShoot_0", SfxType.SHOOT);

        _shooting = true;
        ObjectPool.Instance.SpawnFromPool("EnemyLockedBullet", transform.position, Quaternion.identity);
        _shootCoolDown = _shootCoolDownMax;
    }

    public override void Animate()
    {
        base.Animate();

        _shooting = false;
        _sprintBreak = false;
    }
    public override void Move()
    {
        if (!_isMoving) return;

        if (!_hasReachedDestination)
        {
            _current = Mathf.MoveTowards(_current, 1f, _moveSpeed * Time.deltaTime); 
            Vector3 currentPos = Vector3.Lerp(_currentPos, _targetPos, _curve.Evaluate(_current));
            transform.position = currentPos;
        }

        var destination = Vector2.Distance(transform.position, _targetPos);

        if (!_hasReachedDestination && destination == 0)
        {
            Debug.Log("Stopped!");
            _sprintBreak = true;
            _sprinting = false;
            _hasReachedDestination = true;
            transform.Translate(Vector3.down * _slowSpeed * Time.deltaTime);
        }

        if (_hasReachedDestination)
        {
            transform.Translate(Vector3.down * _slowSpeed * Time.deltaTime);
            _shootingEnabled = true;
        }
    }

    public override void Die()
    {
        _isMoving = false;
        _shootingEnabled = false;

        ObjectPool.Instance.SpawnFromPool("Bubble_200", transform.position, Quaternion.identity);

        base.Die();
    }
    public override void Deactivate()
    {
        // Reset
        _shootCoolDown = 0f;
        // Deactivate this drone
        _hasReachedDestination = false;
        base.Deactivate();
    }
}
