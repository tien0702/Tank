using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LTAUnityBase.Base.DesignPattern;
public class SoundController : MonoBehaviour
{
    AudioSource audioSource;

    Dictionary<string, AudioClip> dic_Name_AudioClip = new Dictionary<string, AudioClip>();

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        audioSource = GetComponent<AudioSource>();
        AudioClip[] audioClips = Resources.LoadAll<AudioClip>("Sounds");
        foreach(AudioClip audioClip in audioClips)
        {
            dic_Name_AudioClip.Add(audioClip.name, audioClip);
        }
        
    }

    public void PlayMusic(string musicName)
    {
        if (!dic_Name_AudioClip.ContainsKey(musicName))
        {
            Debug.LogError("music Name :" + musicName +" is not exist");
            return;
        }
        audioSource.clip = dic_Name_AudioClip[musicName];
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlaySound(string soundName)
    {
        if (!dic_Name_AudioClip.ContainsKey(soundName))
        {
            Debug.LogError("music Name :" + soundName + " is not exist");
            return;
        }
        audioSource.PlayOneShot(dic_Name_AudioClip[soundName]);
    }
}

public class Sound : SingletonMonoBehaviour<SoundController>
{
}