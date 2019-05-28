using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
  public string name;
  public AudioClip clip;
  private AudioSource source;

  [Range(0.0f, 1.0f)]
  public float volume = 0.7f;
  [Range(0.0f, 1.0f)]
  public float pitch = 1.0f;

  [Range(0.0f, 0.5f)]
  public float randomVolume = 0.1f;
  [Range(0.0f, 0.5f)]
  public float randomPitch = 0.1f;


  public void SetSource(AudioSource _source)
  {
    source = _source;
    source.clip = clip;
  }

  public void Play()
  {
    source.volume = volume * (1 + Random.Range(-randomVolume / 2f, randomVolume / 2f));
    source.pitch = pitch * (1 + Random.Range(-randomPitch / 2f, randomPitch / 2f));
    source.maxDistance = 100.0f;
    source.spatialBlend = 1.0f;
    source.rolloffMode = AudioRolloffMode.Linear;
    source.dopplerLevel = 0.0f;
    source.Play();
  }

  public void PlayOneShot()
  {
    source.volume = volume * (1 + Random.Range(-randomVolume / 2f, randomVolume / 2f));
    source.pitch = pitch * (1 + Random.Range(-randomPitch / 2f, randomPitch / 2f));
    source.PlayOneShot(clip);
  }


}

public class AudioManager : MonoBehaviour
{
  [SerializeField]
  Sound[] sounds;
  public static AudioManager instance = null;

  private GameObject oneShotGameObject;
  private AudioSource oneShotAudioSource;
  private Dictionary<string, float> soundTimerDictionary;

  void Awake ()
  {
    //Check if there is already an instance of SoundManager
    if (instance == null)
    {
      //if not, set it to this.
      instance = this;
    }
    //If instance already exists:
    else if (instance != this)
    {
      //Destroy this, this enforces our singleton pattern so there can only be one instance of SoundManager.
      Destroy (gameObject);
    }

    //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
    DontDestroyOnLoad (gameObject);
    Initialize();
  }

  public void Initialize()
  {
    soundTimerDictionary = new Dictionary<string, float>();
    soundTimerDictionary["footstep"] = 0.0f;
  }

  private Sound GetAudioClip(string _name)
  {
    for(int i = 0; i < sounds.Length; i++)
    {
      if(sounds[i].name == _name)
      {
        return sounds[i];
      }
    }
    Debug.LogWarning("AudioManager: Sound not found in list, " + _name);
    return null;
  }

  public void PlaySound(string soundName)
  {
    Sound sound = GetAudioClip(soundName);
    if(CanPlaySound(sound))
    {
      if(oneShotGameObject == null)
      {
        oneShotGameObject = new GameObject("Sound_" + sound.name);
        oneShotAudioSource = oneShotGameObject.AddComponent<AudioSource>();
        oneShotGameObject.transform.SetParent(transform);
        sound.SetSource(oneShotAudioSource);
        sound.PlayOneShot();
      }
    }
  }

  public void PlaySound(string soundName, Vector3 position)
  {
    Sound sound = GetAudioClip(soundName);
    if(CanPlaySound(sound))
    {
      GameObject soundGameObject = new GameObject("Sound_" + sound.name);
      AudioSource source = soundGameObject.AddComponent<AudioSource>();
      soundGameObject.transform.SetParent(transform);
      soundGameObject.transform.position = position;
      sound.SetSource(source);
      sound.Play();

      Object.Destroy(soundGameObject, source.clip.length);
    }
  }

  private bool CanPlaySound(Sound sound)
  {
    switch(sound.name)
    {
      default:
        return true;
      case "footstep":
        if(soundTimerDictionary.ContainsKey(sound.name))
        {
          float lastTimePlayed = soundTimerDictionary[sound.name];
          float playerMoveTimerMax = 0.25f;
          if(lastTimePlayed + playerMoveTimerMax < Time.time)
          {
            soundTimerDictionary[sound.name] = Time.time;
            return true;
          } else
          {
            return false;
          }
        }
      break;
    }
    return true;
  }


}
