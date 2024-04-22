using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class House : MonoBehaviour, IBuilding
{
    #region Attributes
    
    public IBuilding.State CurrentState { get; private set; }
    public IBuilding.BuildingType Type { get => _type; }

    [SerializeField] private IBuilding.BuildingType _type;

    private uint _currentReward;
    private IPeopleService _peopleService;
    private IBuildingService _buildingService;
    
    #endregion

    #region Unity Callbacks

    private void Start()
    {
        _peopleService = GameManager.Instance.Get<IPeopleService>();
        _buildingService = GameManager.Instance.Get<IBuildingService>();
        _buildingService.AddBuilding(this);
    }

    #endregion
    
    public void SetReward(uint reward)
    { //actualiza su estado y reward
        CurrentState = IBuilding.State.HasReward;
        _currentReward = reward;
    }

    public void SetEvent(object buildingEvent) //tipo object a cambiar luego
    {
        CurrentState = IBuilding.State.HasEvent; //falta implementar la logica de los eventos
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
        switch (CurrentState)
        {
            case IBuilding.State.Idle:
                //animacion o no hacer nada
                break;
            case IBuilding.State.HasReward:
                //llamar al manager que corresponda para sumar la reward
                _peopleService.AddPeople(_currentReward);
                _currentReward = 0;
                CurrentState = IBuilding.State.Idle;
                break;
            case IBuilding.State.HasEvent:
                //llamar al manager que sea para triggerear el evento
                CurrentState = IBuilding.State.Idle; //no estoy seguro de ponerlo a idle aqui o cuando acabe el evento
                break;
        }

    }
}
