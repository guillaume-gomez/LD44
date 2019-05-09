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
    source.Play();
  }


}

public class AudioManager : MonoBehaviour
{
  [SerializeField]
  Sound[] sounds;
  public static AudioManager instance = null;

  private static Dictionnary<Sound, float> soundTimerDictionary;

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
  }

  private AudioClip GetAudioClip(string _name)
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

  public void PlaySound(Sound sound)
  {

  }


}
