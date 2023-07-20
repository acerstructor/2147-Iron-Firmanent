using UnityEngine;

/// <summary>
/// This class handles all game object effects in the game
/// such as explosions, bullets and etc.
/// </summary>
public class EffectManager : Singleton<EffectManager>
{
    private Bullet[] _bullets;
    private Explosion[] _explosions;

    protected override void Awake()
    {
        base.Awake();
        _bullets = Resources.FindObjectsOfTypeAll<Bullet>();
        _explosions = Resources.FindObjectsOfTypeAll<Explosion>();
    }
    private void Update()
    {
        foreach (var bullet in _bullets)
        {
            if (!bullet.gameObject.activeInHierarchy) continue;

            bullet.Move();
        }

        foreach (var explosion in _explosions)
        {
            if (!explosion.gameObject.activeInHierarchy) continue;

            explosion.Animate();
        }
    }
}
