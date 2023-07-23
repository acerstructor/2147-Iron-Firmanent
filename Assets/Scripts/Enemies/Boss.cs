using System.Collections;
using UnityEngine;

public partial class Boss : ShooterDrone
{
    [SerializeField] protected AnimationCurve _movementCurve;
    [SerializeField] protected Vector3[] _positions;
    [SerializeField] protected float _movementCooldownMax;
    [SerializeField] protected float _spiralBulletDurationMax;
    [SerializeField] protected float _spreadBulletDurationMax;
    [SerializeField] protected float _bulletHellCoolDownMax;
    [SerializeField] protected SpriteRenderer _spriteRenderer;
    [SerializeField] protected int _dyingDurationMax;
    [SerializeField] protected float _firingSoundDurationMax;

    private float _angle = 0f; // for Spiral Bullet Hell
    private float _startAngle = 90f, _endAngle = 270f; // for Spreading Bullet

    // Durations and Cooldowns:
    protected float _bulletHellDuration = 0f;
    protected float _bulletHellCoolDown = 0f;
    protected float _movementCoolDown = 0f;
    protected float _firingSoundDuration = 1f;
    protected bool _isMovementCoolDown = false;
    protected bool _isBulletHellCoolDown = false;
    protected bool _isDying = false;
    protected bool _bulletHell = false;
    protected BossMovePosition _movePos;

    private static readonly int Flying = Animator.StringToHash("Flying");
    private static readonly int Dying = Animator.StringToHash("Dying");
    private static readonly int BulletHell = Animator.StringToHash("BulletHell");

    private void OnEnable()
    {
        GameManager.Instance.SetLevelState(LevelState.BOSSBATTLE);

        _currentHealth = _maxHealth;
        _bulletHell = false;
        _isMoving = true;
        _isDying = false;
        _movePos = BossMovePosition.ENTRANCE;
        _currentPos = transform.position;
        _firingSoundDuration = 0;
        _targetPos = new Vector3(_currentPos.x, _targetPosY, 0);
    }

    protected void SetMoveDirection(float moveSpeed, Vector3 targetPos, BossMovePosition nextPos,
        AnimationCurve curve, bool isCooldown)
    {
        _current = Mathf.MoveTowards(_current, 1f, moveSpeed * Time.deltaTime);
        Vector3 currentPos = Vector3.Lerp(_currentPos, targetPos, curve.Evaluate(_current));
        transform.position = currentPos;

        var distance = Vector3.Distance(transform.position, targetPos);

        if (distance <= 0)
        {
            // Activate shooting before moving to next position
            if (_movePos == BossMovePosition.ENTRANCE)
                _shootingEnabled = true;

            _movePos = nextPos;
            _currentPos = transform.position;
            _current = 0;

            if (isCooldown)
            {
                _movementCoolDown = _movementCooldownMax;
                _isMovementCoolDown = true;
            }
        }
    }

    protected override int GetState()
    {
        // Priorities
        if (_isDying) return Dying;
        if (_bulletHell) return BulletHell;
        return Flying;
    }

    public override void Shoot()
    {
        if (!_shootingEnabled) return;
        if (_movePos == BossMovePosition.ENTRANCE) return;
        
        CheckBulletDuration("FireSpiral");
        CheckBulletDuration("FireSpread");
        CheckBulletHellCoolDown();

        if (_shootCoolDown > 0)
            _shootCoolDown -= Time.deltaTime;
        else
        {
            AudioManager.Instance.PlaySoundEffect("EnemyShoot_0", SfxType.SHOOT);
            ObjectPool.Instance.SpawnFromPool("EnemyLockedBullet", transform.position, Quaternion.identity);
            _shootCoolDown = _shootCoolDownMax;
        }

        if (_isMovementCoolDown)
        {
            if (IsInvoking("FireSpread")) return;

            // Fire Spiral Bullet Hell:
            if (!_isBulletHellCoolDown && !IsInvoking("FireSpiral"))
            {
                InvokeRepeating("FireSpiral", 0f, 0.1f);
                _bulletHellDuration = _spiralBulletDurationMax;
            }
        }
        else
        {
            if (IsInvoking("FireSpiral")) return;

            // Fire Spread Nullet Hell:
            if (!_isBulletHellCoolDown && !IsInvoking("FireSpread"))
            {
                InvokeRepeating("FireSpread", 0f, 2f);
                _bulletHellDuration = _spreadBulletDurationMax; 
            }
        }
    }

    protected void CheckBulletDuration(string name)
    {
        if (_bulletHellDuration > 0)
        {
            _bulletHellDuration -= Time.deltaTime;
            _bulletHell = true;
            return;
        }

        if (IsInvoking(name))
        {
            _bulletHellCoolDown = _bulletHellCoolDownMax;
            _isBulletHellCoolDown = true;
            CancelInvoke(name);
            _bulletHell = false;
        }
    }

    protected void CheckBulletHellCoolDown()
    {
        if (_bulletHellCoolDown > 0)
        {
            _bulletHellCoolDown -= Time.deltaTime;
            return;
        }

        _isBulletHellCoolDown = false;
    }

    /// <summary>
    /// This builds a spiral bullet hell which is 
    /// fired from the boss. Should be always put
    /// in invoke functions!
    /// </summary>
    protected void FireSpiral()
    {
        for (int i = 0; i <= 1; i++)
        {
            float bulDirX = transform.position.x + Mathf.Sin(((_angle + 180f * i) * Mathf.PI) / 180f);
            float bulDirY = transform.position.y + Mathf.Cos(((_angle + 180f * i) * Mathf.PI) / 180f);

            Vector3 bulMoveVector = new Vector3(bulDirX, bulDirY, 0f);
            Vector2 bulDir = (bulMoveVector - transform.position).normalized;

            GameObject bullet = ObjectPool.Instance.SpawnFromPool("EnemyBullet", transform.position, transform.rotation);

            bullet.GetComponent<EnemyBullet>().SetMoveDirection(bulDir);
        }

        _angle += 10f;

        if (_angle >= 360f) _angle = 0f; // reset to 0

        BulletHellSound();
    }

    private void BulletHellSound()
    {
        if (_firingSoundDuration > 0)
        {
            _firingSoundDuration -= Time.deltaTime;
            return;
        }

        AudioManager.Instance.PlaySoundEffect("BulletHellShoot", SfxType.SHOOT);
        _firingSoundDuration = _firingSoundDurationMax;
    }

    protected void FireSpread()
    {
        int bulletAmount = 10;
        float angleStep = (_endAngle - _startAngle) / bulletAmount;
        float angle = _startAngle;

        for (int i = 0; i <= bulletAmount; i++)
        {
            float bulDirX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180f);
            float bulDirY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180f);

            Vector3 bulMoveVector = new Vector3(bulDirX, bulDirY, 0f);
            Vector2 bulDir = (bulMoveVector - transform.position).normalized;

            GameObject bullet = ObjectPool.Instance.SpawnFromPool("EnemyBullet", transform.position, transform.rotation);
            bullet.GetComponent <EnemyBullet>().SetMoveDirection(bulDir);

            angle += angleStep;
            BulletHellSound();
        }

    }

    public override void Move()
    {
        if (!_isMoving) return;

        if (_movementCoolDown > 0)
        {
            _movementCoolDown -= Time.deltaTime;
            return;
        }

        _isMovementCoolDown = false;

        switch (_movePos)
        {
            case BossMovePosition.ENTRANCE:
                SetMoveDirection(0.2f, _targetPos, BossMovePosition.ORIGINALPOS, _curve, true);
                break;
            case BossMovePosition.ORIGINALPOS:
                SetMoveDirection(_moveSpeed, _positions[1], BossMovePosition.HORIZONTALRIGHT,
                    _movementCurve, false);
                break;
            case BossMovePosition.HORIZONTALRIGHT:
                SetMoveDirection(_moveSpeed, _positions[2], BossMovePosition.VERTICALRIGHT,
                    _movementCurve, false);
                break;
            case BossMovePosition.VERTICALRIGHT:
                SetMoveDirection(_moveSpeed, _positions[3], BossMovePosition.HORIZONTALLEFT,
                    _movementCurve, false);
                break;
            case BossMovePosition.HORIZONTALLEFT:
                SetMoveDirection(_moveSpeed, _positions[4], BossMovePosition.VERTICALLEFT,
                    _movementCurve, false);
                break;
            case BossMovePosition.VERTICALLEFT:
                SetMoveDirection(_moveSpeed, _positions[0], BossMovePosition.ORIGINALPOS,
                    _movementCurve, true);
                break;
        }  
    }

    public override void Die()
    {
        if (_isDying) return;
        _isDying = true;

        GameManager.Instance.SetLevelState(LevelState.BOSSWIN);

        _shootingEnabled = false;
        _isMoving = false;
        if (IsInvoking("FireSpiral")) CancelInvoke("FireSpiral");
        if (IsInvoking("FireSpread")) CancelInvoke("FireSpread");

        StartCoroutine(Exploding());
    }

    public override void Deactivate()
    {
        // Reset all Attributes
        _bulletHellDuration = 0f;
        _bulletHellCoolDown = 0f;
        _isBulletHellCoolDown = false;
        _isMovementCoolDown = false;
        _movementCoolDown = 0f;
        _bulletHellDuration = 0f;
        _angle = 0f;

        _isDead = true;
        GameManager.Instance.PlayerScored();
        base.Deactivate();
    }

    private IEnumerator Exploding()
    {
        int i = 0;
        while (i < _dyingDurationMax)
        {
            float posX = Random.Range(_spriteRenderer.bounds.min.x, _spriteRenderer.bounds.max.x);
            float posY = Random.Range(_spriteRenderer.bounds.min.y, _spriteRenderer.bounds.max.y);
            Vector2 pos = new Vector2(posX, posY);
            ObjectPool.Instance.SpawnFromPool("Explosion", pos, Quaternion.identity);
            ObjectPool.Instance.SpawnFromPool("Bubble_100", pos, Quaternion.identity);
            i++;
            yield return new WaitForSeconds(0.5f);
        }

        Deactivate();
        yield return null;
    }


}
