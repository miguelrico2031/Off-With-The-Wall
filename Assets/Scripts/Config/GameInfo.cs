
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

// [CreateAssetMenu(menuName = "ScriptableObjects/GameInfo")]
public class GameInfo : ScriptableObject
{ //Objeto unico con informacion serializable sobre configuracion del juego, referenciada por el GameManager
    [field:SerializeField] public uint InitialPeople { get; private set; }

    [field:Header("Pedestrian AI")]
    [field:SerializeField] public uint InitialPedestrians { get; private set; }
    [field:SerializeField][field:Range(0f, 1f)] public float PedDespawnProbability { get; private set; }
    [field:SerializeField] public float MinAgentSpeed { get; private set; }
    [field:SerializeField] public float MaxAgentSpeed { get; private set; }
    [field:SerializeField] public float PedTryIdleDelay { get; private set; }
    [field:SerializeField][field:Range(0f, 1f)] public float PedTryIdleProbability { get; private set; }
    [field:SerializeField] public float PedIdleMinTime { get; private set; }
    [field:SerializeField] public float PedIdleMaxTime { get; private set; }
    
    [field:Header("Pop Ups")]
    [field:SerializeField] public int PopUpPoolSize { get; private set; }
    
    [field:Header("Events")]
    [field:SerializeField] public float RewardMinWaitTime { get; private set; }
    [field:SerializeField] public float RewardMaxWaitTime { get; private set; }
    [field:SerializeField] public float EventMinWaitTime { get; private set; }
    [field:SerializeField] public float EventMaxWaitTime { get; private set; }
    [field:SerializeField] public uint RewardValue { get; private set; }

    [field: SerializeField] public uint MaxEventCount { get; private set; }
    [field: SerializeField] public uint MaxHouseRewardCount { get; private set; }

    [field: SerializeField] public ChooseNameEvent GameStartEvent { get; private set; }
    [field: SerializeField] public ChooseSloganEvent GameSloganEvent { get; private set; }
    [field: SerializeField] public DrawEvent GameDrawEvent { get; private set; }

    [field:SerializeField] public PassiveEvent EndGameEvent { get; private set; }
    [field:SerializeField] public Dialogue TutorialDialogue { get; private set; }

    [SerializeField] private ScriptableObject[] _initialEvents;

    [field: Header("Events custom stuff")]
    public string OrgName { get; set; }
    public string OrgSlogan { get; set; }
    public Sprite OrgBanner { get; set; }
    
    [field:Header("Roulette")]
    [field: SerializeField] public float RouletteSpinSpeed { get; private set; }
    [field: SerializeField] public float RouletteHideDelay { get; private set; }
    
    
    [field:Header("Wall")]
    [field: SerializeField] public uint WallFirstPeopleThreshold { get; private set; }
    [field: SerializeField] public uint WallSecondPeopleThreshold { get; private set; }


    [field: Header("Houses")]
    [field: SerializeField] public Color[] buildingColors { get; private set; }






    private IGameEvent[] _initEvents;
    private IGameEvent[] _setUpEvents;

    public IEnumerable<IGameEvent> GetInitEvents()
    {
        if(_initEvents == null)
        {
            _initEvents = new IGameEvent[_initialEvents.Length];
            for (int i = 0; i < _initEvents.Length; i++)
            {
                _initEvents[i] = _initialEvents[i] as IGameEvent;
                Assert.IsNotNull(_initEvents[i], "Espina");
            }
        }
        return _initEvents;
    }

    


}
