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

		//
		// TO DO: Sound effects, refactoring, such
		//

		ObjectPool.Instance.SpawnFromPool("PlayerBullet", _transform.position, Quaternion.identity);
		_player.IsFiring = true;
	}
}
