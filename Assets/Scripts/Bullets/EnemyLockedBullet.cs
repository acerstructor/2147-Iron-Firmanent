using UnityEngine;

public class EnemyLockedBullet : EnemyBullet
{
    [SerializeField] private PlayerPlane _player;
    
    private Vector2 _playerPos;

    private void Awake()
    {
        if (_player == null)
        {
            _player = FindAnyObjectByType<PlayerPlane>();
        }
    }

    private void OnEnable()
    {
        if (_player != null)
            _playerPos = _player.transform.position;
        else
            _playerPos = Vector2.down;
    }

    public override void Move()
    {
        transform.Translate(_playerPos * _moveSpeed * Time.deltaTime);
    }

}
