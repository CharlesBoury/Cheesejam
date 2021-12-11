using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tourniquet : MonoBehaviour
{
    public float speed = 1f;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        speed += Time.deltaTime;
        rb.AddTorque(0f, Time.deltaTime * speed, 0f);
    }
}
