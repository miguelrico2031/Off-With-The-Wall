using System;
using UnityEngine;
using FMODUnity;  // Importante para interactuar con FMOD

public class AudioManager : MonoBehaviour, IAudioService
{
    private FMOD.Studio.EventInstance gameplayMusic; // Instancia para manejar la música de juego
    private FMOD.Studio.Bus masterBus; // Para controlar el bus maestro, por ejemplo para pausas

    private void Start()
    {
        //GameManager.Instance.Get<IPeopleService>().OnPeopleChanged.AddListener(OnPeopleChanged);

        masterBus = RuntimeManager.GetBus("bus:/"); // Asegúrate de poner la ruta correcta del bus en FMOD
        StartGameplayMusic();

        //PlaySound("MUSIC/Credits");
    }

    private void OnDestroy()
    {
        //GameManager.Instance.Get<IPeopleService>().OnPeopleChanged.RemoveListener(OnPeopleChanged);
        gameplayMusic.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        gameplayMusic.release();
    }

    public void PlaySound(string key)
    {
        var sound = RuntimeManager.CreateInstance("event:/" + key); // Asegúrate que las rutas coincidan con tus eventos en FMOD
        sound.start();
        sound.release();
    }

    public void StartGameplayMusic()
    {
        gameplayMusic = RuntimeManager.CreateInstance("event:/MUSIC/Level");
        gameplayMusic.start();
    }

    public void ChangeMusic(string key)
    {
        gameplayMusic.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        gameplayMusic.release();
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
        gameplayMusic.setParameterByName("PeopleCount", people);
    }
}
