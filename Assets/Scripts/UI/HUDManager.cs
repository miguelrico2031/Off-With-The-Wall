using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour, IHUDService
{
    #region Attributes

    [SerializeField] private GameObject _logoUI;
    [SerializeField] private TextMeshProUGUI _peopleCountText;
    [SerializeField] private float _peopleCountAnimationDuration;
    
    private int _currentPeople = 0;
        
    #endregion


    public void ShowHUD()
    {
        _logoUI.SetActive(true);
        _peopleCountText.enabled = true;
        _peopleCountText.text = "0";
    }
    private void Start()
    {
        var peopleService = GameManager.Instance.Get<IPeopleService>(); 
        peopleService.OnPeopleChanged.AddListener(OnPeopleChanged);
        _logoUI.SetActive(false);
        _peopleCountText.enabled = false;
    }

    private void OnPeopleChanged(uint people)
    {
        if ((int)people == _currentPeople) return;
        StartCoroutine(PeopleChangeAnimation((int)people));
    }

    private IEnumerator PeopleChangeAnimation(int newPeople)
    {
        var difference = Mathf.Abs(newPeople - _currentPeople);
        var increment = newPeople > _currentPeople ? 1 : -1;
        var delay = _peopleCountAnimationDuration / difference;
        while (difference > 0)
        {
            difference--;
            _currentPeople += increment;
            _peopleCountText.text = $"{_currentPeople}";
            yield return new WaitForSeconds(delay);
        }
    }
}
