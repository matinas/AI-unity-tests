using System;
using UnityEngine;

public class AutoPilotUnity : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private float speed = 0.5f;

    [SerializeField]
    private float rotSpeed = 50f;

    private bool _autoPilot = false;
    private float _accuracy = 0.5f;
    private float _rotAccuracy = 0.5f;
    private float _rotSnapness = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!_autoPilot)
        {
            float yRot = Input.GetAxis("Horizontal");
            float zTrans = Input.GetAxis("Vertical");

            gameObject.transform.Rotate(0.0f, yRot * rotSpeed * Time.deltaTime, 0.0f);
            gameObject.transform.Translate(new Vector3(0.0f, 0.0f, zTrans) * speed * Time.deltaTime, Space.Self);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            _autoPilot = !_autoPilot;
        }
    }

    void LateUpdate()
    {
        if (_autoPilot)
        {
            Vector3 targetDir = target.transform.position - gameObject.transform.position;

            if (targetDir.magnitude > _accuracy)
            {
                Vector3 forwardDir = gameObject.transform.forward;

                // Rotate the player towards the target
                float angle = Vector3.SignedAngle(forwardDir, targetDir, gameObject.transform.up);

                if (Math.Abs(angle) > _rotAccuracy) // avoid rotation if the angle is not big enough (smoother movement)
                {
                    gameObject.transform.Rotate(0.0f, angle * _rotSnapness, 0.0f, Space.Self); // we use the _rotSnapness to smooth the rotation around,
                                                                                               // don't just rotate and stick immediately to the target
                }

                // Move the player towards the target
                gameObject.transform.Translate(gameObject.transform.forward * speed * Time.deltaTime, Space.World);
            }
        }
    }
}
