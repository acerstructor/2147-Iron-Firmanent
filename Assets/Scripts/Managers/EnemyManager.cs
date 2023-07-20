using System.Linq;
using UnityEngine;


/// <summary>
/// The purpose of this manager is to manage all enemy drones
/// especially the boss drones in order to check the states of
/// each active drones in the game
/// </summary>
public class EnemyManager : Singleton<EnemyManager>
{
    [SerializeField] private float _borderLimitX, _borderLimitAbove, _borderLimitBelow;

    private Drone[] _drones;

    public Drone[] GetDrones { get { return _drones; } }

    public Drone[] GetActiveDrones()
    {
        return _drones.Where(drone => drone.gameObject.activeInHierarchy).ToArray();
    }

    protected override void Awake()
    {
        _drones = Resources.FindObjectsOfTypeAll<Drone>();
        base.Awake();
    }

    private void Update()
    {
        ManageDrones();
    }
    
    private void ManageDrones()
    {
        foreach (var drone in _drones)
        {
            if (!drone.gameObject.activeInHierarchy) continue;

            drone.Animate();

            if (drone.GetHealth() <= 0) drone.Die();

            drone.Move();

            if (drone is ShooterDrone shooterDrone)
            {
                shooterDrone.Shoot();
            }

            CheckBorder(drone);
        }
    }

    private void CheckBorder(Drone drone)
    {
        var currentPos = drone.transform.position;

        if (currentPos.y > _borderLimitAbove || currentPos.y < _borderLimitBelow
            || currentPos.x > _borderLimitX || currentPos.x < -_borderLimitX)
        {
            drone.Deactivate();
        }
    }
}
