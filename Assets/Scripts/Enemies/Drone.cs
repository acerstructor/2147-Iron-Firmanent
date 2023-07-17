using UnityEngine;


/// <summary>
/// This is the parent class of the drone enemy
/// and the main foundation of the game to be more
/// entertaining
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public abstract class Drone : MonoBehaviour
{
    [SerializeField] protected float _health;
    [SerializeField] protected float _moveSpeed;
    [SerializeField] protected int _points;

    protected bool _isDead;
    protected bool _isMoving;

    public abstract void Move();

    public virtual void Die()
    {
        _isDead = true;
        _isMoving = false;
        gameObject.SetActive(false);
    }

    public virtual void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public float GetHealth() { return _health; }
    public int GetPoints() { return _points; }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Colliding!");
        if (collision.gameObject.tag == "PlayerBullet")
        {
            //
            // TODO: Color Effects
            //
            _health--;
        }
    }
}
