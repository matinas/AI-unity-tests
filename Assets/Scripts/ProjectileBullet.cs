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

        _angle = Vector3.SignedAngle(transform.forward, targetDir, Vector3.Cross(transform.forward, targetDir));

        v0x = _speed * Mathf.Cos(_angle * Mathf.Deg2Rad);
        v0y = _speed * Mathf.Sin(_angle * Mathf.Deg2Rad);

        vy = v0y;
    }

    void LateUpdate()
    {
        vy -= Gravity * Time.deltaTime; // update the y component of the velocity vector

        var v = new Vector3(0.0f, vy, v0x) * Time.deltaTime; // calculate new velocity vector
        transform.Translate(v);  // translate along the new velocity

        Debug.DrawRay(transform.position, Vector3.right * v.z, Color.red, 2.0f);
        Debug.DrawRay(transform.position, Vector3.up * v.y, Color.blue, 2.0f);
    }
}
