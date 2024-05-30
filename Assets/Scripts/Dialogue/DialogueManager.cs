using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class DialogueManager : MonoBehaviour, IDialogueService
{
    [SerializeField] private DialogueInfo _dialogueInfo;
    [SerializeField] private TextMeshProUGUI _dialogueText, _newspaperText;
    [SerializeField] private Image _speakerframe,_speakerImg, _newspaperCover;
    [SerializeField] private float _typeSpeed;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Button _continueButton;
    [FormerlySerializedAs("_newsPaper")] [SerializeField] private GameObject _newspaper;
    [SerializeField] private GameObject _dialoguePanel;
    [SerializeField] private float _newspaperEntryDuration;

    [SerializeField] private Sprite _defaultSourceImage, _infoSourceImage;

    private float _typeDelay;
    private Dialogue _currentDialogue;
    private int _phraseIndex = -1;
   // private int currentPhraseCount;
    private bool _phraseFinished, _skip, _hideOnFinish = true, _isInfo,onAnimation;
    private TextMeshProUGUI _continueButtonText;
    private Sprite _currentCover = null;

    private Action _finishDialogueAction;

    [SerializeField] private Animator _textAnimator;
    [SerializeField] private Animator _portraitAnimator;


    private void Awake()
    {
        _typeDelay = 1f / _typeSpeed;
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;
        _continueButtonText = _continueButton.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SendDialogue(Dialogue dialogue, bool hideOnFinish, Action nextAction)
    {
        _isInfo = false;
        _currentDialogue = dialogue;
        _currentCover = _dialogueInfo.GetNewspaperCover(_currentDialogue);
        _hideOnFinish = hideOnFinish;
        _continueButton.gameObject.SetActive(true);
        _finishDialogueAction = nextAction;
        //_canvasGroup.alpha = 1;
        //_canvasGroup.blocksRaycasts = true;
        print("hola");
        

        DisplayNextPhrase();
    }

    public void SendInfoText(string text, Action nextAction)
    {
        _isInfo = true;
        _hideOnFinish = true;
        _continueButton.gameObject.SetActive(true);
        _finishDialogueAction = nextAction;
        //_canvasGroup.alpha = 1;
        //_canvasGroup.blocksRaycasts = true;
        _speakerImg.enabled = false;
        var p = new Dialogue.Phrase { Speaker = DialogueInfo.Speaker.Info, Text = text };

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
        AudioManager.Instance.PlayClick1();
        if (onAnimation)
        {
            print("do");

            onAnimation = false;
            _textAnimator.Play("New State", 0);
            _portraitAnimator.Play("New State", 0);
            StartCoroutine(writeLetters());

        }
        if (!_phraseFinished) _skip = true;
        else DisplayNextPhrase();
    }


    private IEnumerator TypePhrase(Dialogue.Phrase phrase)
    {
        bool isFirst = (_canvasGroup.alpha == 0 || !_dialogueText.IsActive());
        _canvasGroup.alpha = 1;
        _canvasGroup.blocksRaycasts = true;
        phrase = _dialogueInfo.ProcessPhrase(phrase);
        if (phrase.Speaker is DialogueInfo.Speaker.Newspaper)
        {
            StartCoroutine(DisplayNewspaper(phrase));
            yield break;
        }

        bool isInfo = phrase.Speaker is DialogueInfo.Speaker.Info;
        _speakerImg.enabled = _speakerframe.enabled = !isInfo;
        _dialoguePanel.GetComponent<Image>().sprite = !isInfo ? _defaultSourceImage : _infoSourceImage;
        _newspaper.SetActive(false);
        _dialoguePanel.SetActive(true);
        var sd = _dialogueInfo.GetSpeakerData(phrase.Speaker);
        _phraseFinished = false;
        _continueButtonText.text = "Skip";
        // _dialogueText.text = $"{sd.Name}:\n";
        _dialogueText.text = "";
        _speakerImg.sprite = sd.Sprite;
        _dialogueText.text = phrase.Text;
        _dialogueText.maxVisibleCharacters = 0;
       // currentPhraseCount = phrase.Text.Length;
        if (!isFirst)
        {
            StartCoroutine(writeLetters());
        }
        else
        {
            print("playanim");
            onAnimation = true;
            float time = _speakerImg.enabled ? 0 : 0.5f;
                _textAnimator.Play("UIdialog", 0,time);
                _portraitAnimator.Play("UIface",0, time);
            
        }


    }
    IEnumerator writeLetters()
    {

        for (int i = 0; i < _dialogueText.textInfo.characterCount; i++)
        {
            if (i % 5 == 0) AudioManager.Instance.PlayTalkSound();

            _dialogueText.maxVisibleCharacters++;
            if (!_skip)
            {
                yield return new WaitForSeconds(_typeDelay);
            }
            else
            {
                _dialogueText.maxVisibleCharacters = _dialogueText.textInfo.characterCount;
                break;
            }
        }

        _skip = false;
        _phraseFinished = true;
        _continueButtonText.text = "Continue";
    }
    private IEnumerator DisplayNewspaper(Dialogue.Phrase phrase)
    {
        AudioManager.Instance.PlaySound("periodico");
        _phraseFinished = true;
        _newspaper.SetActive(true);
        _newspaperCover.sprite = _currentCover;
        _newspaperCover.enabled = _currentCover is not null;
        _dialoguePanel.SetActive(false);
        _speakerImg.enabled = false;
        _speakerframe.enabled = false;
        _newspaperText.text = phrase.Text;
        _continueButton.gameObject.SetActive(false);
        
        var anim = _newspaper.GetComponent<Animator>(); 
        anim.Play("Entry");
        
        yield return new WaitForSeconds(_newspaperEntryDuration);
        
        _continueButton.gameObject.SetActive(true);
        _continueButtonText.text = "Continue";
        _skip = false;

    }
    public void TypePhraseAnim()
    {
        onAnimation = false;
        StartCoroutine(writeLetters());
    }

}
