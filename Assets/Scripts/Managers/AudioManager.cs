using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicSource;

    [SerializeField]
    private AudioClip titleMusic;
    [SerializeField]
    private AudioClip goodGuyMusic;
    [SerializeField]
    private AudioClip badGuyMusic;

    public static AudioManager instance;

    void Awake(){
        instance = this;
    }

    public void ChangeMusicToBadGuy(){
        musicSource.clip = badGuyMusic;
        musicSource.Play();
    }

    public void ChangeMusicToGoodGuy(){
        musicSource.clip = goodGuyMusic;
        musicSource.Play();
    }

    public void ChangeMusicToTitle(){
        musicSource.clip = titleMusic;
        musicSource.Play();
    }
}
