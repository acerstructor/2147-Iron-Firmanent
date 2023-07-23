using UnityEngine;

public class MainMenuScene : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.SetGameState(GameState.MAINMENU);
    }

    void Update()
    {
        var isMusicPlaying = AudioManager.Instance.IsMusicPlaying;
        var isLoading = LevelManager.Instance.IsLoading;

        if (!isMusicPlaying && !isLoading)
            AudioManager.Instance.PlayMusic("MainMenu");

        if (Input.GetMouseButtonDown(0))
        {
            // Play Sound Effect
            if (isLoading) return;

            AudioManager.Instance.PlaySoundEffect("StartGame", SfxType.GAMESTATE);
            AudioManager.Instance.StopMusic();
            LevelManager.Instance.LoadScene(1);
        }
    }
}
