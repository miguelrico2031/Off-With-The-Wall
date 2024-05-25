using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLoseUIManager : MonoBehaviour, IStartLoseUIService
{
    [SerializeField] private CanvasGroup _startScreen;
    [SerializeField] private CanvasGroup _loseScreen;
    [SerializeField] private CanvasGroup _pauseScreen;

    // Start is called before the first frame update
    public void SetStartScreen(bool show)
    {
        _startScreen.alpha = show ? 1 : 0;
        _loseScreen.blocksRaycasts = show;

    }

    public void SetLoseScreen(bool show)
    {
        _loseScreen.alpha = show ? 1 : 0;
        _loseScreen.blocksRaycasts = show;
    }
    public void SetPauseScreen(bool show)
    {
        _pauseScreen.alpha = show ? 1 : 0;
        _pauseScreen.blocksRaycasts = show;
    }

    public void RestartGame()
    {
        GameManager.Instance.Restart();
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void UnPause()
    {
        GameManager.Instance.Pause(false);
    }
}
