using System;
using UnityEngine;
using FMODUnity;

public class AudioManager : MonoBehaviour, IAudioService
{
    private FMOD.Studio.EventInstance gameplayMusic;
    private FMOD.Studio.Bus masterBus;

    private uint maxGente = 100;


    private void Start()
    {
        masterBus = RuntimeManager.GetBus("bus:/");
        StartGameplayMusic();
    }

    private void OnDestroy()
    {
        gameplayMusic.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        gameplayMusic.release();
    }

    public void PlaySound(string key)
    {
        var sound = RuntimeManager.CreateInstance("event:/" + key);
        sound.start();
        sound.release();
    }

    public void StartGameplayMusic()
    {
        if (!gameplayMusic.isValid())
        {
            gameplayMusic = RuntimeManager.CreateInstance("event:/MUSIC/Level");
            gameplayMusic.start();
        }
    }


    public void ChangeMusic(string key)
    {
        if (gameplayMusic.isValid())
        {
            gameplayMusic.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            gameplayMusic.release();
        }

        gameplayMusic = RuntimeManager.CreateInstance("event:/" + key);
        gameplayMusic.start();
    }


    public void StopMusic()
    {
        gameplayMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public void ResumeMusic()
    {
        gameplayMusic.start();
    }

    private void OnPeopleChanged(uint people)
    {
        float proportion = people / (float)maxGente;
        if (gameplayMusic.isValid())
        {
            gameplayMusic.setParameterByName("seguidores", proportion);
        }
    }

}
