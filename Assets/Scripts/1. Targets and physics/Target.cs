using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField]
    private float speed = 1.5f;

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        gameObject.transform.Translate(new Vector3(x,0.0f,z) * speed * Time.deltaTime, Space.Self);
    }
}
