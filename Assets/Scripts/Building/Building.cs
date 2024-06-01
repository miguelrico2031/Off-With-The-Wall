using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Building : MonoBehaviour, IBuilding
{
    #region Attributes
    
    public IBuilding.State CurrentState { get; private set; }
    public IBuilding.BuildingType Type { get => _type; }
    public bool CanClick { get; set; } = true;
    public Transform PopUpPos { get; private set; }

    [SerializeField] private IBuilding.BuildingType _type;

    private uint _currentReward;
    private IPopUpService _popUpService;
    private IPeopleService _peopleService;
    private IBuildingService _buildingService;
    private IEventService _eventService;
    private IGameEvent _currentEvent;
    private Action _currentCallback;
    private BuildingClick _buildingClick;
    private SpriteRenderer _destroyedSprite;
    private GameObject _fires;
    
    #endregion

    #region Unity Callbacks

    private void Awake()
    {
        _popUpService = GameManager.Instance.Get<IPopUpService>();
        _peopleService = GameManager.Instance.Get<IPeopleService>();
        _buildingService = GameManager.Instance.Get<IBuildingService>();
        _eventService = GameManager.Instance.Get<IEventService>();
        _buildingService.AddBuilding(this);
        _buildingClick = GetComponentInChildren<BuildingClick>();
        PopUpPos = transform.Find("PopUpOffset");

        var dsTransform = transform.Find("Destroyed Sprite");
        if (dsTransform is null) return;
    
        _destroyedSprite = dsTransform.GetComponent<SpriteRenderer>();
        _destroyedSprite.enabled = false;
        _fires = transform.Find("Fires").gameObject;
        _fires.SetActive(false);
    


    }
    private void Start()
    {
        GameManager.Instance.OnTearDownWall += isEnd;
    }
    private void isEnd()
    {
        SetColor(0);

    }
    #endregion

    public void SetReward(uint reward, Action onCollected)
    { //actualiza su estado y reward
        CurrentState = IBuilding.State.HasReward;
        _currentReward = reward;
        _popUpService.ShowPopUp(this);
        _currentCallback = onCollected;
    }

    public void SetEvent(IGameEvent _event, Action onDispatched) //tipo object a cambiar luego
    {
        CurrentState = IBuilding.State.HasEvent; //falta implementar la logica de los eventos
        _currentEvent = _event;
        _popUpService.ShowPopUp(this, _event.GetType());
        _currentCallback = onDispatched;
    }



    public void OnPointerClick(PointerEventData eventData)
    {
        if (GameManager.Instance.CurrentGameState is not GameManager.GameState.OnPlay || CurrentState is IBuilding.State.Burned) return;
        if (!CanClick)
        {
            print("click2");
            AudioManager.Instance.PlayClick2();
            return;
        }
        if (_buildingClick.Click())
        {
            AudioManager.Instance.PlayClick1();
            SetColor(2);
            _buildingService.RegisterBuildingClick(this); //para q no se pueda clickar 2 veces el mismo edificio
            uint value = _peopleService.AddPeople(1);
            GameManager.Instance.Get<IScoreIncreaseUIService>().setIncrease(value, value != 1);
        }
        else
        {
           // AudioManager.Instance.PlayClick2();
        }
    }
    
    public void CollectReward()
    {
        //llamar al manager que corresponda para sumar la reward
        uint value = _peopleService.AddPeople(_currentReward);
        GameManager.Instance.Get<IScoreIncreaseUIService>().setIncrease(value, value != _currentReward);
        _currentReward = 0;
        CurrentState = IBuilding.State.Idle;   
        _popUpService.HidePopUp(this);
        _currentCallback();
        _currentCallback = null;
    }
    public void StartEvent()
    {
        _eventService.StartEvent(_currentEvent, this);
        _currentEvent = null;
        CurrentState = IBuilding.State.Idle; //no estoy seguro de ponerlo a idle aqui o cuando acabe el evento
        _popUpService.HidePopUp(this);
        _currentCallback();
        _currentCallback = null;
    }
    public void GetBurned()
    {
        if (CurrentState is IBuilding.State.HasReward or IBuilding.State.HasEvent)
        {
            _popUpService.HidePopUp(this);
            _currentCallback();
            _currentCallback = null;
            _currentReward = 0;
            _currentEvent = null;
        }
        CurrentState = IBuilding.State.Burned;
        SetColor(0);
        _buildingClick.gameObject.SetActive(false);
        _destroyedSprite.enabled = true;
        _fires.SetActive(true);
        foreach (var wp in GetComponentsInChildren<AIWaypoint>()) wp.Valid = false;
    }

    public void SetColor(int type)
    {
        foreach(SpriteRenderer spr in GetComponentsInChildren<SpriteRenderer>())
        {
            spr.color = GameManager.Instance.GameInfo.buildingColors[type];
        }

    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GameManager.Instance.CurrentGameState is GameManager.GameState.OnPlay && CanClick && CurrentState is not IBuilding.State.Burned)
        {
            SetColor(1);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (GameManager.Instance.CurrentGameState is GameManager.GameState.OnPlay && CanClick && CurrentState is not IBuilding.State.Burned)
        {
            SetColor(0);
        }
    }
}
