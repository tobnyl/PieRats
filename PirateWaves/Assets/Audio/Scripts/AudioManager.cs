using UnityEngine;
using UE = UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using System.Linq;
using System;

public class QueuedAudio
{
    public string Name { get; set; }
    public Vector3 Position { get; set; }
}

[Serializable]
public class MinMax
{
    public float Min;
    public float Max;
}

public class AudioManager : MonoBehaviour
{
	#region Public Fields

	public AudioMixer Mixer;
	public AudioSource AudioSourcePrefab;
    public AudioMixerGroup Master;
    public bool AllowDuplicatesPerFrame;
    public Audio Music;
	public MinMax MusicLowPassCutoffFreq;
	public bool MuteAllSfx;

	#endregion
	#region Properties

	public static AudioManager Instance
    {
        get { return _instance; }
    }

	#endregion
	#region Private Fields

	private static AudioManager _instance;    
    private List<QueuedAudio> _clipList;
    private float _audioSourceCollidingDistance;
    private AudioSource _musicAudioSource;
	private float _currentMusicLowPassCutoffFreq;


	#endregion
	#region Events

	void Awake()
    {
        _clipList = new List<QueuedAudio>();
        _audioSourceCollidingDistance = AudioSourcePrefab.minDistance * 2f;
		_currentMusicLowPassCutoffFreq = MusicLowPassCutoffFreq.Max;


		InitializeMusic(Music);

        if (_instance == null)
        {
            _instance = GetComponent<AudioManager>();
        }

        DontDestroyOnLoad(gameObject);
    }

	void LateUpdate()
	{
		_clipList.Clear();
	}

	#endregion
	#region Public Methods

	public void PlayMusic()
	{
		_musicAudioSource.Play();
	}

	public void StopMusic()
	{
		_musicAudioSource.Stop();
	}

	public void PauseLoopingSfxAndEnableMusicFilters(bool pause)
	{
		var audioSources = GetComponentsInChildren<AudioSource>().Where(a => a.loop == true);

		//Mixer.SetFloat("MusicLowPassCutoff", (pause ? MusicLowPassCutoffFreq.Min : MusicLowPassCutoffFreq.Max));

		foreach (var audioSource in audioSources)
		{
			if (pause)
			{
				Mixer.SetFloat("MusicLowPassCutoff", MusicLowPassCutoffFreq.Min);

				if (audioSource.outputAudioMixerGroup.name != "Music")
				{ 
					audioSource.Pause();
				}
			}
			else
			{
				Mixer.SetFloat("MusicLowPassCutoff", MusicLowPassCutoffFreq.Max);

				if (audioSource.outputAudioMixerGroup.name != "Music")
				{					
					audioSource.UnPause();
				}
			}
		}
	}

	public GameObject Play(Audio audio, Vector3 position, bool muteAllActiveSfx = false)
	{
		GameObject _audioSource = null;

		if (muteAllActiveSfx)
		{
			var sfxPlaying = GetComponentsInChildren<AudioSource>();

			foreach (var sfx in sfxPlaying)
			{
				sfx.Stop();
			}

			StartCoroutine(MuteAllActiveSfxDuration(audio.Clip.length));
		}

		if (!audio.IsNull() && !audio.Mute && !MuteAllSfx && !AlreadyInQueueAndOutOfDistance(audio, position))
		{
			_audioSource = Play(audio.Clip, audio.Output, position, audio.Loop, audio.MinVolume, audio.MaxVolume, audio.MinPitch, audio.MaxPitch);
		}

		if (muteAllActiveSfx)
		{
			MuteAllSfx = muteAllActiveSfx;
			_musicAudioSource.Stop();
		}

		return _audioSource;
	}

	#endregion
	#region Private Methods

	private void InitializeMusic(Audio music)
	{
		_musicAudioSource = gameObject.AddComponent<AudioSource>();
		_musicAudioSource.clip = music.Clip;
		_musicAudioSource.outputAudioMixerGroup = (music.Output != null ? music.Output : Master);
		_musicAudioSource.loop = music.Loop;
		_musicAudioSource.mute = music.Mute;
		_musicAudioSource.Play();
	}

	private bool AlreadyInQueueAndOutOfDistance(Audio audio, Vector3 position)
	{
		var clips = _clipList.Where(x => x.Name == audio.Clip.name);

		foreach (var clip in clips)
		{
			if (Vector3.Distance(clip.Position, position) < _audioSourceCollidingDistance)
			{
				return true;
			}
		}

		return false;
	}

	private GameObject Play(AudioClip clip, AudioMixerGroup group, Vector3 position, bool loop, float minVol = 1.0f, float maxVol = 1.0f, float minPitch = 1.0f, float maxPitch = 1.0f)
	{
		AudioSource audioSource = Instantiate(AudioSourcePrefab, transform.position, transform.rotation) as AudioSource;
		audioSource.transform.parent = this.transform;
		audioSource.transform.position = position;
		audioSource.loop = loop;

		if (minPitch != 1.0f || maxPitch != 1.0f)
		{
			audioSource.pitch = UE.Random.Range(minPitch, maxPitch);
		}

		if (minVol != 1.0f || maxVol != 1.0f)
		{
			audioSource.volume = UE.Random.Range(minVol, maxVol);
		}

		AudioSourceExtended audioSourceExtended = audioSource.GetComponent<AudioSourceExtended>();
		audioSourceExtended.Duration = clip.length;
		audioSourceExtended.Loop = loop; // I don't like this code...


		audioSource.clip = clip;
		audioSource.outputAudioMixerGroup = (group != null ? group : Master);
		audioSource.Play();

		if (!AllowDuplicatesPerFrame)
		{
			_clipList.Add(new QueuedAudio { Name = clip.name, Position = position });
		}

		return audioSource.gameObject;
	}

	#endregion
	#region Coroutines

	IEnumerator MuteAllActiveSfxDuration(float length)
    {
        yield return new WaitForSeconds(length);

        MuteAllSfx = false;
        _musicAudioSource.Play();
    }

	#endregion
}
