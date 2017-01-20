using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public int Index;
    public float Force = 1f;

    private Rigidbody _rigidbody;

    private Vector3 AxisLeft
    {
        get { return new Vector3(Input.GetAxis("Horizontal" + Index), 0, -Input.GetAxis("Vertical" + Index)); }
    }

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

	// Use this for initialization
	void Start ()
    {
		
	}

    void FixedUpdate()
    {
        _rigidbody.AddForce(AxisLeft * Force);
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if (Input.GetButtonDown("Fire1"))
	    {
	        Debug.Log("Fire!");
	    }

		Debug.Log(AxisLeft);
	}
}
