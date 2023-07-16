using UnityEngine;

public class MoveCommand : ICommand
{
	private Transform _transform;
	private Vector3 _direction;
	private float _speed;
	private float _movementLimitX, _movementLimitY;

	public MoveCommand(Transform transform, Vector3 direction, float speed, float movementLimitX, float movementLimitY)
	{
		_transform = transform;
		_direction = direction;
		_movementLimitX = movementLimitX;
		_movementLimitY = movementLimitY;
		_speed = speed;
	}

	public void Execute()
    {
		//
		// TO DO: Sound effects, refactoring, such
		//
		Vector3 movement = _direction * _speed * Time.deltaTime;
		Vector3 newPosition = _transform.position + movement;

		newPosition.x = Mathf.Clamp(newPosition.x, -_movementLimitX, _movementLimitX);
		newPosition.y = Mathf.Clamp(newPosition.y, -_movementLimitY, _movementLimitY);
		newPosition.z = _transform.position.z; // Maintain the original z position

		_transform.position = newPosition;
	}
}
