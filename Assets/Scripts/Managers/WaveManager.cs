using System.Collections;
using UnityEngine;

public class WaveManager : Singleton<WaveManager>
{
    public void Start()
    {
        StartCoroutine(Wave_One(0.5f));
    }

    private IEnumerator Wave_One(float spawnTimer)
    {
        //
        // TO DO: SET-UP
        // 

        yield return new WaitForSeconds(2);

        ObjectPool.Instance.SpawnFromPool("ShooterDrone", new Vector2(0, 6.5f), Quaternion.identity);
        ObjectPool.Instance.SpawnFromPool("PrimaryDrone", new Vector2(1, 6.5f), Quaternion.identity);
    }


}
