using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EventSpawner : MonoBehaviour, IEventSpawnService
{
    private readonly List<IGameEvent> _availableEvents = new();
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
            _buildingService.SetEvent(sendEvent);
        }
    }
    
    private IGameEvent GetEvent()
    {
        //int i = 0;
        int randomIdx = -1;
        IGameEvent newEvent = null;
        do
        {
            randomIdx = Random.Range(0, _availableEvents.Count);
            newEvent = _availableEvents[randomIdx];
            //i++;
        } while (_doneEvents.Contains(newEvent)/* && i < 50*/);
        _availableEvents.RemoveAt(randomIdx);
        _doneEvents.Add(newEvent);

        // if(i == 50)
        // {
        //     _newEvent = _emergencyEvent;
        // }
        return newEvent;
    }
}
