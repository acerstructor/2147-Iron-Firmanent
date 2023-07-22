using UnityEngine;

public class Player : MonoBehaviour 
{
    [SerializeField] private int _maxLives;
    [SerializeField] private PlayerPlane _playerPlane;
    [SerializeField] private float _respawnTimeMax;
    [SerializeField] private int _score;
    [SerializeField] private float _firingDurationMax;

    private float _respawnTime;
    private int _currentLives;

    private float _firingDuration = 0;

    public int GetScore { get { return _score; } }
    public bool IsFiring
    {
        get
        {
            if (_firingDuration <= 0) return false;
            return true;
        }
        set
        {
            if (!value) return;

            _firingDuration = _firingDurationMax;
        }
    }

    private void OnEnable()
    {
        GameManager.Instance.OnPlayerStateChange += PlayerOnStateChange;
        GameManager.Instance.OnPlayerScoreGain += PlayerOnScoreGain;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnPlayerStateChange -= PlayerOnStateChange;
        GameManager.Instance.OnPlayerScoreGain -= PlayerOnScoreGain;
    }

    private void Awake()
    {
        _currentLives = _maxLives;
    }

    private void Update()
    {
        if (IsGameOver()) return;

        CheckFireDuration();
        CheckLives();
        CheckRespawn();

        if (_playerPlane.isActiveAndEnabled) _playerPlane.Animate();

        _playerPlane.Entrance();
        _playerPlane.Recover();
        _playerPlane.Die();
    }

    private void CheckFireDuration()
    {
        if (_firingDuration > 0)
        {
            _firingDuration -= Time.deltaTime;
        }
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

        if (currentPlayerState != PlayerState.INACTIVE) return;

        if (_currentLives <= 0)
        {
            GameManager.Instance.SetGameState(GameState.GAMEOVER);
        }

    }

    private void CheckRespawn()
    {
        var currentState = GameManager.Instance.PlayerState;

        if (currentState != PlayerState.INACTIVE) return;

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
        if (playerState == PlayerState.INACTIVE)
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
