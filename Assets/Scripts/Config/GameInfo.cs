
using UnityEngine;
using UnityEngine.Assertions;

// [CreateAssetMenu(menuName = "ScriptableObjects/GameInfo")]
public class GameInfo : ScriptableObject
{ //Objeto unico con informacion serializable sobre configuracion del juego, referenciada por el GameManager
    [field:SerializeField] public uint InitialPeople { get; private set; }

    [field:Header("Pedestrian AI")]
    [field:SerializeField] public uint InitialPedestrians { get; private set; }
    [field:SerializeField][field:Range(0f, 1f)] public float DespawnProbability { get; private set; }
    [field:SerializeField] public float MinAgentSpeed { get; private set; }
    [field:SerializeField] public float MaxAgentSpeed { get; private set; }
    
    [field:Header("Pop Ups")]
    [field:SerializeField] public int PopUpPoolSize { get; private set; }
    
    [field:Header("Events")]
    [field:SerializeField] public float RewardWaitTime { get; private set; }
    [field:SerializeField] public float EventWaitTime { get; private set; }
    [field:SerializeField] public uint RewardValue { get; private set; }

    [SerializeField] private ScriptableObject[] _initialEvents;

    [field: Header("Events custom stuff")]
    public string OrgName { get; set; }
    public string OrgSlogan { get; set; }
    public Sprite OrgBanner { get; set; }


    private IGameEvent[] _initEvents;

    public IGameEvent[] getInitEvents()
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
