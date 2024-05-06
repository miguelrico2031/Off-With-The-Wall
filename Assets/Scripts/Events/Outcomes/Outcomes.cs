using System;
using System.Linq;
using UnityEngine;


[Serializable]
public class Outcomes //clase serializable para agrupar outcomes
{
    [SerializeField] private PeopleIncrease[] _peopleIncreases;
    [SerializeField] private PeopleDecrease[] _peopleDecreases;
    [SerializeField] private RewardMultiplier[] _rewardMultipliers;
    [SerializeField] private PopUpMultiplier[] _popUpMultipliers;
    [SerializeField] private HouseBurn[] _houseBurns;


    private IOutcome[] _outcomes;
    
    public IOutcome[] Get()
    {
        if (_outcomes is not null) return _outcomes;
        var l = _peopleIncreases.Cast<IOutcome>().ToList();
        l.AddRange(_peopleDecreases.Cast<IOutcome>());
        l.AddRange(_rewardMultipliers.Cast<IOutcome>());
        l.AddRange(_popUpMultipliers.Cast<IOutcome>());
        l.AddRange(_houseBurns.Cast<IOutcome>());
        _outcomes = l.ToArray();
        return _outcomes;
    }
}

[Serializable]
public class PeopleIncrease : IOutcome
{
    [field:SerializeField] public string DisplayText { get; private set; }
    public uint People;
    public void Execute(IBuilding building = null)
    {
        GameManager.Instance.Get<IPeopleService>().AddPeople(People);
    }
}

[Serializable]
public class RewardMultiplier : IOutcome
{
    [field:SerializeField] public string DisplayText { get; private set; }
    [Range(.1f, 3f)] public float Multiplier;
    public string MultiplierName;
    public bool IsPermanent;
    public float Duration;
    public void Execute(IBuilding building = null)
    {
        var peopleService = GameManager.Instance.Get<IPeopleService>();
        if (IsPermanent) peopleService.AddMultiplier(MultiplierName, Multiplier);
        else peopleService.AddMultiplier(MultiplierName, Multiplier, Duration);
    }
}

[Serializable]
public class PopUpMultiplier : IOutcome
{
    [field:SerializeField] public string DisplayText { get; private set; }
    [Range(.1f, 3f)] public float Multiplier;
    public string MultiplierName;
    public bool IsPermanent;
    public float Duration;

    public void Execute(IBuilding building = null)
    {
        
    }
}

[Serializable]
public class PeopleDecrease : IOutcome
{
    [field:SerializeField] public string DisplayText { get; private set; }
    public uint People;
    public void Execute(IBuilding building = null)
    {
        GameManager.Instance.Get<IPeopleService>().RemovePeople(People);
    }
}

[Serializable]
public class EventAdd : IOutcome
{
    [field: SerializeField] public string DisplayText { get; private set; }
    public IGameEvent gameEvent;
    public uint People;
    public void Execute(IBuilding building = null)
    {
        GameManager.Instance.Get<IEventSpawnService>().AddEvent(gameEvent);
    }
}

[Serializable]
public class HouseBurn : IOutcome
{
    [field: SerializeField] public string DisplayText { get; private set; }
    public void Execute(IBuilding building = null)
    {
        building.GetBurned();
    }
}


