using UnityEngine;

public class EnemyBullet : Bullet
{
    private Vector2 _moveDirection;

    public override void Move()
    {
        transform.Translate(_moveDirection * _moveSpeed * Time.deltaTime);
    }

    public void SetMoveDirection(Vector2 dir)
    {
        _moveDirection = dir;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            gameObject.SetActive(false);
        }
    }
}
