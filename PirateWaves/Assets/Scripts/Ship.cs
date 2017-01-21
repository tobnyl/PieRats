using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    #region Fields/Properties

    public int Index;
    public float ForwardForce = 1f;
    public float RotationForce = 1f;
        
    [Header("Base Canon")]
    public GameObject BaseCanon;
    public float BaseCannonRotationSpeedY = 10f;
    public float BaseCannonSlerpSpeedY = 1f;
    [Header("Canon")]
    public GameObject Canon;
    public float CannonRotationSpeedX = 10f;
    public float CannonSlerpSpeedX = 1f;
    public Vector2 CanonAngleLimit;

    private Rigidbody _rigidbody;
    private float _currentBaseCanonAngleY;
    private float _currentCanonAngleX;

    private Vector3 AxisLeft
    {
        get { return new Vector3(Input.GetAxis("Horizontal" + Index), 0, -Input.GetAxis("Vertical" + Index)); }
    }

    private Vector3 AxisRight
    {
        get { return new Vector3(Input.GetAxis("HorizontalRight" + Index), 0, -Input.GetAxis("VerticalRight" + Index)); }
    }


    #endregion
    #region Events

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        _rigidbody.AddForce(transform.forward * AxisLeft.z * ForwardForce);
        _rigidbody.AddTorque(Vector3.up * AxisLeft.x * RotationForce);
    }

	void Update ()
	{
        RotateBaseCannon();
	    RotateCannon();
	}

    #endregion
    #region Methods

    private void RotateBaseCannon()
    {
        _currentBaseCanonAngleY += AxisRight.x * BaseCannonRotationSpeedY;

        var newRotationY = Quaternion.AngleAxis(_currentBaseCanonAngleY + transform.rotation.eulerAngles.y, BaseCanon.transform.up);

        BaseCanon.transform.rotation = Quaternion.Slerp(BaseCanon.transform.rotation, newRotationY, Time.deltaTime * BaseCannonSlerpSpeedY);
    }

    private void RotateCannon()
    {
        _currentCanonAngleX += AxisRight.z * CannonRotationSpeedX;

        _currentCanonAngleX = Mathf.Clamp(_currentCanonAngleX, CanonAngleLimit.x, CanonAngleLimit.y);

        Debug.Log(_currentCanonAngleX);
        var newRotationX = Quaternion.AngleAxis(_currentCanonAngleX, BaseCanon.transform.right);
        Canon.transform.rotation = Quaternion.Slerp(Canon.transform.rotation, newRotationX, Time.deltaTime*CannonSlerpSpeedX);
    }

    #endregion
}
