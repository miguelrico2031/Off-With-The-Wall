using System;
using UnityEngine;


public class AudioManager : MonoBehaviour, IAudioService
{
    private void Start()
    {
        GameManager.Instance.Get<IPeopleService>().OnPeopleChanged.AddListener(OnPeopleChanged);
    }

    private void OnDestroy()
    {
        GameManager.Instance.Get<IPeopleService>().OnPeopleChanged.RemoveListener(OnPeopleChanged);
    }

    public void PlaySound(string key)
    {
        //implementala anton
    }
    
    public void StartGameplayMusic()
    {
            
    }
    
    public void ChangeMusic(string key) //para cambiar de cancion (en la cinematica final etc)
    {
            
    }
    
    public void StopMusic()
    {
            
    }
    
    public void ResumeMusic()
    {
            
    }
    
    
    private void OnPeopleChanged(uint people)
    {
        //se llama cada vez que cambia la gente, aqui cambiarian los instrumentos y tal segun la gente que haya
    }
}