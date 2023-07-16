using UnityEngine;

public abstract class Drone : MonoBehaviour
{

    [SerializeField] protected float _health;
    [SerializeField] protected float _moveSpeed;
    [SerializeField] protected int _points;

    public abstract void Move();

    public virtual void Die()
    {
        //
        // TO DO: Sound Effects, Explosions
        //

        gameObject.SetActive(false);
    }

    public abstract void Spawn();

    public float GetHealth() { return _health; }
    public int GetPoints() { return _points; }
}
