using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class DialogueManager : MonoBehaviour, IDialogueService
{
    [SerializeField] private Dialogues _dialogues;
    [SerializeField] private TextMeshProUGUI _dialogueText;
    [SerializeField] private Image _speakerImg;
    [SerializeField] private float _typeSpeed;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Button _continueButton;

    private float _typeDelay;
    private Dialogue _currentDialogue;
    private int _phraseIndex = -1;
    private bool _phraseFinished, _skip, _hideOnFinish = true, _isInfo;
    private TextMeshProUGUI _continueButtonText;

    private Action _finishDialogueAction;

    private void Awake()
    {
        _typeDelay = 1f / _typeSpeed;
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;
        _continueButtonText = _continueButton.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SendDialogue(string key, bool hideOnFinish, Action nextAction)
    {
        _isInfo = false;
        _hideOnFinish = hideOnFinish;
        _continueButton.gameObject.SetActive(true);
        _currentDialogue = _dialogues.GetDialogue(key);

        if (_currentDialogue is null)
        {
            Debug.LogError($"Dialogue key {key} not found.");
            return;
        }
    
        _finishDialogueAction = nextAction;
        _canvasGroup.alpha = 1;
        _canvasGroup.blocksRaycasts = true;
        DisplayNextPhrase();
    }

    public void SendInfoText(string text, Action nextAction)
    {
        _isInfo = true;
        _hideOnFinish = true;
        _continueButton.gameObject.SetActive(true);
        _finishDialogueAction = nextAction;
        _canvasGroup.alpha = 1;
        _canvasGroup.blocksRaycasts = true;
        _speakerImg.enabled = false;
        var p = new Dialogue.Phrase { Speaker = Dialogues.Speaker.Info, Text = text };
        StartCoroutine(TypePhrase(p));
    }

    public void Hide()
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;
    }

    private void DisplayNextPhrase()
    {
        if (_isInfo || ++_phraseIndex >= _currentDialogue.Phrases.Length)
        {
            //dialogue finished
            _phraseIndex = -1;
            _continueButton.gameObject.SetActive(false);
            _speakerImg.enabled = true;
            _isInfo = false;
            if (!_hideOnFinish)
            {
                _finishDialogueAction();
                return;
            }
            
            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = false;
            _finishDialogueAction();
            return;
        }

        StartCoroutine(TypePhrase(_currentDialogue.Phrases[_phraseIndex]));
    }

    public void SkipOrContinue()
    {
        if (_phraseIndex == -1 && !_isInfo) return;
        if (!_phraseFinished) _skip = true;
        else DisplayNextPhrase();
    }


    private IEnumerator TypePhrase(Dialogue.Phrase phrase)
    {
        _phraseFinished = false;
        _continueButtonText.text = "Skip";  
        var sd = _dialogues.GetSpeakerData(phrase.Speaker);
        // _dialogueText.text = $"{sd.Name}:\n";
        _dialogueText.text = "";
        _speakerImg.sprite = sd.Sprite;

        foreach (var c in phrase.Text)
        {
            _dialogueText.text += c;
            if (!_skip) yield return new WaitForSeconds(_typeDelay);
        }

        _skip = false;
        _phraseFinished = true;
        _continueButtonText.text = "Continue";

    }
}
