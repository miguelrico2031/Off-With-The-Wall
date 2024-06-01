using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CreditsManager : MonoBehaviour
{
    private void Start()
    {
        AudioManager.Instance.ChangeMusic("Credits");   
        AudioManager.Instance.StopAmbience();
        if (GameManager.Instance != null)
        {
            GetComponentInChildren<TMP_Text>().text += "\n Thank you and " + GameManager.Instance.GameInfo.OrgSlogan;
        }
    }

    public void RestartGame()
    {
        AudioManager.Instance.FadeOutGameplayMusic(3);
        StartCoroutine(death());
    }
    public IEnumerator death()
    {
        yield return new WaitForSecondsRealtime(3.2f);
        Destroy(AudioManager.Instance.gameObject);
        if (GameManager.Instance != null)
        {
            GameManager.Instance.destroySelf();
        }
        SceneManager.LoadScene(0);
    }
}
