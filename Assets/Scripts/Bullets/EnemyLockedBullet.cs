using UnityEngine;

public class EnemyLockedBullet : EnemyBullet
{
    [SerializeField] private GameObject _player;

    private Vector2 _playerPos;

    private void Awake()
    {
        if (_player == null)
        {
            _player = GameObject.FindWithTag("Player");
        }
    }

    private void OnEnable()
    {
        _playerPos = _player.transform.position;
    }

    public override void Move()
    {
        transform.Translate(_playerPos * _moveSpeed * Time.deltaTime);
    }

}
