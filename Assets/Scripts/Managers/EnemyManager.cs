using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    [SerializeField] private float _borderLimitX, _borderLimitY;

    public Drone[] _drones;

    protected override void Awake()
    {
        base.Awake();
        _drones = Resources.FindObjectsOfTypeAll<Drone>();
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

        if (currentPos.y > _borderLimitY || currentPos.y < -_borderLimitY
            || currentPos.x > _borderLimitX || currentPos.x < -_borderLimitX)
        {
            drone.Deactivate();
        }
    }
}
