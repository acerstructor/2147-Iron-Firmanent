using UnityEngine;

/// <summary>
/// This is the parent class of the Bullet which is derived
/// to both player and enemy class
/// </summary>
public abstract class Bullet : MonoBehaviour
{
    [SerializeField] protected float _moveSpeed;
    private float _borderLimitX = 4f, _borderLimitAbove = 3.7f, _borderLimitBelow = -4.5f;

    private void Update()
    {
        CheckBorder();
    }

    public abstract void Move();

    private void CheckBorder()
    {
        var currentPosition = transform.position;

        if (currentPosition.x > _borderLimitX || currentPosition.x < -_borderLimitX) 
            gameObject.SetActive(false);
        
        if (currentPosition.y > _borderLimitAbove || currentPosition.y < _borderLimitBelow)
            gameObject.SetActive(false);
    }
}
