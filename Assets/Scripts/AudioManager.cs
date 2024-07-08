using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource[] musics;
    public AudioSource[] SFXs;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(int index) 
    {
        for (int i = 0; i < musics.Length; ++i)
        {
            if (i==index) 
            {
                if (!musics[i].isPlaying)
                {
                    musics[i].Play();
                }
            }
            else 
            {
                musics[i].Stop();
            }
        }
    }

    public void PlaySFX(int index)
    {
        SFXs[index].Stop();
        SFXs[index].Play();
    }

    public void PlaySFXWithRandomPitch(int index)
    {
        SFXs[index].pitch=Random.Range(0.8f,1.2f);
        PlaySFX(index);
    }
}
