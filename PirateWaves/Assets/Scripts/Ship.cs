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
    public float BaseCanonRotationSpeedY = 10f;
    public float BaseCanonSlerpSpeedY = 1f;
    public Vector2 BaseCanonAngleLimit;

    [Header("Canon")]
    public GameObject Canon;
    public float CanonRotationSpeedX = 10f;
    public float CanonSlerpSpeedX = 1f;
    public Vector2 CanonAngleLimit;

    [Header("Canon Ball")]
    public GameObject CanonBallPosition;
    public GameObject CanonBallPrefab;
    public float CanonBallForce = 10f;
    public float CoolDown = 1f;

    private Rigidbody _rigidbody;
    private float _currentBaseCanonAngleY;
    private float _currentCanonAngleX;
    private bool _isFiring;
    private bool _instantiateCannonBall;
    private float _currentCoolDown;

    private Vector3 AxisLeft
    {
        get { return new Vector3(Input.GetAxis("Horizontal" + Index), 0, -Input.GetAxis("Vertical" + Index)); }
    }

    private Vector3 AxisRight
    {
        get { return new Vector3(Input.GetAxis("HorizontalRight" + Index), 0, Input.GetAxis("VerticalRight" + Index)); }
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

        if (_instantiateCannonBall)
        {
            var canonBall = CanonBallPrefab.Instantiate(CanonBallPosition.transform.position, Quaternion.identity);
            var canonBallRigidBody = canonBall.GetComponent<Rigidbody>();

            canonBallRigidBody.AddForce(Canon.transform.up * CanonBallForce, ForceMode.Impulse);


            _instantiateCannonBall = false;
        }

    }

	void Update ()
	{
        RotateBaseCannon();
	    RotateCannon();
        Fire();
	}

    #endregion
    #region Methods

    private void RotateBaseCannon()
    {
        _currentBaseCanonAngleY += AxisRight.x * BaseCanonRotationSpeedY;
        _currentBaseCanonAngleY = Mathf.Clamp(_currentBaseCanonAngleY, BaseCanonAngleLimit.x, BaseCanonAngleLimit.y);

        var newRotationY = Quaternion.AngleAxis(_currentBaseCanonAngleY + transform.rotation.eulerAngles.y, BaseCanon.transform.up);

        BaseCanon.transform.rotation = Quaternion.Slerp(BaseCanon.transform.rotation, newRotationY, Time.deltaTime * BaseCanonSlerpSpeedY);
    }

    private void RotateCannon()
    {
        _currentCanonAngleX += AxisRight.z * CanonRotationSpeedX;

        _currentCanonAngleX = Mathf.Clamp(_currentCanonAngleX, CanonAngleLimit.x, CanonAngleLimit.y);

        var newRotationX = Quaternion.AngleAxis(_currentCanonAngleX, BaseCanon.transform.right);
        Canon.transform.rotation = Quaternion.Slerp(Canon.transform.rotation, newRotationX, Time.deltaTime*CanonSlerpSpeedX);
    }

    private void Fire()
    {
        if (Input.GetAxisRaw("Fire" + Index) > 0 && _currentCoolDown <= 0 && !_isFiring)
        {
            _isFiring = true;
            _currentCoolDown = CoolDown;
            _instantiateCannonBall = true; 
        }
        else if (Input.GetAxisRaw("Fire" + Index) == 0)
        {
            _isFiring = false;
        }

        _currentCoolDown -= Time.deltaTime;
    }

    #endregion
}
