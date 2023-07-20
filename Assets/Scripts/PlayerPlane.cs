using UnityEngine;

public class PlayerPlane : MonoBehaviour
{
    // For Entrance: 
    [SerializeField] private Vector3 _startingPos;
    [SerializeField] private Vector3 _targetPos;
    [SerializeField] private AnimationCurve _curve;
    [SerializeField] private float _entranceMoveSpeed;
    [SerializeField] private float _maxRecovery;
    [SerializeField] private Animator _animator;

    private float _current = 0f;
    private float _currentRecovery = 0f;

    private int _currentState;

    private bool _recovering;

    private static readonly int Alive = Animator.StringToHash("Alive");
    private static readonly int Recovering = Animator.StringToHash("Recovering");

    private void OnEnable()
    {
        transform.position = _startingPos;
        _recovering = false;
    }

    public void Recover()
    {
        var playerState = GameManager.Instance.PlayerState;

        if (playerState != PlayerState.RECOVERING) return;

        if (_currentRecovery > 0)
        {
            _currentRecovery -= Time.deltaTime;
            _recovering = true;
        }
        else
        {
            GameManager.Instance.SetPlayerState(PlayerState.ALIVE);
            _recovering = false;
        }
    }

    public void Animate() // I forgot to create IAnimated interface so screw it! >:(
    {
        var state = GetState();

        if (state == _currentState) return;

        _animator.CrossFade(state, 0, 0);
        _currentState = state;

    }

    protected int GetState()
    {
        // Priorities
        if (_recovering) return Recovering;
        return Alive;
    }

    public void Entrance()
    {
        var playerState = GameManager.Instance.PlayerState;

        if (playerState != PlayerState.ENTERING) return;
 
        gameObject.SetActive(true);

        _current = Mathf.MoveTowards(_current, 1f, _entranceMoveSpeed * Time.deltaTime);
        Vector3 currentPos = Vector3.Lerp(_startingPos, _targetPos, _curve.Evaluate(_current));       

        transform.position = currentPos;

        var distance = Vector2.Distance(transform.position, _targetPos);

        if (distance <= 0)
        {
            _currentRecovery = _maxRecovery;
            GameManager.Instance.SetPlayerState(PlayerState.RECOVERING);
            _current = 0f;
        }
    }

    public void Die()
    {
        var currentState = GameManager.Instance.PlayerState;

        if (currentState != PlayerState.DEAD) return;

        ObjectPool.Instance.SpawnFromPool("Explosion", transform.position, Quaternion.identity);

        GameManager.Instance.SetPlayerState(PlayerState.INACTIVE);

        Deactivate();
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "EnemyBullet")
        {
            PlayerState currentState = GameManager.Instance.PlayerState;
            if (currentState == PlayerState.ENTERING || currentState == PlayerState.DEAD || 
                currentState == PlayerState.RECOVERING)
                return;

            GameManager.Instance.SetPlayerState(PlayerState.DEAD);
        }
    }
}