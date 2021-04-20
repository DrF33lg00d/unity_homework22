using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCarController : MonoBehaviour
{
    public float speed = 10f;
    public float rotationSpeed = 100f;

    // Update is called once per frame
    void Update()
    {
        float right = Input.GetAxis("Horizontal");
        float forward = Input.GetAxis("Vertical");

        transform.Translate(0, 0, forward * speed * Time.deltaTime);
        transform.Rotate(0, right * rotationSpeed * Time.deltaTime * forward, 0);
    }
}
