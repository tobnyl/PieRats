using UnityEngine;
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
            //Background.gameObject.SetActive(false);
            LoadingScreen.LoadScene();
            //StartGame();
        }
    }

    #endregion
    #region Methods

    //private void StartGame()
    //{
    //    Background.gameObject.SetActive(false);

    //    Debug.Log("Loading scene...");

    //    SceneManager.LoadScene("Main");

    //    Debug.Log("Done loading...");
    //}

    #endregion

}