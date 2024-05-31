using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MultUIManager :MonoBehaviour, IMultUIService
{
    [SerializeField] GameObject _prefabPeople;
    [SerializeField] GameObject _prefabPopUP;

    [SerializeField] private GameObject _peopleMults;
    [SerializeField] private float _peopleDist;
    [SerializeField] private int _peopleCount;
    [SerializeField] private int _peoplePermCount;
    [SerializeField] private TMP_Text _peopleCountText;
    [SerializeField] private float _peopleMultCount;

    [SerializeField] private GameObject _popUpMults;
    [SerializeField] private float _popUpDist;
    [SerializeField] private int _popUpCount;
    [SerializeField] private int _popUpPermCount;
    [SerializeField] private TMP_Text _popUpCountText;
    [SerializeField] private float _popUpMultCount;

    public void AddPeopleMult(bool perm,float value)
    {
        print(_peopleMults.transform.childCount + " : "+ _peopleCount);
        _peopleMults.SetActive(true);
        _peopleMultCount *= value;
        _peopleCountText.text = "x" + _peopleMultCount;
        if ((_peopleCount+1) < _peopleMults.transform.childCount)
        {
            _peopleMults.transform.GetChild(_peopleCount+1).GetComponent<Image>().enabled = true;
            _peopleCount++;
            _peopleMults.transform.GetChild(0).localPosition = new Vector3(120 + _peopleCount * _peopleDist, 0, 0);

        }
    }
    public void AddPopUpMult(bool perm,float value)
    {
        _popUpMults.SetActive(true);
        _popUpMultCount *= value;
        _popUpCountText.text = "x" + _popUpMultCount;
        if ((_popUpCount+1) < _popUpMults.transform.childCount)
        {
            _popUpMults.transform.GetChild(_popUpCount+1).GetComponent<Image>().enabled = true;
            _popUpCount++;
            _popUpMults.transform.GetChild(0).localPosition = new Vector3(120 + _popUpCount * _popUpDist, 0, 0);

        }
    }
    public void RemovePeopleMult(float value)
    {
        _peopleMultCount /= value;
        _peopleCountText.text = "x" + _peopleMultCount;
        if (_peopleCount > 0)
        {
            _peopleCount--;
            _peopleMults.transform.GetChild(_peopleCount+1).GetComponent<Image>().enabled = false;
            _peopleMults.transform.GetChild(0).localPosition = new Vector3(120 + _peopleCount * _peopleDist, 0, 0);
        }
        if (_peopleCount == 0)
        {
            _peopleMults.SetActive(false);
        }
    }
    public void RemovePopUpMult(float value)
    {
        _popUpMultCount /= value;
        _popUpCountText.text = "x" + _popUpMultCount;
        if (_popUpCount > 0)
        {
            _popUpCount--;
            _popUpMults.transform.GetChild(_popUpCount+1).GetComponent<Image>().enabled = false;
        }
        if (_peopleCount == 0)
        {
            _popUpMults.SetActive(false);
        }
    }
    private void Start()
    {
        SetUI();
    }
    private void HideUI()
    {
        GetComponent<CanvasGroup>().alpha = 0;
    }
    private void SetUI()
    {
        GetComponent<CanvasGroup>().alpha = 1;

        _popUpCount = _peopleCount = 0;
        _popUpMultCount = _peopleMultCount = 1;
        Vector3 peoplePos = _peopleMults.transform.localPosition;
        for (int i = 1; i < _peopleMults.transform.childCount; i++)
        {
            _peopleMults.transform.GetChild(i).localPosition = new Vector3(0 + i*_peopleDist,0,0);
            _peopleMults.transform.GetChild(i).GetComponent<Image>().enabled = false;
        }
        Vector3 popupPos = _popUpMults.transform.position;
        print(peoplePos);

        for (int i = 1; i < _popUpMults.transform.childCount; i++)
        {
            //_popUpMults.transform.GetChild(i).localPosition = new Vector3(popupPos.x + i * _popUpDist, popupPos.y, popupPos.z);
            _popUpMults.transform.GetChild(i).localPosition = new Vector3(0 +i * _popUpDist, 0, 0);
            _popUpMults.transform.GetChild(i).GetComponent<Image>().enabled = false;
        }
        _popUpMults.SetActive(false);
        _peopleMults.SetActive(false);


    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(_peopleMults.transform.position, 2);
        Gizmos.DrawSphere(_popUpMults.transform.position, 2);

    }
    private void Update()
    {

    }
}
