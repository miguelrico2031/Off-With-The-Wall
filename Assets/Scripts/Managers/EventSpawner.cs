using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EventSpawner : MonoBehaviour, IEventSpawnService
{
   [field: SerializeField] private readonly HashSet<IGameEvent> _eventPool = new();

    private readonly HashSet<IGameEvent> _doneEvents = new();

    private IBuildingService _buildingService;
    private GameInfo _gameInfo;

    private Coroutine _rewardCountdown, _eventCountdown;

    private readonly Dictionary<string, float> _multipliers = new();

    private float dividervalue;
    private void Start()
    {
        _buildingService = GameManager.Instance.Get<IBuildingService>();
        _gameInfo = GameManager.Instance.GameInfo;
        _eventPool.Clear();
        dividervalue = 1;
        foreach (IGameEvent gEvent in GameManager.Instance.GameInfo.GetInitEvents())
            AddEvent(gEvent);
    }

    public void StartSpawn()
    {
        print("helou");
        _rewardCountdown = StartCoroutine(RewardCountdown());
        _eventCountdown = StartCoroutine(EventCountdown());
    }
    

    private IEnumerator RewardCountdown()
    {
        while (true)
        {
            for (int i = 0; i < 10; i++)
            {
                var delay = Random.Range(_gameInfo.RewardMinWaitTime, _gameInfo.RewardMaxWaitTime);

                yield return new WaitForSeconds(delay / 10f);
                if(GameManager.Instance.CurrentGameState != GameManager.GameState.OnPlay)
                    yield return new WaitUntil(() => GameManager.Instance.CurrentGameState == GameManager.GameState.OnPlay);
            }
            if(_buildingService.HouseRewardLimitReached) continue;

            _buildingService.SetHouseReward(_gameInfo.RewardValue);
        }

    }

    private IEnumerator EventCountdown()
    {
        while (true)
        {
            for (int i = 0; i < 10; i++)
            {
                var delay = Random.Range(_gameInfo.EventMinWaitTime, _gameInfo.EventMaxWaitTime);
                delay *= dividervalue;
                yield return new WaitForSeconds(delay / 10f);
                if(GameManager.Instance.CurrentGameState != GameManager.GameState.OnPlay)
                    yield return new WaitUntil(() => GameManager.Instance.CurrentGameState == GameManager.GameState.OnPlay);
            }
            if(_buildingService.EventLimitReached) continue;
            IGameEvent sendEvent = GetEvent();
            
            if (sendEvent is null)
            {
                Debug.LogError("No hay eventos!!");
                continue;
            }
            print("spawnear");
            if (_buildingService.SetEvent(sendEvent)) RemoveEvent(sendEvent);
  
        }
    }
    
    private IGameEvent GetEvent()
    {

        IGameEvent newEvent = null;
        int tries = 0;
        do
        {
            tries++;
            int r = Random.Range(0, _eventPool.Count);
            int i = 0;
            foreach (var e in _eventPool)
            {
                if (i++ != r) continue;
                //_eventPool.Remove(e);
                newEvent = e;
                break;
            }
            if (_doneEvents.Contains(newEvent))
            {
                _eventPool.Remove(newEvent);
                newEvent = null;
            }
            //_doneEvents.Add(newEvent);
        } while (newEvent != null && tries < _doneEvents.Count + _eventPool.Count);
        return newEvent;
    }
    private void RemoveEvent(IGameEvent _event)
    {
        _eventPool.Remove(_event);
        _doneEvents.Add(_event);
    }
    public bool AddEvent(IGameEvent _event) => _eventPool.Add(_event);
    
    public void SpawnSetUpEvents()
    {
        _buildingService.SetEvent(GameManager.Instance.GameInfo.GameDrawEvent);
        _buildingService.SetEvent(GameManager.Instance.GameInfo.GameSloganEvent);
    }


    public void AddMultiplier(string key, float multiplier)
    {
        if (multiplier <= 0f)
        {
            Debug.LogError("People multipliers must be positive floats.");
            return;
        }
        if (!_multipliers.TryAdd(key, multiplier))
            Debug.LogError($"Multiplier with key = {key} already exists.");

        dividervalue /= multiplier;
    }

    public void AddMultiplier(string key, float multiplier, float duration)
    {
        AddMultiplier(key, multiplier);
        
        //esto lo hago porque solo se pueden llamar coroutines en monobehaviors
        GameManager.Instance.StartCoroutine(RemoveMultiplierAfterDuration(key, duration));
    }

    public bool RemoveMultiplier(string key)
    {
        if (_multipliers.ContainsKey(key))
        {
            AudioManager.Instance.PlaySound("loseMulti");
            dividervalue *= _multipliers[key];
            GameManager.Instance.Get<IMultUIService>().RemovePopUpMult(_multipliers[key]);
            _multipliers.Remove(key);

            return true;
        }
        else
        {
            return false;
        }

    }

    private IEnumerator RemoveMultiplierAfterDuration(string key, float duration)
    {
        while (duration > 0)
        {
            yield return new WaitForSeconds(.5f);
            duration -= .5f;
            if (GameManager.Instance.CurrentGameState != GameManager.GameState.OnPlay)
                yield return new WaitUntil(() => GameManager.Instance.CurrentGameState == GameManager.GameState.OnPlay);
        }

        RemoveMultiplier(key);

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            print("Posibles: "+ _eventPool.Count+"Hechos: " +_doneEvents.Count);
            
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            foreach (IGameEvent item in _doneEvents)
            {
                print(item.ToString());
            }
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            foreach (IGameEvent item in _eventPool)
            {
                print(item.ToString());
            }
        }
    }
}
