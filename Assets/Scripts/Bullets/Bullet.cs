using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    private float _borderLimitX = 4f, _borderLimitY = 6f;

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
        
        if (currentPosition.y > _borderLimitY || currentPosition.y < -_borderLimitY)
            gameObject.SetActive(false);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy") gameObject.SetActive(false);
    }
}