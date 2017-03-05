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

    public Color MenuItemColor;
    public Color MenuItemSelectedColor;
    public GameObject Instructions;

    private GameObject _currentItem;
    private int _currentIndex;

    private bool _verticalDown = false;
    private List<GameObject> _menuItems;

    // Use this for initialization
    void Start ()
    {
        _currentItem = StartItem;

        _menuItems = new List<GameObject>();
        _menuItems.Add(StartItem);
        _menuItems.Add(InstructionsItem);
        _menuItems.Add(QuitItem);
	}
	
	// Update is called once per frame
	void Update ()
    {
        var verticalRaw = -(int)Input.GetAxisRaw("Vertical") | -(int)Input.GetAxisRaw("DPadVertical");

        if (verticalRaw > 0 && !_verticalDown)
        {
            _currentIndex = (_currentIndex + 1) % 3;
            _verticalDown = true;
        }
        else if (verticalRaw < 0 && !_verticalDown)
        {
            _currentIndex = (_currentIndex - 1) % 3;
            _verticalDown = true;

            if (_currentIndex < 0)
            {
                _currentIndex = 2;
            }
        }
        else if (verticalRaw == 0)
        {
            _verticalDown = false;
        }

        foreach (var menuItem in _menuItems)
        {
            _currentItem.GetComponent<Text>().color = MenuItemColor;
        }

        _currentItem = _menuItems[_currentIndex];
        _currentItem.GetComponent<Text>().color = MenuItemSelectedColor;        
        
        if (Input.GetButtonDown("Submit"))
        {
            if (_currentItem == StartItem)
            {
                StartGame();
            }
            else if (_currentItem == InstructionsItem)
            {                
                Instructions.SetActive(true);

                // TODO: back to menu
                if (Input.GetButtonDown("Cancel"))
                {
                    Instructions.SetActive(false);
                }
            }
            else if (_currentItem == QuitItem)
            {
                #if UNITY_EDITOR                    
                    UnityEditor.EditorApplication.isPlaying = false;
                #endif

                Application.Quit();
            }
        }
	}

    public void StartGame()
    {
        Background.gameObject.SetActive(false);

        SceneManager.LoadScene("main");
    }
}
