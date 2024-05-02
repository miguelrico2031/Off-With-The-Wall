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
    [SerializeField] CanvasGroup _canvasGroup;

    private float _typeDelay;
    private Dialogue _testDialogue;
    private int _phraseIndex = -1;
    private bool _phraseFinished, _skip;

    private Action finishDialogueAction;

    private void Start()
    {
        _typeDelay = 1f / _typeSpeed;
        //SendDialog()
        //DisplayNextPhrase();
    }

    public void SendDialogue(string _key,Action _nextAction)
    {
        _testDialogue = _dialogues.GetDialogue(_key);
        if(_testDialogue != null)
        {
            finishDialogueAction = _nextAction;
            _canvasGroup.alpha = 1;
            _canvasGroup.blocksRaycasts = true;
            DisplayNextPhrase();
        }
        else
        {
            print("fallo");
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SkipOrContinue();
        }
    }
    private void DisplayNextPhrase()
    {
        if (++_phraseIndex >= _testDialogue.Phrases.Length)
        {
            //dialogue finished
            _phraseIndex = -1;
            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = false;
            finishDialogueAction.Invoke();
            return;
        }

        StartCoroutine(TypePhrase(_testDialogue.Phrases[_phraseIndex]));
    }

    public void SkipOrContinue()
    {
        if (!_phraseFinished) _skip = true;
        else DisplayNextPhrase();
    }


    private IEnumerator TypePhrase(Dialogue.Phrase phrase)
    {
        _phraseFinished = false;
        var sd = _dialogues.GetSpeakerData(phrase.Speaker);
        _dialogueText.text = $"{sd.Name}:\n";
        _speakerImg.sprite = sd.Sprite;

        foreach (var c in phrase.Text)
        {
            _dialogueText.text += c;
            if (!_skip) yield return new WaitForSeconds(_typeDelay);
        }

        _skip = false;
        _phraseFinished = true;
    }
}
