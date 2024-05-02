using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSpawner : MonoBehaviour, IEventSpawnService
{

  [SerializeField] private List<Event> AllEvents;
  private HashSet<Event> DoneEvents;


    private IBuildingService _buildingService;

    [field:SerializeField] public float RewardWaitTime { get; private set; }
    [SerializeField] float _currentRewardWaitTime;

    [field:SerializeField] public float EventWaitTime { get; private set; }
    [SerializeField] float _currentEventWaitTime;


   [field:SerializeField] public uint _rewardValue { get; private set; }

    [SerializeField] private Event _emergencyEvent; //En el caso imposible de que se acaben los eventos cuando no deben saldrá este evento. 
    [SerializeField] private int _eventSearchThreshold; //Cuantas veces busca el evento hasta que se cansa

    // Start is called before the first frame update
    void Start()
    {
        _buildingService = GameManager.Instance.Get<IBuildingService>();
        DoneEvents = new HashSet<Event>();
    }

    // Update is called once per frame
    void Update()
    {
        print("update");
        if(GameManager.Instance.CurrentGameState == GameManager.GameState.OnPlay)
        {
            print("play");
            _currentRewardWaitTime -= Time.deltaTime;
            _currentEventWaitTime -= Time.deltaTime;
            if(_currentRewardWaitTime<= 0)
            {
                _buildingService.SetReward(_rewardValue);
                _currentRewardWaitTime = RewardWaitTime;
            }
            if (_currentEventWaitTime <= 0)
            {
                Event _sendEvent = getEvent();
                _buildingService.SetEvent(_sendEvent,_sendEvent._buildingtype);
                _currentEventWaitTime = EventWaitTime;
            }
        }
    }
    Event getEvent()
    {
        int i = 0;
        Event _newEvent = null;
        print("bum");
        while((_newEvent == null || !DoneEvents.Contains(_newEvent)) && i < 50)
        {
            _newEvent = AllEvents[Random.Range(0, AllEvents.Count)];
            i++;
        }
        if(i == 50)
        {
            _newEvent = _emergencyEvent;
        }
        return _newEvent;
    }
}
