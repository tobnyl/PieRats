using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System;

[Serializable]
public class Audio
{
    public AudioClip Clip;
    public AudioMixerGroup Output;
    [Range(0,1)]
    public float MinVolume = 1.0f;
    [Range(0, 1)]
    public float MaxVolume = 1.0f;
    [Range(0, 3)]
    public float MinPitch = 1.0f;
    [Range(0, 3)]
    public float MaxPitch = 1.0f;
    public bool Loop;
    public bool Mute;

	public Audio()
	{
		MinVolume = 1.0f;
		MaxVolume = 1.0f;
		MinPitch = 1.0f;
		MaxPitch = 1.0f;
	}

    public bool IsNull()
    {
        return Clip == null;
    }
}
