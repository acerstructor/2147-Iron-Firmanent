using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _explosionDuration;
    [SerializeField] private float _moveDownSpeed;

    private float _lockedTill;
    private int _currentState;

    private static readonly int Explode = Animator.StringToHash("Explode");
    private static readonly int Vanish = Animator.StringToHash("Vanish");
    private bool _explode = false;

    private void OnEnable()
    {
        _explode = true;
    }

    public virtual void Animate()
    {
//        if (!gameObject.activeSelf) return;

        var state = GetState();

        if (state == _currentState) return;
        _explode = false;

        _animator.CrossFade(state, 0, 0);
        _currentState = state;
    }

    public virtual void Move()
    {
        transform.Translate(Vector3.down * _moveDownSpeed * Time.deltaTime);
    }

    protected virtual int GetState()
    {
        if (Time.time < _lockedTill) return _currentState;

        // Priorities
        if (_explode) return LockState(Explode, _explosionDuration);

        gameObject.SetActive(false);
        return Vanish;

        int LockState(int s, float t)
        {
            _lockedTill = Time.time + t;
            return s;
        }
    }
}