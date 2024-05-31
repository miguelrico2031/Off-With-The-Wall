using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    #region Attributes

    public static GameManager Instance { get; private set; } //referencia al Singleton
    public enum GameState
    {
        OnEvent,
        OnPlay,
        OnPause,
        OnEnd
    }

    public event Action OnTearDownWall;

    [field: SerializeField] public GameState CurrentGameState { get; set; } = GameState.OnPlay;
    public GameInfo GameInfo { get => _gameInfo; } //archivo de config del juego (ScriptableObject)

    [SerializeField] private GameInfo _gameInfo;

    [SerializeField] private bool gameStarted, gameEnded;


    private Dictionary<string, IService> _services;

    private void LoseGame()
    {
        gameEnded = true;
        AudioManager.Instance.ChangeMusic("Lose");
        Get<IStartLoseUIService>().SetLoseScreen(true);
    }
    #endregion

    #region UnityCallbacks

    private void Awake() //se llama antes que cualquier awake
    {
        if (Instance is not null && Instance != this) Destroy(gameObject);
        Instance = this;
        GetComponent<ServicesBootstrapper>().Bootstrap();
        gameStarted = gameEnded = false;
        Get<IStartLoseUIService>().SetStartScreen(true);
        Get<IStartLoseUIService>().SetLoseScreen(false);
        CurrentGameState = GameState.OnPause;
        AudioManager.Instance.StartGameplayMusic("Level");
        AudioManager.Instance.PlayAmbience();
        AudioManager.Instance.startGame();

    }

    private IEnumerator StartGame()
    {
        yield return null; //para que todos se inicialicen en el frame del start
        gameStarted = true;
        Get<IEventService>().StartEvent(GameInfo.GameStartEvent, null);
        //Get<IEventSpawnService>().StartSpawn();
        Get<IPeopleService>().OnZeroPeople.AddListener(LoseGame);
        Get<IStartLoseUIService>().SetStartScreen(false);
        Get<IHUDService>().ShowHUD();
        // CurrentGameState = GameState.OnPlay;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && (!gameStarted || gameEnded))
        {
            if (!gameStarted)
                StartCoroutine(StartGame());

            if (gameEnded)
            {
                Restart();
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (CurrentGameState == GameState.OnPlay)
            {
                Pause(true);

            }
            else if (CurrentGameState == GameState.OnPause)
            {
                Pause(false);

            }
        }
    }

    #endregion
    public T Get<T>() where T : IService //Para acceder a los servicios (managers)
    {
        _services ??= new();
        return (T)_services.GetValueOrDefault(typeof(T).Name);
    }

    public void Register<T>(IService service) where T : IService
    {
        _services ??= new();
        var k = typeof(T).Name;
        if (_services.TryAdd(k, service)) return;
        Debug.LogError($"Service {nameof(service)} not registered " +
                       $"due to service {_services[k]} already registered as {k}.");
    }

    public void Restart()
    {
        Instance = null;
        //AudioManager.Instance.startGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
    public void Pause(bool setPause)
    {
        if (CurrentGameState is GameState.OnEnd) return;
        CurrentGameState = setPause ? GameState.OnPause : GameState.OnPlay;
        Get<IStartLoseUIService>().SetPauseScreen(setPause);
    }



    public void TearDownWall()
    {
        Debug.Log("MURO");
        OnTearDownWall?.Invoke();
        CurrentGameState = GameState.OnEnd;
        Get<IPopUpService>().HideAllPopUps();
        Get<IHUDService>().HideHUD();
        GetComponent<EndCinematic>().StartCinematic();
    }
}
