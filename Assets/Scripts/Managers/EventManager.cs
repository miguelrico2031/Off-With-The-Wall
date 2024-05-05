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

    [SerializeField] private GameObject _wheel;
    private void Start()
    {
        print("hsdh");
        _dialogueService = GameManager.Instance.Get<IDialogueService>();
        _buttonsCanvas.GetComponentsInChildren<Button>()[0].onClick.AddListener(OnAButtonClick);
        _buttonsCanvas.GetComponentsInChildren<Button>()[1].onClick.AddListener(OnBButtonClick);
        _buttonsCanvas.GetComponentsInChildren<Button>()[2].onClick.AddListener(OnCButtonClick);


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

            case RouletteEvent rouletteEvent:
              //_currentKeyA = choiceEvent.EndDialogueAKey;
                _currentKeyB = rouletteEvent.RefuseDialogueKey;


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
    private void OnCButtonClick()
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

    public int spinWheel(int[] chance,out int val)
    {

        int maxValue = 0;
        for (int i = 0; i < chance.Length; i++)
        {
            maxValue += chance[i];
        }
        int rval = UnityEngine.Random.Range(0, 360);
        val = rval;
        for (int i = 0; i < chance.Length; i++)
        {
            if (rval < (360 * chance[i]/maxValue) )
            {
                return i;
            }
            else
            {
                rval -= chance[i];
            }
        }
        return chance.Length - 1;
    }

    public void visualSpin(int value)
    {
        print("value: " + value);
        _wheel.transform.rotation = Quaternion.Euler(0, 0, value);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            print(spinWheel(new int[] { 100, 150, 110 }, out int val));
            visualSpin(val);
        }
    }
}
