using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class TestDialogueManager : MonoBehaviour
{
    [SerializeField] private Dialogues _dialogues;
    [SerializeField] private TextMeshProUGUI _dialogueText;
    [SerializeField] private Image _speakerImg;
    [SerializeField] private float _typeSpeed;

    private float _typeDelay;
    private Dialogue _testDialogue;
    private int _phraseIndex = -1;
    private bool _phraseFinished, _skip;

    private void Start()
    {
        _typeDelay = 1f / _typeSpeed;
        _testDialogue = _dialogues.GetDialogue("test");
        DisplayNextPhrase();
    }

    private void DisplayNextPhrase()
    {
        if (++_phraseIndex >= _testDialogue.Phrases.Length)
        {
            //dialogue finished
            _phraseIndex = -1;
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
