using System;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour, IHUDService
{
    #region Attributes

    [SerializeField] private TextMeshProUGUI _peopleCountText;
    

    #endregion
    
    private void Start()
    {
        var peopleService = GameManager.Instance.Get<IPeopleService>(); 
        peopleService.OnPeopleChanged.AddListener(OnPeopleChanged);
        _peopleCountText.text = $"{peopleService.People}";
    }

    private void OnPeopleChanged(uint people)
    {
        _peopleCountText.text = $"{people}";
    }
}
