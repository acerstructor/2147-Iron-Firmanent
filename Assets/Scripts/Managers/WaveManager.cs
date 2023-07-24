using System.Collections;
using UnityEngine;

/// <summary>
/// This class is one of the main core of the game in order
/// to handle levels, spawning enemies, boss fights, and so on
/// </summary>
public class WaveManager : Singleton<WaveManager>
{
    protected override void Awake()
    {
        base.Awake();

        InitStates();
    }

    private void OnEnable()
    {
        GameManager.Instance.OnStateChange += GameOnStateChange;
    }

    private void GameOnStateChange(GameState state)
    {
        if (state == GameState.GAMEOVER)
        {
            if (AudioManager.Instance.IsMusicPlaying) AudioManager.Instance.StopMusic();

            AudioManager.Instance.PlaySoundEffect("GameOver", SfxType.GAMESTATE);
        }
    }

    public void Start()
    {
        StartCoroutine(ManageWaves());
        //StartCoroutine(Debug_Wave());
    }

    private void InitStates()
    {
        GameManager.Instance.SetPlayerState(PlayerState.ENTERING);
        GameManager.Instance.SetGameState(GameState.GAMEPLAY);
        GameManager.Instance.SetLevelState(LevelState.LEVELFIGHT);
    }

    private void Update()
    {
        var currentGameState = GameManager.Instance.State;

        if (currentGameState == GameState.GAMEOVER)
        {
            StopCoroutine(ManageWaves());
            //StopCoroutine(Debug_Wave());
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private IEnumerator ManageWaves()
    {
        yield return LevelOne();
    }

    private IEnumerator LevelOne()
    {
        var minX = -2f;
        var maxX = 0f;

        // Set-up Gameplay Soundtrack
        AudioManager.Instance.PlayMusic("Gameplay");

        yield return new WaitForSeconds(2);

        yield return SpawnFormat.Polygon("CarrierDrone", 2, 0.35f, transform.position, minX, maxX, 5f);

        yield return SpawnFormat.Polygon("CarrierDrone", 2, 0.35f, transform.position, minX, maxX, 2f);

        yield return SpawnFormat.Line("PrimaryDrone", 2, 1f, transform.position, minX, maxX, 2f);

        yield return SpawnFormat.Polygon("CarrierDrone", 2, 0.35f, transform.position, minX, maxX, 3f);

        yield return SpawnFormat.Line("PrimaryDrone", 2, 1f, transform.position, minX, maxX, 6f);

        yield return SpawnFormat.Polygon("CarrierDrone", 2, 0.35f, transform.position, minX, maxX, 3f);

        yield return SpawnFormat.Line("PrimaryDrone", 4, 1f, transform.position, minX, maxX, 3f);

        yield return SpawnFormat.Line("ShooterDrone", 2, 1.5f, transform.position, minX, maxX, 2f);

        yield return SpawnFormat.Repeated("CarrierDroneFromLeftToRight", 5, transform.position, 1, 1f);

        yield return SpawnFormat.Polygon("CarrierDrone", 2, 0.35f, transform.position, minX, maxX, 3f);

        yield return SpawnFormat.Repeated("CarrierDroneFromRightToLeft", 5, transform.position, 1, 1f);

        yield return SpawnFormat.Line("PrimaryDrone", 2, 1f, transform.position, minX, maxX, 2f);

        yield return SpawnFormat.Polygon("CarrierDrone", 2, 0.35f, transform.position, minX, maxX, 3f);

        yield return SpawnFormat.Line("PrimaryDrone", 2, 1f, transform.position, minX, maxX, 6f);
   
        yield return BossBattle("MothershipLevel1");

        yield return LevelTwo();
    }

    private IEnumerator BossBattle(string BossName)
    {
        while (EnemyManager.Instance.GetActiveDrones().Length > 0) yield return null;

        if (AudioManager.Instance.IsMusicPlaying) 
            AudioManager.Instance.StopMusic();

        AudioManager.Instance.PlayMusic("BossBattle");

        Vector2 currentSpawnPos = transform.position;
        ObjectPool.Instance.SpawnFromPool(BossName, new Vector2(currentSpawnPos.x, currentSpawnPos.y + 1f) , Quaternion.identity);

        while (GameManager.Instance.LevelState == LevelState.BOSSBATTLE)
        {
            Debug.Log("Boss Battle");

            yield return null;
        }

        if (GameManager.Instance.LevelState == LevelState.BOSSWIN)
        {
            if (AudioManager.Instance.IsMusicPlaying)
                AudioManager.Instance.StopMusic();

            AudioManager.Instance.PlaySoundEffect("Victory", SfxType.GAMESTATE);
            yield return new WaitForSeconds(7f);
        }

        yield return null;
    }

    private IEnumerator LevelTwo()
    {
        var minX = -2f;
        var maxX = 0f;

        // Set-up Gameplay Soundtrack
        AudioManager.Instance.PlayMusic("Gameplay");

        yield return new WaitForSeconds(2);

        yield return SpawnFormat.Polygon("CarrierDrone", 2, 0.35f, transform.position, minX, maxX, 2f);

        yield return SpawnFormat.Polygon("ShooterDrone", 2, 0.35f, transform.position, minX, maxX, 2f);

        yield return SpawnFormat.Line("PrimaryDrone", 2, 1f, transform.position, minX, maxX, 2f);

        yield return SpawnFormat.Polygon("Primary Drone", 2, 0.35f, transform.position, minX, maxX, 2f);

        yield return SpawnFormat.Line("PrimaryDrone", 2, 1f, transform.position, minX, maxX, 4f);

        yield return SpawnFormat.Polygon("CarrierDrone", 2, 0.35f, transform.position, minX, maxX, 3f);

        yield return SpawnFormat.Line("ShooterDrone", 4, 1f, transform.position, minX, maxX, 3f);

        yield return SpawnFormat.Line("ShooterDrone", 2, 1.5f, transform.position, minX, maxX, 2f);

        yield return SpawnFormat.Repeated("CarrierDroneFromLeftToRight", 5, transform.position, 1, 1f);

        yield return SpawnFormat.Polygon("CarrierDrone", 2, 0.35f, transform.position, minX, maxX, 3f);

        yield return SpawnFormat.Repeated("CarrierDroneFromRightToLeft", 5, transform.position, 1, 1f);

        yield return SpawnFormat.Line("PrimaryDrone", 2, 1f, transform.position, minX, maxX, 2f);

        yield return SpawnFormat.Polygon("ShooterDrone", 2, 0.35f, transform.position, minX, maxX, 3f);

        yield return SpawnFormat.Line("PrimaryDrone", 2, 1f, transform.position, minX, maxX, 6f);

        yield return BossBattle("MothershipLevel1");

        yield return LevelTwo();
    }

    /// <summary>
    /// This coroutine function only works on debug scene
    /// to test out the enemy drones and also the format
    /// </summary>
    private IEnumerator Debug_Wave()
    {
        yield return BossBattle("MothershipLevel1");

        yield return BossBattle("MothershipLevel1");

        yield return BossBattle("MothershipLevel1");
    }
}
