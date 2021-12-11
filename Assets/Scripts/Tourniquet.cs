using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tourniquet : MonoBehaviour
{
    public float speed = 1f;
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        rb.AddTorque(0f, Time.deltaTime * speed, 0f);
    }
}
