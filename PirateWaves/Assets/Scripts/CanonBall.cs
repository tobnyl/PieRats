using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonBall : MonoBehaviour
{
    public Audio PlumsSfx;

	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision c)
    {        
        if (c.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            AudioManager.Instance.Play(PlumsSfx, transform.position);
            Destroy(gameObject);
        }
    }
}
