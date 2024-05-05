using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class GameManager : MonoBehaviour
{
    #region Attributes

    public static GameManager Instance { get; private set; } //referencia al Singleton
    public enum GameState
    {
        OnEvent,
        OnPlay,
        OnPause
    }

    public GameState CurrentGameState { get; set; } = GameState.OnPlay;
    public GameInfo GameInfo { get => _gameInfo; } //archivo de config del juego (ScriptableObject)
    
    [SerializeField] private GameInfo _gameInfo;
    
    private Dictionary<string, IService> _services;
    

    #endregion

    #region UnityCallbacks

    private void Awake() //se llama antes que cualquier awake
    {
        if(Instance is not null && Instance != this) Destroy(gameObject);
        Instance = this;
        GetComponent<ServicesBootstrapper>().Bootstrap();
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
    
}
