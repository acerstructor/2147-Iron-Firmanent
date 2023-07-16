using UnityEngine;

public class ShootCommand : ICommand 
{
   	private Transform _transform;

	public ShootCommand(Transform transform)
	{
		_transform = transform;
	}

	public void Execute()
    {
		//
		// TO DO: Sound effects, refactoring, such
		//

		ObjectPool.Instance.SpawnFromPool("PlayerBullet", _transform.position, Quaternion.identity);
	}
}
