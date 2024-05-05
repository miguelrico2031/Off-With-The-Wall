using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class TextInputUIManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField _input;
    [SerializeField] private TextMeshProUGUI _requestText;
    
    private GameObject _uIGroup;
    private Action<string> _callback;

    private void Awake()
    {
        _uIGroup = transform.GetChild(0).gameObject;
        _uIGroup.SetActive(false);
    }

    public void Display(string request, Action<string> callback)
    {
        _uIGroup.SetActive(true);
        _requestText.text = request;
        _callback = callback;
    }

    public void EnterButton()
    {
        _uIGroup.SetActive(false);
        _requestText.text = "";
        _callback(_input.text);
    }
}
