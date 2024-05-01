using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;


public class PeopleManager : IPeopleService
{
    #region Attributes
    
    public uint People { get; private set; } = GameManager.Instance.GameInfo.InitialPeople; //contador de la gente
    //Eventos cuando cambia el contador y cuando llega a 0 (game over(?))
    public UnityEvent<uint> OnPeopleChanged { get; private set; } = new();
    public UnityEvent OnZeroPeople { get; private set; } = new();

    private readonly Dictionary<string, float> _multipliers = new();

    #endregion

    public void AddPeople(uint people)
    {
        float total = _multipliers.Values.Aggregate(1f, (current, m) => current * m);
        People += people * (uint) Mathf.RoundToInt(total);
        OnPeopleChanged.Invoke(People);
    }

    public void RemovePeople(uint people)
    {
        if (People > people)
        {
            People -= people;
            OnPeopleChanged.Invoke(People);
            return;
        }

        People = 0;
        OnPeopleChanged.Invoke(People);
        OnZeroPeople.Invoke();
    }

    public void AddMultiplier(string key, float multiplier)
    {
        if (multiplier <= 0f)
        {
            Debug.LogError("People multipliers must be positive floats.");
            return;
        }
        if (!_multipliers.TryAdd(key, multiplier)) 
            Debug.LogError($"Multiplier with key = {key} already exists.");
    }

    public bool RemoveMultiplier(string key) => _multipliers.Remove(key);
    
}
