using UnityEngine;

/// <summary>
/// A carrier drone. It just carry but doesn't attack.
/// Hmmm... maybe it carries a supply to another AI outpost
/// I guess?....
/// </summary>
public class CarrierDrone : Drone
{
    private float _current;

    [SerializeField] private AnimationCurve _curve;
    [SerializeField] protected Vector2 _spawningPos;
    [SerializeField] protected Vector2 _targetPos;
    [SerializeField] protected bool _isMovementLocked;

    public override void Move()
    {
        if (_isMovementLocked)
        {
            _current = Mathf.MoveTowards(_current, 1f, _moveSpeed * Time.deltaTime);
            Vector3 currentPos = Vector3.Lerp(_spawningPos, _targetPos, _curve.Evaluate(_current));
            transform.position = currentPos;

            return;
        }

        transform.Translate(Vector2.down * _moveSpeed * Time.deltaTime);
    }

    public override void Die()
    {
        //
        // TO DO:
        //
        base.Die();
    }
}
