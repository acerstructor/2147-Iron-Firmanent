using System.Collections;
using UnityEngine;

/// <summary>
/// This class is one of the main core of the game in order
/// to handle levels, spawning enemies, boss fights, and so on
/// </summary>
public class WaveManager : Singleton<WaveManager>
{
    public void Start()
    {
        InitStates();

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
        yield return new WaitForSeconds(2);

        yield return SpawnFormat.Polygon("CarrierDrone", 2, 0.2f, transform.position, -1.5f, 1.5f, 3f);

        yield return SpawnFormat.Rectangle("CarrierDrone", 0.5f, transform.position, -1.5f, 1.5f, 1.5f);

        yield return SpawnFormat.Polygon("CarrierDrone", 2, 0.2f, transform.position, -1.5f, 1.5f, 5f);

        yield return SpawnFormat.Line("PrimaryDrone", 2, 0.5f, transform.position, -1.5f, 1.5f, 4f);

        yield return SpawnFormat.Line("PrimaryDrone", 2, 0.5f, transform.position, -1.5f, 1.5f, 4f);

        yield return SpawnFormat.Line("PrimaryDrone", 4, 1f, transform.position, -1.5f, 0f, 1f);

        yield return SpawnFormat.Line("ShooterDrone", 2, 0.6f, transform.position, -1.5f, 1.5f, 1f);

        yield return SpawnFormat.Repeated("CarrierDroneFromLeftToRight", 5, transform.position, 1, 1f);

        yield return SpawnFormat.Repeated("CarrierDroneFromRightToLeft", 5, transform.position, 1, 5f);

        yield return SpawnFormat.Line("PrimaryDrone", 2, 0.5f, transform.position, -1.5f, 1.5f, 2f);

        yield return SpawnFormat.Line("PrimaryDrone", 2, 0.5f, transform.position, -1.5f, 1.5f, 6f);
   
        yield return BossBattle("MothershipLevel1");

        yield return LevelOne();
    }

    private IEnumerator BossBattle(string BossName)
    {
        while (EnemyManager.Instance.GetActiveDrones().Length > 0) yield return null;

        Vector2 currentSpawnPos = transform.position;
        ObjectPool.Instance.SpawnFromPool(BossName, new Vector2(currentSpawnPos.x, currentSpawnPos.y + 1f) , Quaternion.identity);

        while (GameManager.Instance.LevelState == LevelState.BOSSBATTLE)
        {
            Debug.Log("Boss Battle");

            yield return null;
        }

        if (GameManager.Instance.LevelState == LevelState.BOSSWIN)
        {
            //
            // TO DO: WINNING SFX, EFFECTS, SUCH
            // 
            Debug.Log("Winner!");
            yield return new WaitForSeconds(7f);
        }

        yield return null;
    }

    private IEnumerator LevelTwo()
    {
        yield return new WaitForSeconds(1f);
    }

    /// <summary>
    /// This coroutine function only works on debug scene
    /// to test out the enemy drones and also the format
    /// </summary>
    private IEnumerator Debug_Wave()
    {
        yield return new WaitForSeconds(2);

        ObjectPool.Instance.SpawnFromPool("ShooterDrone", transform.position, Quaternion.identity);

        yield return new WaitForSeconds(6f);

        ObjectPool.Instance.SpawnFromPool("ShooterDrone", transform.position, Quaternion.identity);

    }
}
