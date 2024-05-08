


public interface IAudioService : IService
{
    public void PlaySound(string key);

    public void StartGameplayMusic();
    public void ChangeMusic(string key);
    public void StopMusic();
    public void ResumeMusic();
}
