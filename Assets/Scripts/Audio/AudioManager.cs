using System;
using System.Collections;
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
    private FMOD.Studio.EventInstance ruleta;
    private FMOD.Studio.Bus masterBus;

    private uint maxGente = 300;
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

    public void startGame()
    {
        people = 0;
        maxGente = GameManager.Instance.GameInfo.WallSecondPeopleThreshold;
    }

    private void OnDestroy()
    {
        StopAndReleaseInstance(ref gameplayMusic);
        StopAndReleaseInstance(ref ambience);
        StopAndReleaseInstance(ref ruleta);
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            UpdateMusicParameter();
        }
    }

    private void StopAndReleaseInstance(ref FMOD.Studio.EventInstance instance)
    {
        if (instance.isValid())
        {
            instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            instance.release();
            instance.clearHandle();
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
        StopAndReleaseInstance(ref gameplayMusic);

        gameplayMusic = RuntimeManager.CreateInstance("event:/MUSIC/" + key);
        gameplayMusic.start();
    }

    public void ChangeMusic(string key)
    {
        StopAndReleaseInstance(ref gameplayMusic);

        gameplayMusic = RuntimeManager.CreateInstance("event:/MUSIC/" + key);
        gameplayMusic.start();
    }

    public void StopMusic()
    {
        StopAndReleaseInstance(ref gameplayMusic);
    }

    public void ResumeMusic()
    {
        if (gameplayMusic.isValid())
        {
            gameplayMusic.start();
        }
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
        PlaySound("subeGente");
    }
    public void RemovePeople(uint amount)
    {
        people -= amount;
        OnPeopleChanged(people);
        PlaySound("subeGente");
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
        print("hablando");
        var talkSound = RuntimeManager.CreateInstance("event:/SOUND EFFECTS/talk");
        talkSound.start();
        talkSound.release();
    }

    public void PlayClick1()
    {
        var clickSound = RuntimeManager.CreateInstance("event:/SOUND EFFECTS/click");
        clickSound.start();
        clickSound.release();
    }

    public void PlayClick2()
    {
        var clickNoSound = RuntimeManager.CreateInstance("event:/SOUND EFFECTS/click no");
        clickNoSound.start();
        clickNoSound.release();
    }

    public void PlayAmbience()
    {
        StopAndReleaseInstance(ref ambience);

        ambience = RuntimeManager.CreateInstance("event:/ambience");
        ambience.start();
    }

    public void StopAmbience()
    {
        StopAndReleaseInstance(ref ambience);
    }

    public void IniciaRuleta()
    {
        StopAndReleaseInstance(ref ruleta);

        ruleta = RuntimeManager.CreateInstance("event:/SOUND EFFECTS/ruleta");
        ruleta.start();
    }

    public void FinalizaRuleta(RouletteUI.Result resultado)
    {
        StopAndReleaseInstance(ref ruleta);

        string resultadoSound = resultado switch
        {
            RouletteUI.Result.Success=> "ganaRuleta",
            RouletteUI.Result.Fail=> "pierdeRuleta",
            RouletteUI.Result.Crit => "criticoRuleta",
            _ => throw new ArgumentOutOfRangeException(nameof(resultado), "Valor no válido para resultado de la ruleta")
        };

        PlaySound(resultadoSound);
    }
    
    public void FadeOutGameplayMusic(float duration)
    {
        if (gameplayMusic.isValid())
        {
            StartCoroutine(FadeOutCoroutine(duration));
        }
    }

    private IEnumerator FadeOutCoroutine(float duration)
    {
        float startVolume;
        gameplayMusic.getVolume(out startVolume);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float newVolume = Mathf.Lerp(startVolume, 0f, elapsed / duration);
            gameplayMusic.setVolume(newVolume);
            yield return null;
        }

        gameplayMusic.setVolume(0f);
        StopAndReleaseInstance(ref gameplayMusic);
    }
}
