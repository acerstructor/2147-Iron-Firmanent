using UnityEngine;

/// <summary>
/// This class handles all the input which the
/// player does such as moving and shooting
/// </summary>
public class InputHandler : MonoBehaviour
{
    private ICommand _moveForward;
    private ICommand _moveBackward;
    private ICommand _moveLeft;
    private ICommand _moveRight;
    private ICommand _shoot;

    [SerializeField] private GameObject _playerPlane;
    [SerializeField] private Player _player;
    [SerializeField] private float _speed;
    [SerializeField] private float _movementLimitX, _movementLimitY;

    private void Awake()
    {
        if (_playerPlane == null) _playerPlane = GameObject.FindWithTag("Player");
        
        Transform playerTransform = _playerPlane.GetComponent<Transform>();

        _moveForward = new MoveCommand(playerTransform, Vector3.up, _speed, _movementLimitX, _movementLimitY);
        _moveBackward = new MoveCommand(playerTransform, Vector3.down, _speed, _movementLimitX, _movementLimitY);
        _moveLeft = new MoveCommand(playerTransform, Vector3.left, _speed, _movementLimitX, _movementLimitY);
        _moveRight = new MoveCommand(playerTransform, Vector3.right, _speed, _movementLimitX, _movementLimitY);
        _shoot = new ShootCommand(playerTransform, _player);
    }

    private void Update()
    {
        var currentPlayerState = GameManager.Instance.PlayerState;
        var currentGameState = GameManager.Instance.State;

        if (currentGameState == GameState.GAMEOVER) return;

        switch(currentPlayerState)
        {
            // Check if player is alive or recovering
            case PlayerState.ALIVE:
            case PlayerState.RECOVERING:

                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) _moveForward.Execute();
                if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) _moveBackward.Execute();
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) _moveLeft.Execute();
                if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) _moveRight.Execute();
                if (Input.GetKey(KeyCode.Space)) _shoot.Execute();

                break;
        }
    }
}
