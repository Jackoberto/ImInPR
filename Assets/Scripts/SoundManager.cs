﻿using System;
using System.Collections;
using UnityEngine;
public class SoundManager : MonoBehaviour
{
    [Header("Game Sounds")]
    [SerializeField] public AudioSource gameSoundAudioSource;
    [SerializeField] private GameSoundController gameSoundController;
    [Range(0,1f)]public float gameSoundVolume = 0.5f;
    
    [Header("Game Music")]
    [SerializeField] public AudioSource musicAudioSource;
    [SerializeField] private GameSoundController musicController;
    [Range(0,1f)] public float musicVolume = 0.5f;
    [SerializeField] private GameObject nowPlayingPrefab;

    [Header("Background Music")] 
    [SerializeField] private AudioSource backgroundMusicPlayer;
    [SerializeField] private GameSound idleMusic;
    [Range(0,1f)]public float backgroundMusicVolume = 0.3f;

    private void Start()
    {
        playBackgroundMusic();
    }

    public float BackgroundMusicVolume
    {
        get => PlayerPrefs.GetFloat("BackGroundVolume", 0.3f);
        set
        {
            PlayerPrefs.SetFloat("BackGroundVolume", value);
            backgroundMusicPlayer.volume = BackgroundMusicVolume;
        }
    }

    public float MusicVolume
    {
        get => PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        set => PlayerPrefs.SetFloat("MusicVolume", value);
    }
    
    public float GameSoundVolume
    {
        get => PlayerPrefs.GetFloat("GameSoundVolume", 0.5f);
        set => PlayerPrefs.SetFloat("GameSoundVolume", value);
    }

    #region GameSound Player

    public void PlayGameSound(GameSound sound)
    {
        Play(sound, gameSoundAudioSource, GameSoundVolume);
    }
    
    public void PlayGameSound(string soundName)
    {
        Play(gameSoundController.FindGameSound(soundName), gameSoundAudioSource, gameSoundVolume);
    }

    private void Play(GameSound sound, AudioSource source, float volume)
    {
        if (backgroundMusicPlayer.isPlaying)
        {
            backgroundMusicPlayer.Pause();
        }
            
        source.PlayOneShot(sound.clip, volume);
    }
    #endregion
    
    
    #region Music Player
    public void PlayMusic(Band band) 
    {
        PlayMusic(band.song, MusicVolume);
    }
    
    public void PlayMusic(string songName)
    {
        var sound = (GameSong)musicController.FindGameSound(songName);
        PlayMusic(sound, MusicVolume);
    }
    
    private void PlayMusic(GameSong sound, float volume)
    {
        if (sound != null)
        {
            if(musicAudioSource.isPlaying)
                musicAudioSource.Stop();
            
            backgroundMusicPlayer.Pause();
            StartCoroutine(resumePlayingAfter(sound.clip.length));
            Play(sound, musicAudioSource, volume);
            var instance = Instantiate(nowPlayingPrefab, this.transform);
            instance.GetComponent<NowPlaying>().Setup(sound.bandName, sound.songName, sound.soundDesignerName);
        }
    }

    IEnumerator resumePlayingAfter(float t)
    {
        yield return new WaitForSeconds(t);
        backgroundMusicPlayer.UnPause();
    }
    public void PauseToggleMusic()
    {
        if(musicAudioSource.isPlaying)
            musicAudioSource.Pause();
        else
            musicAudioSource.UnPause();
    }
    #endregion

    #region backgroundMusic
    public void playBackgroundMusic()
    {
        backgroundMusicPlayer.loop = true;
        backgroundMusicPlayer.clip = idleMusic.clip;
        backgroundMusicPlayer.volume = BackgroundMusicVolume;
        backgroundMusicPlayer.Play();
    }
    #endregion
}