using TMPro;
using UnityEngine;

public partial class HUD : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private LivesBar _livesBar;
    [SerializeField] private TMP_Text _scoreText;
    
    private void OnEnable()
    {
        GameManager.Instance.OnPlayerStateChange += PlayerOnStateChange;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnPlayerStateChange -= PlayerOnStateChange;
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

    private void Update()
    {
        UpdateText();
    }

    private void UpdateText()
    {
        _scoreText.text = _player.GetScore.ToString().PadLeft(12, '0');
    }
}
