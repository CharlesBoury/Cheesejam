using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tourniquet : MonoBehaviour
{
    float speed;
    // Start is called before the first frame update
    void Start()
    {
        speed = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        speed += Time.deltaTime;
        transform.Rotate(0f, Time.deltaTime * speed, 0f);
    }
}
