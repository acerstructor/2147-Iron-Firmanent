using UnityEngine;

public class PlayerBullet : Bullet
{
    [SerializeField] private float _bulletSpeed;

    public override void Move()
    {
        //
        // TODO: Sound Effects
        // 

        var currentPosition = transform.position;
        currentPosition.y += _bulletSpeed * Time.deltaTime;
        transform.position = currentPosition;
    }


}