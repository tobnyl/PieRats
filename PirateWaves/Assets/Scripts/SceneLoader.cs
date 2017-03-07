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
    [SerializeField]
    private Text loadingText;

    private Image _image;
    private Transform _child;
    private CanvasGroup _canvasGroup;
    private List<Shadow> _shadowList;
    private List<Outline> _outlineList;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _child = transform.GetChild(0);
        loadingText = _child.GetComponent<Text>();

        _shadowList = _child.GetComponentsInChildren<Shadow>().ToList();
        _outlineList = _child.GetComponentsInChildren<Outline>().ToList();
        _canvasGroup = _child.GetComponentInChildren<CanvasGroup>();
    }

    // Updates once per frame
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


    // The coroutine runs on its own at the same time as Update() and takes an integer indicating which scene to load.
    IEnumerator LoadNewScene()
    {

        // This line waits for 3 seconds before executing the next line in the coroutine.
        // This line is only necessary for this demo. The scenes are so simple that they load too fast to read the "Loading..." text.
        //yield return new WaitForSeconds(10);

        // Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
        AsyncOperation async = SceneManager.LoadSceneAsync(scene);

        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
        while (!async.isDone)
        {
            print("%: " + async.progress);
            yield return null;
        }

        
    }

}