using System.Collections;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class WaveManager : Singleton<WaveManager>
{
    public void Start()
    {
        bool isDebug = GameManager.Instance.IsDebug;

        InitStates();

        if (!isDebug)
            StartCoroutine(LevelOne(0.5f));
        else
            StartCoroutine(Debug_Wave(0.5f));
    }

    private void InitStates()
    {
        GameManager.Instance.SetPlayerState(PlayerState.ENTERING);
        GameManager.Instance.SetGameState(GameState.GAMEPLAY);
    }

    private void Update()
    {
        
    }

    /// <summary>
    /// 
    /// </summary>
    private IEnumerator LevelOne(float spawnTimer)
    {
        //
        // TO DO: SET-UP
        // 

        yield return new WaitForSeconds(2);

        yield return SpawnFormat.Polygon("CarrierDrone", 2, 0.2f, transform.position, -1.5f, 1.5f, 3f);

        yield return SpawnFormat.Rectangle("CarrierDrone", 0.5f, transform.position, -1.5f, 1.5f, 1.5f);

        yield return SpawnFormat.Polygon("CarrierDrone", 2, 0.2f, transform.position, -1.5f, 1.5f, 5f);

        yield return SpawnFormat.Line("PrimaryDrone", 2, 0.5f, transform.position, -1.5f, 1.5f, 4f);

        yield return SpawnFormat.Line("PrimaryDrone", 2, 0.5f, transform.position, -1.5f, 1.5f, 6f);

        yield return SpawnFormat.Line("PrimaryDrone", 4, 1f, transform.position, -1.5f, 0f, 1f);

        yield return SpawnFormat.Line("ShooterDrone", 2, 0.6f, transform.position, -1.5f, 1.5f, 1f);

        yield return SpawnFormat.Repeated("CarrierDroneFromLeftToRight", 5, transform.position, 1, 1f);

        yield return SpawnFormat.Repeated("CarrierDroneFromRightToLeft", 5, transform.position, 1, 5f);

        

        
    }

    /// <summary>
    /// This coroutine function only works on debug scene
    /// to test out the enemy drones and also the format
    /// </summary>
    private IEnumerator Debug_Wave(float spawnTimer)
    {
        yield return new WaitForSeconds(2);

        ObjectPool.Instance.SpawnFromPool("MothershipLevel1", transform.position, Quaternion.identity);
    }
}
