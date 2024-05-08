using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
public class EventManager : MonoBehaviour, IEventService
{
    [SerializeField] private CanvasGroup _choiceButtonsCanvas;
    [SerializeField] private Button _buttonA, _buttonB;
    [SerializeField] private CanvasGroup _wheelCanvas;
    [SerializeField] private DrawUI _drawUI;
    [SerializeField] private TextInputUI _textInputUI;
    [SerializeField] private RouletteUI _rouletteUI;
    
    private IDialogueService _dialogueService;

    private Dictionary<Button, Action>_buttonActions;
    
    private int _currentResult;
    private int _currentSpin;

    [SerializeField] private GameObject _wheel;
    private void Start()
    {
        _dialogueService = GameManager.Instance.Get<IDialogueService>();

        _buttonActions = new()
        {
            { _buttonA, null },
            { _buttonB, null }
        };
        
        HideChoiceButtons();
    }

    public void StartEvent(IGameEvent e, IBuilding building)
    {
        GameManager.Instance.CurrentGameState = GameManager.GameState.OnEvent;
        switch (e)
        {
            case ChoiceEvent choiceEvent:
                
                _dialogueService.SendDialogue(choiceEvent.StartDialogueKey, false, ShowChoiceButtons);
                SetChoiceToButton(_buttonA, choiceEvent.ChoiceTextA, choiceEvent.EndDialogueKeyA, 
                    () => ResolveOutcomes(choiceEvent.OutcomesA, building));
                SetChoiceToButton(_buttonB, choiceEvent.ChoiceTextB, choiceEvent.EndDialogueKeyB, 
                    () => ResolveOutcomes(choiceEvent.OutcomesB, building));
                
                break;
            
            case PassiveEvent passiveEvent:
                
                _dialogueService.SendDialogue(passiveEvent.StartDialogueKey, true, () => ResolveOutcomes(passiveEvent.Outcomes, building));
                
                break;

            case RouletteEvent rouletteEvent:
                
                _dialogueService.SendDialogue(rouletteEvent.StartDialogueKey, false, ShowChoiceButtons);
                SetRouletteToButton(_buttonA, rouletteEvent, building);
                SetChoiceToButton(_buttonB, rouletteEvent.ChoiceTextRefuse, rouletteEvent.EndDialogueKeyRefuse, 
                    () => ResolveOutcomes(rouletteEvent.OutcomesRefuse, building));
              
                break;
            
            case DrawEvent drawEvent: //joder como me pone la programacion asincrona joder que bonito y poco mantenible
                
                _dialogueService.SendDialogue(drawEvent.StartDialogueKey, true, () =>
                {
                    _drawUI.Display(() => GameManager.Instance.CurrentGameState = GameManager.GameState.OnPlay);
                });
                
                break;
            
            case ChooseNameEvent chooseNameEvent:
                
                _dialogueService.SendDialogue(chooseNameEvent.StartDialogueKey, true, () =>
                {
                    _textInputUI.Display(chooseNameEvent.RequestPhrase, (chosenName) =>
                    {
                        GameManager.Instance.GameInfo.OrgName = chosenName;
                        GameManager.Instance.CurrentGameState = GameManager.GameState.OnPlay;
                    });
                });
                
                break;
            
            case ChooseSloganEvent chooseSloganEvent:
                
                _dialogueService.SendDialogue(chooseSloganEvent.StartDialogueKey, true, () =>
                {
                    _textInputUI.Display(chooseSloganEvent.RequestPhrase, (chosenSlogan) =>
                    {
                        GameManager.Instance.GameInfo.OrgSlogan = chosenSlogan;
                        GameManager.Instance.CurrentGameState = GameManager.GameState.OnPlay;
                    });
                });
                
                break;
        }
    }

    private void SetChoiceToButton(Button button, string choice, string dialogueKey, Action onDialogueFinish)
    {
        button.GetComponentInChildren<TextMeshProUGUI>().text = choice;
        _buttonActions[button] = () =>
        {
            _dialogueService.SendDialogue(dialogueKey, true, onDialogueFinish);
        };
    }

    private void SetRouletteToButton(Button button, RouletteEvent rEvent, IBuilding building)
    {
        button.GetComponentInChildren<TextMeshProUGUI>().text = rEvent.ChoiceTextAccept;
        _buttonActions[button] = () =>
        {
            _dialogueService.Hide();
            _rouletteUI.Display(rEvent, (result) => OnRouletteResolved(rEvent, result, building));
        };
    }

    private void OnRouletteResolved(RouletteEvent rouletteEvent, RouletteUI.Result result, IBuilding building)
    {
        (string key, Outcomes outcomes) = result switch
        {
            RouletteUI.Result.Crit => (rouletteEvent.EndDialogueKeyCrit, rouletteEvent.OutcomesCrit),
            RouletteUI.Result.Success => (rouletteEvent.EndDialogueKeyWin, rouletteEvent.OutcomesWin),
            RouletteUI.Result.Fail => (rouletteEvent.EndDialogueKeyLose, rouletteEvent.OutcomesLose),
            _ => (null, null)
        };
        _dialogueService.SendDialogue(key, true, () => ResolveOutcomes(outcomes, building));
    }

    public void AButtonClick()
    {
        HideChoiceButtons();
        _buttonActions[_buttonA]();
        
    }

    public void BButtonClick()
    {
        HideChoiceButtons();
        _buttonActions[_buttonB]();
    }
    
    private void ShowChoiceButtons()
    {
        _choiceButtonsCanvas.alpha = 1;
        _choiceButtonsCanvas.blocksRaycasts = true;
    }
    
    private void HideChoiceButtons()
    {
        _choiceButtonsCanvas.alpha = 0;
        _choiceButtonsCanvas.blocksRaycasts = false;
    }
    
    private void ResolveOutcomes(Outcomes outcomes, IBuilding building)
    {
        HideChoiceButtons();
        foreach(var outcome in outcomes.Get()) outcome.Execute(building);
        GameManager.Instance.CurrentGameState = GameManager.GameState.OnPlay;
    }

    
    
    
    
    
    public int SpinWheel(uint[] chance,out int val)
    {
      
        int maxValue = 0;
        for (int i = 0; i < chance.Length; i++)
        {
            maxValue += (int)chance[i];
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
                rval -= (int)chance[i];
            }
        }
        return chance.Length - 1;
    }

    public void VisualSpin(int value)
    {
        _wheelCanvas.alpha = 1;
        _wheelCanvas.blocksRaycasts = true;
        print("value: " + value);
        _wheel.transform.rotation = Quaternion.Euler(0, 0, value);
        ShowChoiceButtons();
    }

}
