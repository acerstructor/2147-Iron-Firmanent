using System;
#if !UNITY_EDITOR && UNITY_WEBGL
using System.Runtime.InteropServices;
#endif

/// <summary>
/// This class handles the states of the game
/// whether if it's either game on going, game on pause
/// or rather game over
/// </summary>
public class GameManager : SingletonPersistent<GameManager>
{
    public Action<bool> OnPlayerScoreGain { get; set; }
    public Action<PlayerState> OnPlayerStateChange { get; set; }
    public Action<GameState> OnStateChange { get; set; }
    public Action<LevelState> OnLevelStateChange { get; set; }
    public bool IsDebug { get; private set; }
    public bool HasNewHighScore { get; set; }
    public GameState State { get; private set; }
    public PlayerState PlayerState { get; private set; }
    public LevelState LevelState { get; private set; }

    public void PlayerScored()
    {
        bool isPlayerScored = true;
        OnPlayerScoreGain?.Invoke(isPlayerScored);
    }

    public void SetPlayerState(PlayerState newState)
    {
        PlayerState = newState;
        OnPlayerStateChange?.Invoke(PlayerState);
    }

    public void SetGameState(GameState newState)
    {
        State = newState;
        OnStateChange?.Invoke(State);
    }
 
    public void SetLevelState(LevelState newState)
    {
        LevelState = newState;
        OnLevelStateChange?.Invoke(LevelState);
    }
    protected override void Awake()
    {
        HasNewHighScore = false;
        base.Awake();
    }

    private void Update()
    {
        FramerateManager.Instance.RequestFullFrameRate();
    }
}
