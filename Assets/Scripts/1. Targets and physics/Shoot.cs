using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField]
    private Transform SpawnPoint;

    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private GameObject bulletContainer;

    [SerializeField]
    private float RotSpeed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var go = GameObject.Instantiate(bulletPrefab, SpawnPoint.transform.position, SpawnPoint.transform.rotation, SpawnPoint);
            go.transform.parent = bulletContainer.transform;
        }
        else if (Input.GetKey(KeyCode.Q)) // raise up the gun
        {
            gameObject.transform.Rotate(-RotSpeed, 0.0f, 0.0f, Space.Self);
        }
        else if (Input.GetKey(KeyCode.E)) // pull down the gun
        {
            gameObject.transform.Rotate(RotSpeed, 0.0f, 0.0f, Space.Self);
        }
    }
}
