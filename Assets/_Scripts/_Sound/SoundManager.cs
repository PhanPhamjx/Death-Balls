using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    [Header(" music theme")]
    public AudioClip inGame;
    public AudioClip blow;
    public AudioClip hit;
    public AudioClip col;
    public AudioClip clickMenu;

    private AudioSource fxSource;
    public AudioSource musicSource;
    // am luong
    public float musicVolume = 1.0f;
    public float fxVolume = 1.0f;
    private void Awake()
    {
        
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            fxSource = gameObject.AddComponent<AudioSource>();
            musicSource = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    void Update()
    {
        
    }
    public void ClickMenu ()
    {
        if (clickMenu != null && fxSource != null)
        {

            fxSource.PlayOneShot(clickMenu);  // Phát âm thanh click menu
        }
    }
    public void PlayMusic(AudioClip clip)
    {
        // Kiểm tra xem nhạc đang phát có phải nhạc hiện tại không
        if (musicSource.clip == clip && musicSource.isPlaying)
        {
            return; // Nếu nhạc đã phát đúng và đang phát thì không làm gì
        }

        // Nếu nhạc không phát hoặc không đúng, thiết lập lại
        musicSource.clip = clip;
        musicSource.volume = musicVolume;
        musicSource.loop = true;
        musicSource.Play();
        Debug.Log("dang phat nhac");
    }

    public void PlaySfX(AudioClip clip)
    {
        fxSource.PlayOneShot(clip,fxVolume);
    }
    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        musicSource.volume = musicVolume;
    }
    public void SetSFXVolume(float volume)
    {
        fxVolume = volume;
    }
    public void PauseMusic()
    {
        if (musicSource != null && musicSource.isPlaying)
        {
            musicSource.Pause();  // Tạm dừng nhạc nền
        }
    }

    public void ResumeMusic()
    {
        if (musicSource != null && !musicSource.isPlaying)
        {
            musicSource.UnPause();  // Tiếp tục phát nhạc nền
        }
    }
}
