using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour
{

    public Image Background;

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
        Background.gameObject.SetActive(false);

        SceneManager.LoadScene("main");
    }
}
