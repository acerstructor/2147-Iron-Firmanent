using UnityEngine;

public class Player : MonoBehaviour 
{
    [SerializeField] private int _maxLives;
    [SerializeField] private PlayerPlane _playerPlane;
    [SerializeField] private float _respawnTimeMax;

    private float _respawnTime;
    [SerializeField] private int _score;
    private int _currentLives;

    public int GetScore { get { return _score; } }

    private void OnEnable()
    {
        GameManager.Instance.OnPlayerStateChange += PlayerOnStateChange;
        GameManager.Instance.OnPlayerScoreGain += PlayerOnScoreGain;
    }

    private void Awake()
    {
        _currentLives = _maxLives;
    }
    private void Update()
    {
        if (IsGameOver()) return;

        CheckLives();
        CheckRespawn();

        _playerPlane.Entrance();
        _playerPlane.Recovering();
        _playerPlane.Die();
    }

    private bool IsGameOver()
    {
        var currentGameState = GameManager.Instance.State;
        
        if (currentGameState == GameState.GAMEOVER)
            return true;

        return false;
    }
    private void CheckLives()
    {
         var currentPlayerState = GameManager.Instance.PlayerState;

        if (currentPlayerState != PlayerState.DEAD) return;

        if (_currentLives <= 0)
        {
            GameManager.Instance.SetGameState(GameState.GAMEOVER);
        }

    }

    private void CheckRespawn()
    {
        var currentState = GameManager.Instance.PlayerState;

        if (currentState != PlayerState.DEAD) return;

        if (_respawnTime > 0)
        {
            _respawnTime -= Time.deltaTime;
            return;
        }

        GameManager.Instance.SetPlayerState(PlayerState.ENTERING);
        return;
    }


    private void PlayerOnStateChange(PlayerState playerState)
    {
        if (playerState == PlayerState.DEAD)
        {
            _respawnTime = _respawnTimeMax;
            LoseLive();
        }
    }

    private void PlayerOnScoreGain(bool isScored)
    {
        if (!isScored) return;

        var activeDrones = EnemyManager.Instance.GetActiveDrones();

        foreach (var drone in activeDrones)
        {
            if (drone.IsDead)
            {
                _score += drone.GetPoints;
            }
        }
    }

    private void LoseLive()
    {
        _currentLives--;
    }

    public int MaxLives
    {
        get { return _maxLives; }
        set { _maxLives = value; }
    }

    public int CurrentHealth
    {
        get { return _currentLives; }
        set
        {
            _currentLives = (value > _maxLives) ? _maxLives : value;
        }
    }
} 
