using UnityEngine;
using System.Collections;

public class AudioSourceExtended : MonoBehaviour
{
    public float Duration { get; set; }
    public bool Loop { get; set; }

    void Start()
    {
        if (!Loop)
        {
            Invoke("Destroy", Duration);
        }
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}
