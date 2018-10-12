using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nucleon : MonoBehaviour
{
    public float attractionForce;

    private Rigidbody rgBody;

    private void Awake()
    {
        rgBody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rgBody.AddForce(transform.localPosition * -attractionForce);
    }

}
