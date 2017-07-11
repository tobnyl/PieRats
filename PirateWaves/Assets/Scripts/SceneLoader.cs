using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Collections.Generic;

public class SceneLoader : MonoBehaviour
{
    private bool _loadScene = false;

    [SerializeField]
    private int scene;

    private Image _image;
    private Transform _child;
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _child = transform.GetChild(0);
        _canvasGroup = _child.GetComponentInChildren<CanvasGroup>();
    }

    void Update()
    {
        if (_loadScene == true)
        {
            _canvasGroup.alpha = Mathf.PingPong(Time.time, 1);
        }
    }

    private Color PulseColor(Color color)
    {
        return new Color(color.r, color.g, color.b, Mathf.PingPong(Time.time * 0.5f, 1));
    }

    public void LoadScene()
    {        
        if (_loadScene == false)
        {
            _child.gameObject.SetActive(true);
            _image.enabled = true;
                        
            _loadScene = true;

            StartCoroutine(LoadNewScene());
        }
    }

    IEnumerator LoadNewScene()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(scene);

        while (!async.isDone)
        {
            print("%: " + async.progress);
            yield return null;
        }

        Debug.Log("it are done!");
    }

}