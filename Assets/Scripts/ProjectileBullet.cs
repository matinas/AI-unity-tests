using UnityEngine;

public class ProjectileBullet : MonoBehaviour
{
    private const float Gravity = 9.8f; // This is already an acceleration, not a force, so we don't need to divide it by the object's mass

    [SerializeField]
    private float DecreaseSpeedFactor;

    private float _speed = 15.0f;
    private float v0x, v0y, vy;
    private Vector3 _velocity;
    private float _angle;
    private AutoaimShoot _autoAimer;

    void Start()
    {
        _autoAimer = transform.GetComponentInParent<AutoaimShoot>();
        Vector3 targetDir = _autoAimer.target.position - _autoAimer.transform.position;

        _angle = Vector3.SignedAngle(transform.forward, targetDir, Vector3.right);
        // Debug.Log($"Bullet angle is {_angle}");

        v0x = _speed * Mathf.Cos(_angle * Mathf.Deg2Rad);
        v0y = _speed * Mathf.Sin(_angle * Mathf.Deg2Rad);

        vy = v0y;
    }

    void LateUpdate()
    {
        vy -= Gravity * Time.deltaTime; // update the y component of the velocity vector

        _velocity = new Vector3(v0x, vy, 0.0f); // calculate new velocity vector
        transform.Translate(new Vector3(0.0f, _velocity.y, _velocity.x) * Time.deltaTime); // translate along the new velocity
        
        // transform.forward = _velocity; // FIXME: aim the bullet towards the velocity direction

        Debug.DrawRay(transform.position, _velocity, Color.green, 0.1f);
        Debug.DrawRay(transform.position, Vector3.right * _velocity.x, Color.red, 0.1f);
        Debug.DrawRay(transform.position, Vector3.up * _velocity.y, Color.blue, 0.1f);
    }
}
