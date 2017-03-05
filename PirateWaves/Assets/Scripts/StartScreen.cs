using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour
{

    public Image Background;

    public GameObject StartItem;
    public GameObject InstructionsItem;
    public GameObject QuitItem;
    
    private GameObject _currentItem;

	// Use this for initialization
	void Start ()
    {
        _currentItem = StartItem;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetButtonDown("Submit"))
        {
            if (_currentItem == StartItem)
            {
                StartGame();
            }
        }
	}

    public void StartGame()
    {
        Background.gameObject.SetActive(false);

        SceneManager.LoadScene("main");
    }
}
