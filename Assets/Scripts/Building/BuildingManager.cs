using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class BuildingManager : IBuildingService
{
    public bool EventLimitReached { get => _eventCount >= GameManager.Instance.GameInfo.MaxEventCount; }
    public bool HouseRewardLimitReached  { get => _houseRewardCount >= GameManager.Instance.GameInfo.MaxHouseRewardCount; }
    
    private readonly Dictionary<IBuilding.BuildingType, List<IBuilding>> _buildings;

    private uint _eventCount = 0, _houseRewardCount = 0;

    private Building _lastClicked = null;
    
    public BuildingManager()
    {
        //inicializar diccionario donde guardo los edificios segun su tipo
        _buildings = new();
        foreach (IBuilding.BuildingType t in Enum.GetValues(typeof(IBuilding.BuildingType)))
            _buildings[t] = new();
    }
    
    public void AddBuilding(IBuilding building)
    {
        _buildings[building.Type].Add(building);
        //evito duplicados si el tipo fuera any
        //creo que voy a quitar el tipo Any
        //if(building.Type is not IBuilding.BuildingType.Any) _buildings[IBuilding.BuildingType.Any].Add(building);
    }

    public bool SetHouseReward (uint reward)
    {
        var availables = GetAvailableBuildings(IBuilding.BuildingType.House);
        if (availables is null) return false;
        _houseRewardCount++;
        availables[Random.Range(0, availables.Count)].SetReward(reward, () => _houseRewardCount--); //elijo uno random
        return true;
    }

    public bool SetEvent(IGameEvent buildingEvent)
    {
        var availables = GetAvailableBuildings(buildingEvent.BuildingType);
        if (availables is null) return false;
        _eventCount++;
        availables[Random.Range(0, availables.Count)].SetEvent(buildingEvent, () => _eventCount--); //elijo uno random
        return true;
    }

    public void RegisterBuildingClick(Building building)
    {
        if (_lastClicked is not null) _lastClicked.CanClick = true;
        building.CanClick = false;
        _lastClicked = building;
    }
    
    private List<IBuilding> GetAvailableBuildings(IBuilding.BuildingType target)
    {
        //elijo la lista segun el tipo de edificio (any es todos los edificios)
        var availables = _buildings[target];
        //filtro la lista para quedarme con los edificios que esten en idle
        availables = availables.FindAll(b => b.CurrentState is IBuilding.State.Idle).ToList();
        return availables.Any() ? availables : null; //devuelvo la lista si no esta vacia, sino null
    }
    
}
