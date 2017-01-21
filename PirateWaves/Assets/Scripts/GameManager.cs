using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int CanonBallDamageAmount = 20;

    private static GameManager _instance;

    public static GameManager Instance
    {
        get { return _instance; }
    }

	// Use this for initialization
	void Start ()
	{
	    if (_instance == null)
	    {
	        _instance = this;
	    }

        DontDestroyOnLoad(gameObject);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
