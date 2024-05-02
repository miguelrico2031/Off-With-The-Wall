using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EventManager : MonoBehaviour, IEventService
{
    [SerializeField] private CanvasGroup _buttonsCanvas;
    private IDialogueService _dialogueService;

    private string _currentKeyA, _currentKeyB;
    private Action _currentActionA, _currentActionB;

    private void Start()
    {
        _dialogueService = GameManager.Instance.Get<IDialogueService>();
        _buttonsCanvas.GetComponentsInChildren<Button>()[0].onClick.AddListener(OnAButtonClick);
        _buttonsCanvas.GetComponentsInChildren<Button>()[1].onClick.AddListener(OnBButtonClick);
    }

    private void OnDestroy()
    {
        foreach(var b in _buttonsCanvas.GetComponentsInChildren<Button>()) b.onClick.RemoveAllListeners();
    }

    public void StartEvent(IGameEvent e)
    {
        switch (e)
        {
            case ChoiceEvent choiceEvent:
                // _buttonsCanvas.GetComponentsInChildren<Button>()[0].onClick.AddListener(delegate 
                //     { _dialogueService.SendDialogue(choiceEvent.EndDialogueAKey, () => ResolveOutcomes(choiceEvent.OutcomesA)); });
                // _buttonsCanvas.GetComponentsInChildren<Button>()[1].onClick.AddListener(delegate 
                //     { _dialogueService.SendDialogue(choiceEvent.EndDialogueBKey, () => ResolveOutcomes(choiceEvent.OutcomesB)); });
                //

                _currentKeyA = choiceEvent.EndDialogueAKey;
                _currentKeyB = choiceEvent.EndDialogueBKey;
                _currentActionA = () => ResolveOutcomes(choiceEvent.OutcomesA);
                _currentActionB = () => ResolveOutcomes(choiceEvent.OutcomesB);
                
                _dialogueService.SendDialogue(choiceEvent.StartDialogueKey,ShowButton);
                break;
            
            case PassiveEvent passiveEvent:
                _dialogueService.SendDialogue(passiveEvent.StartDialogueKey, () => ResolveOutcomes(passiveEvent.Outcomes)) ;
                break;
        }
    }

    private void OnAButtonClick()
    {
        _dialogueService.SendDialogue(_currentKeyA, _currentActionA);
    }

    private void OnBButtonClick()
    {
        _dialogueService.SendDialogue(_currentKeyB, _currentActionB);
    }
    
    private void ShowButton()
    {
        _buttonsCanvas.alpha = 1;
        _buttonsCanvas.blocksRaycasts = true;
    }
    
    private void ResolveOutcomes(Outcomes outcomes)
    {
        _buttonsCanvas.alpha = 0;
        _buttonsCanvas.blocksRaycasts = false;
        foreach(var outcome in outcomes.Get()) outcome.Execute();
    }
}
