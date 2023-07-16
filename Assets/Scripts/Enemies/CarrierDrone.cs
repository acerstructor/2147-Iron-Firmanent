using UnityEngine;

public class CarrierDrone : Drone
{
    [SerializeField] private AnimationCurve _curve;
    [SerializeField] private Vector3 _spawningPos;
    [SerializeField] private Vector3 _targetPos;

    private float _current, curveHeight;

    public CarrierDrone()
    {
    }

    public override void Move()
    {
        _current = Mathf.MoveTowards(_current, 1f, _moveSpeed * Time.deltaTime);
        Vector3 currentPos = Vector3.Lerp(_spawningPos, _targetPos, _curve.Evaluate(_current));
        transform.position = currentPos;
    }

    public override void Spawn()
    {
        
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Colliding!");
        if (collision.gameObject.tag == "PlayerBullet")
        {
            //
            // TODO: Color Effects
            //
            _health--;
        }
    }
}
