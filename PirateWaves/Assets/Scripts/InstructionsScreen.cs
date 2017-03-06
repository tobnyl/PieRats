using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InstructionsScreen : MonoBehaviour 
{
    #region Fields/Properties

    public GameObject Background;

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
            StartGame();
        }
    }

    #endregion
    #region Methods

    private void StartGame()
    {
        Background.gameObject.SetActive(false);

        SceneManager.LoadScene("Main");
    }

    #endregion

}