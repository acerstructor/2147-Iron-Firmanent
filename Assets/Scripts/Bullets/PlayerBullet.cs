using UnityEngine;

public class PlayerBullet : Bullet
{
    public override void Move()
    {
        //
        // TODO: Sound Effects
        // 

        var currentPosition = transform.position;
        currentPosition.y += _moveSpeed * Time.deltaTime;
        transform.position = currentPosition;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy") gameObject.SetActive(false);
    }
}