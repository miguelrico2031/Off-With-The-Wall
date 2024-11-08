using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class TextInputUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField _input;
    [SerializeField] private TextMeshProUGUI _requestText;

    private CanvasGroup _group;
    private Action<string> _callback;

    [SerializeField] Button _button;
    private void Awake()
    {
        _group = GetComponent<CanvasGroup>();
        _group.alpha = 0;
        _group.blocksRaycasts = false;
    }

    public void Display(string request, int charLimit, Action<string> callback)
    {
        _group.alpha = 1;
        _group.blocksRaycasts = true;
        _requestText.text = request;
        _callback = callback;
        _input.characterLimit = charLimit;
        _input.text = "";
        _button.interactable = false;
    }

    public void EnterButton()
    {
        _group.alpha = 0;
        _group.blocksRaycasts = false;
        _requestText.text = "";
        _callback(_input.text);
        AudioManager.Instance.PlayClick1();

    }
    public void valueChanged()
    {
        _button.interactable = _input.text != "";
    }
}
