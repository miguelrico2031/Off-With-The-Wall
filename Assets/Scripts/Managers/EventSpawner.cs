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
        foreach (IGameEvent gEvent in GameManager.Instance.GameInfo.GetInitEvents())
        {
            AddEvent(gEvent);
        }


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
    
}
