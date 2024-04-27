using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public class BuildingManager : IBuildingService
{
    //
    
    private readonly Dictionary<IBuilding.BuildingType, List<IBuilding>> _buildings;

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
        if(building.Type is not IBuilding.BuildingType.Any) _buildings[IBuilding.BuildingType.Any].Add(building);
    }

    public bool SetReward(uint reward, IBuilding.BuildingType target = IBuilding.BuildingType.Any)
    {
        var availables = GetAvailableBuildings(target);
        if (availables is null) return false;
        availables[Random.Range(0, availables.Count)].SetReward(reward); //elijo uno random
        return true;
    }

    public bool SetEvent(object buildingEvent, IBuilding.BuildingType target)
    {
        var availables = GetAvailableBuildings(target);
        if (availables is null) return false;
        availables[Random.Range(0, availables.Count)].SetEvent(buildingEvent); //elijo uno random
        return true;
    }

    private List<IBuilding> GetAvailableBuildings(IBuilding.BuildingType target)
    {
        //elijo la lista segun el tipo de edificio (any es todos los edificios)
        var availables = _buildings[target];
        //filtro la lista para quedarme con los edificios que esten en idle
        availables = availables.Where(b => b.CurrentState is IBuilding.State.Idle).ToList();
        return availables.Any() ? availables : null; //devuelvo la lista si no esta vacia, sino null
    }
}
