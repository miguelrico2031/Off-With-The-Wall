using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsManager : MonoBehaviour
{
    private void Start()
    {
        AudioManager.Instance.ChangeMusic("Credits");   
        AudioManager.Instance.StopAmbience();
    }

    public void RestartGame()
    {
        Destroy(AudioManager.Instance);
        SceneManager.LoadScene(0);
    }
}
