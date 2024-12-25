using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingChargedParticle : ChargedParticle
{
    public float mass = 1;
    public Rigidbody rb;

    void Start()
    {
        UpdateColor();

        rb = gameObject.AddComponent<Rigidbody>();
        rb.mass = mass;
        rb.useGravity = false;
    }
}
