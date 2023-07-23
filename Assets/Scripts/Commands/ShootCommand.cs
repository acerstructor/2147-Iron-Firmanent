using UnityEngine;

public class ShootCommand : ICommand 
{
   	private Transform _transform;
	private Player _player;

	public ShootCommand(Transform transform, Player player)
	{
		_transform = transform;
		_player = player;
	}

	public void Execute()
    {
		if (_player.IsFiring) return;

		AudioManager.Instance.PlaySoundEffect("PlayerShoot", SfxType.SHOOT);

		ObjectPool.Instance.SpawnFromPool("PlayerBullet", new Vector2(_transform.position.x, _transform.position.y + 0.4f), Quaternion.identity);
		_player.IsFiring = true;
	}
}
