using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int CanonBallDamageAmount = 20;
    public int ShipDestroyedIdleTime = 10;

    [Header("Sfx")]
    public Audio SeaGullSfx;
    public Vector2 SeaGullSfxRandomIntervalRange;

    private static GameManager _instance;
    private float _currentSeaGullSfxTime;

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

	    StartCoroutine(SeaGullSfxCoroutine());

        DontDestroyOnLoad(gameObject);
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    IEnumerator SeaGullSfxCoroutine()
    {
        while (true)
        {
            AudioManager.Instance.Play(SeaGullSfx, transform.position);
            _currentSeaGullSfxTime = Random.Range((int)SeaGullSfxRandomIntervalRange.x, (int)SeaGullSfxRandomIntervalRange.y);

            yield return new WaitForSeconds(_currentSeaGullSfxTime);
        }
    }
}
