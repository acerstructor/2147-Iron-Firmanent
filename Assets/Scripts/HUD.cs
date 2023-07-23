using TMPro;
using UnityEngine;

public partial class HUD : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private LivesBar _livesBar;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _highScoreText;

    private void OnEnable()
    {
        var currentHighscore = PlayerPrefs.GetInt("Highscore");
        _highScoreText.text = currentHighscore.ToString().PadLeft(8, '0');

        GameManager.Instance.OnPlayerStateChange += PlayerOnStateChange;
        GameManager.Instance.OnStateChange += GameOnStateChange;
    }

    private void PlayerOnStateChange(PlayerState playerState)
    {
        if (playerState == PlayerState.DEAD)
        {
            _livesBar.LoseLiveChild();
        }
    }

    private void Awake()
    {
        if (_player == null)
        {
            _player = GameObject.FindAnyObjectByType<Player>();
        }

        _livesBar.InitHealthChildren(_player);
    }

    private void GameOnStateChange(GameState state)
    {
        if (state == GameState.GAMEOVER) SetHighScore();
    }

    private void SetHighScore()
    {
        var currentHighscore = PlayerPrefs.GetInt("Highscore");
        var currentScore = _player.GetScore;

        if (currentScore > currentHighscore)
        {
            PlayerPrefs.SetInt("Highscore", currentScore);
            GameManager.Instance.HasNewHighScore = true;
        }
    }

    private void Update()
    {
        UpdateText();
    }

    private void UpdateText()
    {
        _scoreText.text = _player.GetScore.ToString().PadLeft(8, '0');
    }
}
