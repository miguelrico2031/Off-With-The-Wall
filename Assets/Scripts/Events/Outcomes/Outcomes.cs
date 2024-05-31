using System;
using System.Linq;
using UnityEngine;


[Serializable]
public class Outcomes //clase serializable para agrupar outcomes
{
    [field:SerializeField] public bool IsWallSuccess { get; private set; }
    [SerializeField] private PeopleIncrease[] _peopleIncreases;
    [SerializeField] private PeopleDecrease[] _peopleDecreases;
    [SerializeField] private RewardMultiplier[] _rewardMultipliers;
    [SerializeField] private PopUpMultiplier[] _popUpMultipliers;
    [SerializeField] private HouseBurn[] _houseBurns;
    [SerializeField] private CustomText[] _customText;
    [SerializeField] private EventAdd[] _eventAdds;


    private IOutcome[] _outcomes;
    
    public IOutcome[] Get()
    {
        if (_outcomes is not null) return _outcomes;
        var l = _peopleIncreases.Cast<IOutcome>().ToList();
        l.AddRange(_peopleDecreases);
        l.AddRange(_rewardMultipliers);
        //l.AddRange(_popUpMultipliers);
        l.AddRange(_houseBurns);
        l.AddRange(_eventAdds);
        _outcomes = l.ToArray();
        return _outcomes;
    }
}

[Serializable]
public class PeopleIncrease : IOutcome
{
    public uint People;
    public string DisplayText { get => $"{People} people gained."; }
    private uint _people;
    public void Execute(IBuilding building = null)
    {
        _people = GameManager.Instance.Get<IPeopleService>().AddPeople(People);
    }
}

[Serializable]
public class RewardMultiplier : IOutcome
{
    public string DisplayText
    {
        get => $"You receive a {(IsPermanent ? "permanent": "temporary")} {(Multiplier >= 1f ? "positive" : "negative")} multiplier. You get x{Multiplier} {(Multiplier >= 1f ? "more" : "less")} people{(IsPermanent ? "" : " for " + Duration + " seconds")}.";
    }
    [Range(.1f, 3f)] public float Multiplier;
    public string MultiplierName;
    public bool IsPermanent;
    public float Duration;
    public void Execute(IBuilding building = null)
    {
        var peopleService = GameManager.Instance.Get<IPeopleService>();
        if (IsPermanent) peopleService.AddMultiplier(MultiplierName, Multiplier);
        else peopleService.AddMultiplier(MultiplierName, Multiplier, Duration);

        GameManager.Instance.Get<IMultUIService>().AddPeopleMult(IsPermanent,Multiplier);
    }

}

[Serializable]
public class PopUpMultiplier : IOutcome
{
    public string DisplayText
    {
        get => $"You receive a {(IsPermanent ? "permanent" : "temporary")} {(Multiplier >= 1f ? "positive" : "negative")} multiplier. PopUps appear x{Multiplier} times {(Multiplier >= 1f ? "faster" : "slower")}{(IsPermanent ? "" : " for " + Duration + " seconds")}.";
    }
    [Range(.1f, 3f)] public float Multiplier;
    public string MultiplierName;
    public bool IsPermanent;
    public float Duration;

    public void Execute(IBuilding building = null)
    {
        var popUPService = GameManager.Instance.Get<IEventSpawnService>();
        if (IsPermanent) popUPService.AddMultiplier(MultiplierName, Multiplier);
        else popUPService.AddMultiplier(MultiplierName, Multiplier, Duration);

        GameManager.Instance.Get<IMultUIService>().AddPopUpMult(IsPermanent,Multiplier);
    }
}

[Serializable]
public class PeopleDecrease : IOutcome
{
    public string DisplayText { get => $"{People} people lost."; }
    public uint People;
    public void Execute(IBuilding building = null)
    {
        GameManager.Instance.Get<IPeopleService>().RemovePeople(People);
    }
}
[Serializable]
public class CustomText : IOutcome
{
    public string DisplayText { get => $"{text}"; }
    public string text;
    public void Execute(IBuilding building = null)
    {

    }
}
[Serializable]
public class EventAdd : IOutcome
{
    public string DisplayText { get => ""; }
    [SerializeField] private ScriptableObject _gameEvent;
    public void Execute(IBuilding building = null)
    {
        if(_gameEvent is not IGameEvent) Debug.LogError("EVENTO INVALIDO EN OUTCOME DE ADDEVENT");
        GameManager.Instance.Get<IEventSpawnService>().AddEvent(_gameEvent as IGameEvent);
    }
}

[Serializable]
public class HouseBurn : IOutcome
{
    [field: SerializeField] public string DisplayText { get; private set; }
    public void Execute(IBuilding building)
    {
        (building as Building).GetBurned();
    }
}


