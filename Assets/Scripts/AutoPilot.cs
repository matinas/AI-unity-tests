using System;
using UnityEngine;

public class AutoPilot : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private float speed = 0.5f;

    [SerializeField]
    private float rotSpeed = 50f;

    private bool _autoPilot = false;
    private float _accuracy = 0.5f;

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
            if (Distance(gameObject.transform.position, target.position) > _accuracy)
            {
                // Rotate the player towards the target
                double angle = CalculateAngle(gameObject.transform, target.transform);
                gameObject.transform.Rotate(0.0f, (float) angle * Mathf.Rad2Deg, 0.0f, Space.Self);

                // Move the player towards the target
                gameObject.transform.Translate(gameObject.transform.forward * speed * Time.deltaTime, Space.World);
                // gameObject.transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self); // exactly the same as above
            }
        }
    }

    private double Distance(Vector3 p1, Vector3 p2)
    {
        return Norm(p2-p1);
    }

    private double CalculateAngle(Transform player, Transform target)
    {
        Vector3 forwardDir = player.transform.forward;
        Vector3 targetDir = target.transform.position - player.transform.position;
        targetDir = Normalize(targetDir);

        Vector3 ortho = CrossProduct(forwardDir, targetDir);

        // Debug.DrawRay(player.transform.position, forwardDir, Color.green,5.0f);
        // Debug.DrawRay(player.transform.position, targetDir, Color.red, 5.0f);
        // Debug.DrawRay(player.transform.position, ortho, Color.blue, 5.0f);
        
        double angle = Math.Acos((DotProduct(forwardDir, targetDir))/(Norm(forwardDir)*Norm(targetDir)));
        double signedAngle = angle * Math.Sign(ortho.y);

        return signedAngle;
    }

    private Vector3 Normalize(Vector3 v)
    {
        float norm = (float) Norm(v);

        return new Vector3(v.x/norm, v.y/norm, v.z/norm);
    }

    private double DotProduct(Vector3 v, Vector3 w)
    {
        return ((v.x*w.x)+(v.y*w.y)+(v.z*w.z));
    }

    private double Norm(Vector3 v)
    {
        return Math.Sqrt(Math.Pow(v.x, 2) + (Math.Pow(v.y, 2)) + (Math.Pow(v.z, 2)));
    }

    private Vector3 CrossProduct(Vector3 v, Vector3 w)
    {
        return new Vector3(v.y*w.z - v.z*w.y, v.z*w.x - v.x*w.z, v.x*w.y - v.y*w.x);
    }
}
