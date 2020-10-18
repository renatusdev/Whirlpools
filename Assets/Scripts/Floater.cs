using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    private readonly static float WATER_HEIGHT = 0;

    public float buoyancyForce;

    Rigidbody r;

    void Start()
    {
        r = GetComponent<Rigidbody>();
        r.useGravity = true;
    }

    void FixedUpdate()
    {
        if(transform.position.y <= 0)
        {
            r.AddForce(Vector3.up * Mathf.Abs(Physics.gravity.y) * buoyancyForce);
        }
    }
}
