using UnityEngine;

public class AudioPrueba : MonoBehaviour
{
    private void Start()
    {
        AudioManager.Instance.StartGameplayMusic("Level");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //AudioManager.Instance.PlayTalkSound();
            //AudioManager.Instance.AddPeople(1);
            //AudioManager.Instance.ResumeMusic();
            AudioManager.Instance.AddPeople(1);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            //AudioManager.Instance.PlayClick1();
            AudioManager.Instance.ChangeMusic("Final");
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            AudioManager.Instance.StopMusic();
            //AudioManager.Instance.PlayClick2();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            AudioManager.Instance.PlaySound("pop");
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            AudioManager.Instance.PlaySound("loseMulti");
        }
    }
}
