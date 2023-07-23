using UnityEngine;

public class GameplayScene : MonoBehaviour
{
    private bool _isLoading = false;
    private float _currentDuration = 0;
    private float _durationMax = 3;

    private void Awake()
    {
        _currentDuration = _durationMax;
    }

    private void Update()
    {
        var currentGameState = GameManager.Instance.State;

        if (currentGameState == GameState.GAMEOVER)
        {
            if (_currentDuration > 0)
            {
                _currentDuration -= Time.deltaTime;
                return;
            }

            if (_isLoading) return;
            _isLoading = true;

            LevelManager.Instance.LoadScene(0); // load to main menu
        }
    }
}
