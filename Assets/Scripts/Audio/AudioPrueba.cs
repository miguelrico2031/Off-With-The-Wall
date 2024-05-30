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
            AudioManager.Instance.IniciaRuleta();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            AudioManager.Instance.FinalizaRuleta(0);
            //AudioManager.Instance.PlayClick2();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            AudioManager.Instance.PlaySound("periodico");
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            AudioManager.Instance.PlaySound("loseMulti");
        }
    }
}
