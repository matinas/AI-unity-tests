using UnityEngine;

public class BasicBullet : MonoBehaviour
{
    private const float Gravity = -9.8f; // This is already an acceleration, not a force, so we don't need to divide it by the object's mass

    [SerializeField]
    private float Deacceleration;

    [SerializeField]
    private float Force;

    [SerializeField]
    private float Mass;

    [SerializeField]
    private float DecreaseSpeedFactor;

    private float _acceleration;
    private float _speedZ = 0.0f;
    private float _speedY = 0.0f;
    private Vector3 _velocity;

    // Start is called before the first frame update
    void Start()
    {
        _acceleration = Force / Mass;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        _speedZ += _acceleration * Time.deltaTime;  // increase velocity forward component based on acceleration over time
        _speedY += Gravity * Time.deltaTime;        // increase velocity updward component based on gravity over time (it's decreased actually)

        _velocity = new Vector3(0.0f, _speedY, _speedZ) * DecreaseSpeedFactor;
        gameObject.transform.Translate(_velocity);

        _acceleration -= Time.deltaTime * Deacceleration; // deaccelerate over time
        _acceleration = Mathf.Clamp(_acceleration, 0, float.MaxValue);
    }
}
