using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public List<Sound> sounds;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Debug.LogError($"AUDIO_MANAGER::INSTANCE -> An Instance of AudioManager already exists!");

        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;

            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            
            sound.source.loop = sound.loop;
            sound.source.playOnAwake = sound.playOnAwake;
        }
    }

    private void Start()
    {
        Play("Main Theme");
    }

    public void Play(string name)
    {
        Sound s = sounds.Where(s => s.name == name).FirstOrDefault();

        if (s == null)
        {
            Debug.LogWarning($"AUDIO_MANAGER::INSTANCE::PLAY -> Sound: '{name}' not found!");
            return;
        }
            
        s.source.Play();
    }
}
