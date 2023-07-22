using UnityEngine;


/// <summary>
/// This is the parent class of the drone enemy
/// and the main foundation of the game to be more
/// entertaining
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public abstract class Drone : MonoBehaviour
{
    [SerializeField] protected int _maxHealth;
    [SerializeField] protected float _moveSpeed;
    [SerializeField] protected int _points;
    [SerializeField] protected Animator _animator;

    protected int _currentHealth;
    protected float _lockedTill;
    protected bool _isDead;
    protected bool _isMoving;
    protected int _currentState;

    public bool IsDead { get { return _isDead; } }
    public bool IsMoving { get { return _isMoving; } }
    public int GetPoints { get { return _points; } }

    public virtual void Animate()
    {
        var state = GetState();

        if (state == _currentState) return;
        _animator.CrossFade(state, 0, 0);
        _currentState = state;
    }

    protected abstract int GetState();

    public abstract void Move();

    public virtual void Die()
    {
        _isDead = true;
        _isMoving = false;

        ObjectPool.Instance.SpawnFromPool("Explosion", transform.position, Quaternion.identity);
        GameManager.Instance.PlayerScored();

        _isDead = false; // Quickly set to false
        Deactivate();
    }

    public virtual void Deactivate()
    {
        _isDead = false; // making sure every inherited game object change _isDead state to false
        gameObject.SetActive(false);
    }

    public float GetHealth() { return _currentHealth; }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Colliding!");
        if (collision.gameObject.tag == "PlayerBullet")
        {
            //
            // TODO: Color Effects
            //
            _currentHealth--;
        }
    }
}
