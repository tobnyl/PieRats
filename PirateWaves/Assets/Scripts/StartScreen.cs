using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (Input.GetButtonDown("Submit"))
	    {
	        StartGame();
	    }	
	}

    public void StartGame()
    {
        SceneManager.LoadScene("main");
    }
}
