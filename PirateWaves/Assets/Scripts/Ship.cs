using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ship : MonoBehaviour
{
    #region Fields/Properties

    public int Index;
    public float ForwardForce = 1f;
    public float RotationForce = 1f;
    public bool BackMovement;
    public float MaxSpeed = 15f;
        
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

    [Header("Barrel")]
    public GameObject Barrel;
    public float BarrelDestroyForce;
    public float BarrelDestroyTorque;

    [Header("Mast")]
    public GameObject Mast;
    public Material MastMaterial;
    public float MastDestroyForce;
    public float MastDestroyTorque;

    [Header("Front")]
    public GameObject Front;

    [Header("Steer")]
    public GameObject Steer;
    public GameObject SteerWheel;

    [Header("Health")]
    public int StartHealth = 100;
    [SerializeField, ReadOnly]
    private int _health;
    public float ExplosionForce = 1f;
    public float CannonDestroyForce = 10f;
    public float CannonDestroyTorque = 10f;

    [Header("Captain Rat")]
    public GameObject CaptainRat;

    [Header("Sfx")]
    public Audio CanonFireSfx;
    public Audio HitSfx;
    public Audio WaveSfx;
    public Vector2 WaveSfxRandomIntervalRange;
    public Audio DestroyedSfx;
    public Audio WillhelmSfx;

    [Header("Particle Systems")]
    public GameObject HitParticleSystem;

    private Rigidbody _rigidbody;
    private float _currentBaseCanonAngleY;
    private float _currentCanonAngleX;
    private bool _isFiring;
    private bool _instantiateCannonBall;
    private float _currentCoolDown;
    private FracturedObject _fracturedObject;
    private float _waveSfxCooldown;
    private bool _isSailing;
    private GameObject _sailingSfx;
    

    private Vector3 AxisLeft
    {
        get { return new Vector3(Input.GetAxis("Horizontal" + Index), 0, -Input.GetAxis("Vertical" + Index)); }
    }

    private Vector3 AxisRight
    {
        get { return new Vector3(Input.GetAxis("HorizontalRight" + Index), 0, Input.GetAxis("VerticalRight" + Index)); }
    }

    public bool IsDead
    {
        get { return _health <= 0; }
    }

    #endregion
    #region Events

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _fracturedObject = GetComponentInChildren<FracturedObject>();
        _health = StartHealth;
        
        var mastMeshRenderer = Mast.GetComponentInChildren<MeshRenderer>();
        mastMeshRenderer.material = MastMaterial;

    }

    void FixedUpdate()
    {
        Debug.Log(_rigidbody.velocity);

        if (!BackMovement && AxisLeft.z > 0 && _rigidbody.velocity.magnitude < MaxSpeed)
        { 
            _rigidbody.AddForce(transform.forward * AxisLeft.z * ForwardForce);

            if (!_isSailing)
            {
                _sailingSfx = AudioManager.Instance.Play(WaveSfx, transform.position);
                _isSailing = true;
            }
        }
        else if (BackMovement)
        {
            _rigidbody.AddForce(transform.forward * AxisLeft.z * ForwardForce);
        }

        if (AxisLeft.x != 0)
        {
            if (!_isSailing)
            {
                _sailingSfx = AudioManager.Instance.Play(WaveSfx, transform.position);
                _isSailing = true;
            }

            //_waveSfxCooldown -= Time.deltaTime;

            //if (_waveSfxCooldown <= 0)
            //{
            //    AudioManager.Instance.Play(WaveSfx, transform.position);
            //    _waveSfxCooldown = Random.Range(WaveSfxRandomIntervalRange.x, WaveSfxRandomIntervalRange.y);
            //}

            _rigidbody.AddTorque(Vector3.up*AxisLeft.x*RotationForce);
            _rigidbody.AddForce(transform.forward * 1);
             //SteerWheel.transform.rotation = Quaternion.AngleAxis(RotationForce, Vector3.up);
        }

        if (AxisLeft.x == 0 && AxisLeft.z == 0)
        {
            Destroy(_sailingSfx);
            _isSailing = false;
        }

        if (_instantiateCannonBall)
        {
            var canonBall = CanonBallPrefab.Instantiate(CanonBallPosition.transform.position, Canon.transform.rotation);
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

    void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.layer == LayerMask.NameToLayer("CanonBall"))
        {
            _health -= GameManager.Instance.CanonBallDamageAmount;

            var go = HitParticleSystem.Instantiate(c.contacts[0].point, Quaternion.identity);
            go.transform.parent = transform;

            AudioManager.Instance.Play(HitSfx, transform.position);

            if (_health <= 0)
            {
                AudioManager.Instance.Play(DestroyedSfx, transform.position);
                AudioManager.Instance.Play(WillhelmSfx, transform.position);

                _fracturedObject.Explode(c.contacts[0].point, ExplosionForce);

                DestroyGameObject(BaseCanon, CannonDestroyForce, CannonDestroyTorque, Vector3.right);
                DestroyGameObject(Barrel, BarrelDestroyForce, BarrelDestroyTorque, Vector3.right);
                DestroyGameObject(Mast, MastDestroyForce, MastDestroyTorque, Vector3.up + Vector3.forward);
                DestroyGameObject(Front, 5, 5, Vector3.forward);
                DestroyGameObject(Steer, 5, 5, Vector3.forward);
                DestroyGameObject(CaptainRat, 30, 50, Vector3.forward);

                StartCoroutine(ShipDestroyed());
            }
            

            Destroy(c.gameObject);
        }
    }

    #endregion
    #region Methods

    private void DestroyGameObject(GameObject go, float force, float torque, Vector3 torqueAxis)
    {
        var rigidbody = go.GetComponent<Rigidbody>();

        if (rigidbody != null)
        {
            rigidbody.isKinematic = false;
            rigidbody.AddForce(Vector3.up * force, ForceMode.Impulse);
            rigidbody.AddTorque(torqueAxis * torque, ForceMode.Impulse);
        }
    }

    private void RotateBaseCannon()
    {
        if (!IsDead)
        { 
            _currentBaseCanonAngleY += AxisRight.x * BaseCanonRotationSpeedY;
            _currentBaseCanonAngleY = Mathf.Clamp(_currentBaseCanonAngleY, BaseCanonAngleLimit.x, BaseCanonAngleLimit.y);

            var newRotationY = Quaternion.AngleAxis(_currentBaseCanonAngleY + transform.rotation.eulerAngles.y, BaseCanon.transform.up);

            BaseCanon.transform.rotation = Quaternion.Slerp(BaseCanon.transform.rotation, newRotationY, Time.deltaTime * BaseCanonSlerpSpeedY);
            CaptainRat.transform.rotation =  BaseCanon.transform.rotation * Quaternion.Euler(0, 90, 0);
        }
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

            AudioManager.Instance.Play(CanonFireSfx, transform.position);
        }
        else if (Input.GetAxisRaw("Fire" + Index) == 0)
        {
            _isFiring = false;
        }

        _currentCoolDown -= Time.deltaTime;
    }

    IEnumerator ShipDestroyed()
    {
        yield return new WaitForSeconds(GameManager.Instance.ShipDestroyedIdleTime);

        SceneManager.LoadScene("StartScreen");
    }

    #endregion
}
