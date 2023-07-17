using UnityEngine;

public class EnemyPrimaryBullet : EnemyBullet
{
    public override void Move()
    {
        transform.Translate(Vector2.down * _moveSpeed * Time.deltaTime);
    }
}
