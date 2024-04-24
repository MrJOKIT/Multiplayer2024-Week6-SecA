using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Music
{
    public string nameMusic;
    public AudioClip audioClip;
}
[Serializable]
public class SoundSfx
{
    public string nameSound;
    public AudioClip soundClip;
}
public class SoundManager : MonoBehaviour
{
    public static SoundManager instancne;
    
    public List<Music> musics;
    public List<SoundSfx> sfxSound;
    public AudioSource soundEffectSource;
    public AudioSource musicSource;

    private void Awake()
    {
        if (instancne == null)
        {
            instancne = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        PlayMusic(0);
    }

    public void PlaySoundEffect(int numberSfx)
    {
        soundEffectSource.PlayOneShot(sfxSound[numberSfx].soundClip);
    }

    public void PlayMusic(int numberMusic)
    {
        musicSource.clip = musics[numberMusic].audioClip;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }
}
