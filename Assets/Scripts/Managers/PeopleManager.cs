using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;


public class PeopleManager : IPeopleService
{
    #region Attributes
    
    public uint People { get; private set; } = 0; //contador de la gente
    //Eventos cuando cambia el contador y cuando llega a 0 (game over(?))
    public UnityEvent<uint> OnPeopleChanged { get; private set; } = new();
    public UnityEvent OnZeroPeople { get; private set; } = new();

    private readonly Dictionary<string, float> _multipliers = new();

    #endregion

    public uint AddPeople(uint people)
    {
        uint total = (uint) Mathf.RoundToInt(_multipliers.Values.Aggregate(1f, (current, m) => current + (m-1)));
        Debug.Log(total);
        People += people * total;
        OnPeopleChanged.Invoke(People);
        return people * total;
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

    public void AddMultiplier(string key, float multiplier, float duration)
    {
        AddMultiplier(key, multiplier);
        //esto lo hago porque solo se pueden llamar coroutines en monobehaviors
        GameManager.Instance.StartCoroutine(RemoveMultiplierAfterDuration(key, duration));
    }

    public bool RemoveMultiplier(string key)
    {
        if (_multipliers.Remove(key))
        {

            GameManager.Instance.Get<IMultUIService>().RemovePeopleMult();
            return true;
        }
        return false;
    }

    private IEnumerator RemoveMultiplierAfterDuration(string key, float duration)
    {
        while(duration > 0)
        {
            yield return new WaitForSeconds(.5f);
            duration -= .5f;
            if(GameManager.Instance.CurrentGameState != GameManager.GameState.OnPlay)
                yield return new WaitUntil(() => GameManager.Instance.CurrentGameState == GameManager.GameState.OnPlay);
        }
        GameManager.Instance.Get<IMultUIService>().RemovePeopleMult();
        RemoveMultiplier(key);

    }
    
}
