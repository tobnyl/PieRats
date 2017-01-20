using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public int Index;
    public float ForwardForce = 1f;
    public float RotationForce = 1f;
    
    [Space(10f)]
    public GameObject BaseCanon;
    public float BaseCannonRotationSpeedY = 10f;
    public float BaseCannonSlerpSpeedY = 1f;

    private Rigidbody _rigidbody;
    private float _currentBaseCanonAngleY;

    private Vector3 AxisLeft
    {
        get { return new Vector3(Input.GetAxis("Horizontal" + Index), 0, -Input.GetAxis("Vertical" + Index)); }
    }

    private Vector3 AxisRight
    {
        get { return new Vector3(Input.GetAxis("HorizontalRight" + Index), 0, -Input.GetAxis("VerticalRight" + Index)); }
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
        _rigidbody.AddForce(transform.forward * AxisLeft.z * ForwardForce);
        _rigidbody.AddTorque(Vector3.up * AxisLeft.x * RotationForce);
    }
	
	// Update is called once per frame
	void Update ()
	{
        _currentBaseCanonAngleY += AxisRight.x * BaseCannonRotationSpeedY;

        var newRotationY = Quaternion.AngleAxis(_currentBaseCanonAngleY, BaseCanon.transform.up);
        	    
        BaseCanon.transform.rotation = Quaternion.Slerp(BaseCanon.transform.rotation, newRotationY, Time.deltaTime * BaseCannonSlerpSpeedY);        


    }
}
