using System;
using UnityEngine;
using FMODUnity;

public class AudioManager : MonoBehaviour, IAudioService
{
    private static AudioManager _instance;

    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<AudioManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("AudioManager");
                    _instance = go.AddComponent<AudioManager>();
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }
    }

    private FMOD.Studio.EventInstance gameplayMusic;
    private FMOD.Studio.EventInstance ambience;
    private FMOD.Studio.Bus masterBus;

    private uint maxGente = 100;
    private uint people = 0;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        masterBus = RuntimeManager.GetBus("bus:/");
    }

    private void OnDestroy()
    {
        gameplayMusic.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        gameplayMusic.release();
        StopAmbience();
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            UpdateMusicParameter();
        }
    }






    public void PlaySound(string key)
    {
        var sound = RuntimeManager.CreateInstance("event:/SOUND EFFECTS/" + key);
        sound.start();
        sound.release();
    }

    public void StartGameplayMusic(string key)
    {
        if (!gameplayMusic.isValid())
        {
            gameplayMusic = RuntimeManager.CreateInstance("event:/MUSIC/"+key);
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

        gameplayMusic = RuntimeManager.CreateInstance("event:/MUSIC/" + key);
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

    public void AddPeople(uint amount)
    {
        people += amount;
        OnPeopleChanged(people);
    }

    private void UpdateMusicParameter()
    {
        float proportion = people / (float)maxGente;
        if (gameplayMusic.isValid())
        {
            gameplayMusic.setParameterByName("seguidores", proportion);
        }
    }

    public void PlayTalkSound()
    {
        var talkSound = RuntimeManager.CreateInstance("event:/SOUND EFFECTS/talk");
        talkSound.start();
        talkSound.release();
        Debug.Log("Talk");
    }

    public void PlayClick1()
    {
        var clickSound = RuntimeManager.CreateInstance("event:/SOUND EFFECTS/click");
        clickSound.start();
        clickSound.release();
        Debug.Log("Click1");
    }

    public void PlayClick2()
    {
        var clickNoSound = RuntimeManager.CreateInstance("event:/SOUND EFFECTS/click no");
        clickNoSound.start();
        clickNoSound.release();
        Debug.Log("Click2");
    }

    public void PlayAmbience()
    {
        if (!ambience.isValid())
        {
            ambience = RuntimeManager.CreateInstance("event:/ambience");
            ambience.start();
            Debug.Log("Ambience started");
        }
    }

    public void StopAmbience()
    {
        if (ambience.isValid())
        {
            ambience.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            ambience.release();
            Debug.Log("Ambience stopped");
        }
    }
}
