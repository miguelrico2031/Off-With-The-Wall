using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultUIManager :MonoBehaviour, IMultUIService
{
    [SerializeField] GameObject _prefabPeople;
    [SerializeField] GameObject _prefabPopUP;

    [SerializeField] private GameObject _peopleMults;
    [SerializeField] private float _peopleDist;
    [SerializeField] private int _peopleCount;
    [SerializeField] private int _peoplePermCount;


    [SerializeField] private GameObject _popUpMults;
    [SerializeField] private float _popUpDist;
    [SerializeField] private int _popUpCount;
    [SerializeField] private int _popUpPermCount;

    public void AddPeopleMult(bool perm)
    {
        print(_peopleMults.transform.childCount + " : "+ _peopleCount);
        if (_peopleCount < _peopleMults.transform.childCount)
        {
            _peopleMults.transform.GetChild(_peopleCount).GetComponent<Image>().enabled = true;
            _peopleCount++;
        }
    }
    public void AddPopUpMult(bool perm)
    {
        if (_popUpCount < _popUpMults.transform.childCount)
        {
            _popUpMults.transform.GetChild(_popUpCount).GetComponent<Image>().enabled = true;
            _popUpCount++;
        }
    }
    public void RemovePeopleMult()
    {
        if (_peopleCount > 0)
        {
            _peopleCount--;
            _peopleMults.transform.GetChild(_peopleCount).GetComponent<Image>().enabled = false;
        }
    }
    public void RemovePopUpMult()
    {
        if (_popUpCount > 0)
        {
            _popUpCount--;
            _popUpMults.transform.GetChild(_popUpCount).GetComponent<Image>().enabled = false;
        }
    }
    private void Start()
    {
        SetUI();
    }
    private void SetUI()
    {
        _popUpCount = _peopleCount = 0;
        Vector3 peoplePos = _peopleMults.transform.localPosition;
        for (int i = 0; i < _peopleMults.transform.childCount; i++)
        {
            _peopleMults.transform.GetChild(i).localPosition = new Vector3(0 + i*_peopleDist,0,0);
            _peopleMults.transform.GetChild(i).GetComponent<Image>().enabled = false;
        }
        Vector3 popupPos = _popUpMults.transform.position;
        print(peoplePos);

        for (int i = 0; i < _popUpMults.transform.childCount; i++)
        {
            //_popUpMults.transform.GetChild(i).localPosition = new Vector3(popupPos.x + i * _popUpDist, popupPos.y, popupPos.z);
            _popUpMults.transform.GetChild(i).localPosition = new Vector3(0 +i * _popUpDist, 0, 0);
            _popUpMults.transform.GetChild(i).GetComponent<Image>().enabled = false;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(_peopleMults.transform.position, 2);
        Gizmos.DrawSphere(_popUpMults.transform.position, 2);

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U)){
            SetUI();
        }
    }
}
