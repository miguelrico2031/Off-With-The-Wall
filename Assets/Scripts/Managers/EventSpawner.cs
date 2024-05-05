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

    private void Start()
    {
        _buildingService = GameManager.Instance.Get<IBuildingService>();
        _gameInfo = GameManager.Instance.GameInfo;

        _rewardCountdown = StartCoroutine(RewardCountdown());
        _eventCountdown = StartCoroutine(EventCountdown());
        _eventPool.Clear();
        foreach (IGameEvent gEvent in GameManager.Instance.GameInfo.getInitEvents())
        {
            addEvent(gEvent);
        }


    }

    private void Update()
    {
        // if(GameManager.Instance.CurrentGameState == GameManager.GameState.OnPlay)
        // {
        //     _currentRewardWaitTime -= Time.deltaTime;
        //     _currentEventWaitTime -= Time.deltaTime;
        //     if(_currentRewardWaitTime<= 0)
        //     {
        //         _buildingService.SetReward(RewardValue);
        //         _currentRewardWaitTime = RewardWaitTime;
        //     }
        //     if (_currentEventWaitTime <= 0)
        //     {
        //         Event _sendEvent = GetEvent();
        //         _buildingService.SetEvent(_sendEvent,_sendEvent._buildingtype);
        //         _currentEventWaitTime = EventWaitTime;
        //     }
        // }
    }

    private IEnumerator RewardCountdown()
    {
        while (true)
        {
            for (int i = 0; i < 10; i++)
            {
                yield return new WaitForSeconds(_gameInfo.RewardWaitTime / 10f);
                if(GameManager.Instance.CurrentGameState != GameManager.GameState.OnPlay)
                    yield return new WaitUntil(() => GameManager.Instance.CurrentGameState == GameManager.GameState.OnPlay);
            }
            _buildingService.SetReward(_gameInfo.RewardValue);
        }

    }

    private IEnumerator EventCountdown()
    {
        while (true)
        {
            for (int i = 0; i < 10; i++)
            {
                yield return new WaitForSeconds(_gameInfo.EventWaitTime / 10f);
                if(GameManager.Instance.CurrentGameState != GameManager.GameState.OnPlay)
                    yield return new WaitUntil(() => GameManager.Instance.CurrentGameState == GameManager.GameState.OnPlay);
            }
            IGameEvent sendEvent = GetEvent();
            if(sendEvent != null)
            _buildingService.SetEvent(sendEvent);
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
                _eventPool.Remove(e);
                newEvent = e;
                break;
            }
            if (_doneEvents.Contains(newEvent))
            {
                newEvent = null;
            }
            _doneEvents.Add(newEvent);
        } while (newEvent != null && tries < _doneEvents.Count + _eventPool.Count);
        return newEvent;
    }
    public bool addEvent(IGameEvent _event)
    {
        if (_doneEvents.Contains(_event))
        {
            return false;
        }
        return _eventPool.Add(_event);
    }
}
