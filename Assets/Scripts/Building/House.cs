using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class House : MonoBehaviour, IBuilding
{
    #region Attributes
    
    public IBuilding.State CurrentState { get; private set; }
    public IBuilding.BuildingType Type { get => _type; }
    [field:SerializeField] public Vector3 PopUpOffset { get; private set; }

    [SerializeField] private IBuilding.BuildingType _type;

    private uint _currentReward;
    private IPopUpService _popUpService;
    private IPeopleService _peopleService;
    private IBuildingService _buildingService;
    private IEventService _eventService;

    private IGameEvent _currentEvent;
    #endregion

    #region Unity Callbacks

    private void Awake()
    {
        _popUpService = GameManager.Instance.Get<IPopUpService>();
        _peopleService = GameManager.Instance.Get<IPeopleService>();
        _buildingService = GameManager.Instance.Get<IBuildingService>();
        _eventService = GameManager.Instance.Get<IEventService>();
        _buildingService.AddBuilding(this);
    }

    #endregion
    
    public void SetReward(uint reward)
    { //actualiza su estado y reward
        Debug.Log("PopUP");
        CurrentState = IBuilding.State.HasReward;
        _currentReward = reward;
        _popUpService.ShowPopUp(this);
        
    }

    public void SetEvent(IGameEvent _event) //tipo object a cambiar luego
    {
        CurrentState = IBuilding.State.HasEvent; //falta implementar la logica de los eventos
        _currentEvent = _event;
        _popUpService.ShowPopUp(this);

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //animacion guapa
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //deshacer animacion guapa (??)
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("CLICK");
        switch (CurrentState)
        {
            case IBuilding.State.Idle:
                //animacion o no hacer nada
                break;
            case IBuilding.State.HasReward:
                CollectReward();
                break;
            case IBuilding.State.HasEvent:
                //llamar al manager que sea para triggerear el evento
                _eventService.StartEvent(_currentEvent);
                CurrentState = IBuilding.State.Idle; //no estoy seguro de ponerlo a idle aqui o cuando acabe el evento
                _popUpService.HidePopUp(this);
                print("evento");
                break;
        }

    }

    public void CollectReward()
    {
        //llamar al manager que corresponda para sumar la reward
        _peopleService.AddPeople(_currentReward);
        _currentReward = 0;
        CurrentState = IBuilding.State.Idle;   
        _popUpService.HidePopUp(this);

    }
}
