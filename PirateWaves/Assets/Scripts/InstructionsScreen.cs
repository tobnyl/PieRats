﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InstructionsScreen : MonoBehaviour 
{
    #region Fields/Properties

    public GameObject Background;
    public SceneLoader LoadingScreen;

    #endregion
    #region Events

    void Awake()
	{
		
	}
	
	void Start() 
	{
	
	}

    void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            LoadingScreen.LoadScene();
        }
    }

    #endregion
    #region Methods

    #endregion

}