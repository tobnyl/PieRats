using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    #region Fields/Properties

    public GameObject Target;
    public float DistanceFromTarget = 5.0f;
    public float OffsetY;

    private Vector3 _offset;
    private Vector3 _offsetY;

    #endregion

    #region Events

    // Use this for initialization
    void Start ()
    {
        transform.position = Target.transform.position - Target.transform.forward * DistanceFromTarget;              
        transform.rotation = Quaternion.Euler(0, Target.transform.rotation.eulerAngles.y, 0);

        _offset = transform.position - Target.transform.position;
        _offsetY = new Vector3(0, OffsetY, 0);

    }

    // Update is called once per frame
    void Update () {
		
	}

    void LateUpdate()
    {
        transform.position = _offset + Target.transform.position + _offsetY;
        transform.rotation = Quaternion.Slerp(transform.rotation, Target.transform.rotation, Time.deltaTime*2);
    }

    #endregion
}
