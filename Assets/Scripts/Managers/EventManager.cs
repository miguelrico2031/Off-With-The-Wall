using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EventManager : MonoBehaviour, IEventService
{
    [SerializeField] private CanvasGroup _buttonsCanvas,_wheelCanvas;
    [SerializeField] private Button[] _buttons;
    [SerializeField] private DrawManager _drawUI;
    [SerializeField] private TextInputUIManager _textInputUI;
    
    
    private IDialogueService _dialogueService;

    private string _currentKeyA, _currentKeyB, _currentKeyC;
    private Action _currentActionA, _currentActionB, _currentActionC;

    //private uint[] _currentChances; //Para la ruleta
    //private Outcomes[] _currentOutcomes; 
    //private string[] _currentKeys;
    private int _currentResult;
    private int _currentSpin;

    [SerializeField] private GameObject _wheel;
    private void Start()
    {
        _dialogueService = GameManager.Instance.Get<IDialogueService>();
        _buttonsCanvas.GetComponentsInChildren<Button>()[0].onClick.AddListener(OnAButtonClick);
        _buttonsCanvas.GetComponentsInChildren<Button>()[1].onClick.AddListener(OnBButtonClick);
        _buttonsCanvas.GetComponentsInChildren<Button>()[2].onClick.AddListener(OnCButtonClick);
        _buttonsCanvas.GetComponentsInChildren<Button>()[3].onClick.AddListener(OnDButtonClick);

    }

    private void OnDestroy()
    {
        foreach(var b in _buttonsCanvas.GetComponentsInChildren<Button>()) b.onClick.RemoveAllListeners();
    }

    public void StartEvent(IGameEvent e)
    {
        GameManager.Instance.CurrentGameState = GameManager.GameState.OnEvent;
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
                setUpButtons(new bool[] { true, true, false, false });
                break;
            
            case PassiveEvent passiveEvent:
                _dialogueService.SendDialogue(passiveEvent.StartDialogueKey, () => ResolveOutcomes(passiveEvent.Outcomes)) ;
                break;

            case RouletteEvent rouletteEvent:
              //_currentKeyA = choiceEvent.EndDialogueAKey;
                _currentKeyB = rouletteEvent.RefuseDialogueKey;
                _currentActionB = () => ResolveOutcomes(rouletteEvent.OutcomesRefuse);
                _currentResult = SpinWheel(new uint[] { rouletteEvent.WinChance, rouletteEvent.LoseChance, rouletteEvent.CritChance }, out int val);
                _currentSpin = val;
                Outcomes[] newoutcomes = new  Outcomes[] { rouletteEvent.OutcomesWin, rouletteEvent.OutcomesLose, rouletteEvent.OutcomesCrit };
                _currentActionC = () => ResolveOutcomes(newoutcomes[_currentResult]);
                string[] newkeys = new string[] { rouletteEvent.EndDialogueWinKey, rouletteEvent.EndDialogueLoseKey, rouletteEvent.EndDialogueCritKey };
                _currentKeyC = newkeys[_currentResult];
                setUpButtons(new bool[] { false, true, true, false });
                _dialogueService.SendDialogue(rouletteEvent.StartDialogueKey, ShowButton);
                break;
            
            case DrawEvent drawEvent: //joder como me pone la programacion asincrona joder que bonito y poco mantenible
                _dialogueService.SendDialogue(drawEvent.StartDialogueKey, () =>
                {
                    _drawUI.Display(() => GameManager.Instance.CurrentGameState = GameManager.GameState.OnPlay);
                });
                break;
            
            case ChooseNameEvent chooseNameEvent:
                _dialogueService.SendDialogue(chooseNameEvent.StartDialogueKey, () =>
                {
                    _textInputUI.Display(chooseNameEvent.RequestPhrase, (chosenName) =>
                    {
                        GameManager.Instance.GameInfo.OrgName = chosenName;
                        GameManager.Instance.CurrentGameState = GameManager.GameState.OnPlay;
                    });
                });
                break;
            
            case ChooseSloganEvent chooseSloganEvent:
                _dialogueService.SendDialogue(chooseSloganEvent.StartDialogueKey, () =>
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

    private void OnAButtonClick()
    {
        _dialogueService.SendDialogue(_currentKeyA, _currentActionA);
        HideButton();
    }

    private void OnBButtonClick()
    {
        _dialogueService.SendDialogue(_currentKeyB, _currentActionB);
        HideButton();

    }
    private void OnCButtonClick()
    {
        VisualSpin(_currentSpin);
        //HideButton();

    }
    private void OnDButtonClick()
    {
        print("ey");
        _dialogueService.SendDialogue(_currentKeyC, _currentActionC);
        _wheelCanvas.alpha = 0;
        _wheelCanvas.blocksRaycasts = false;
        HideButton();

    }
    private void setUpButtons(bool[] setUps)
    {
        return;
        for (int i = 0; i < setUps.Length; i++)
        {
            if (i < _buttons.Length)
            {
                _buttons[i].enabled = setUps[i];
            }
            else
            {
                break;
            }
        }
    }
    private void ShowButton()
    {
        _buttonsCanvas.alpha = 1;
        _buttonsCanvas.blocksRaycasts = true;
    }
    private void HideButton()
    {
        _buttonsCanvas.alpha = 0;
        _buttonsCanvas.blocksRaycasts = false;
    }
    private void ResolveOutcomes(Outcomes outcomes)
    {
        _buttonsCanvas.alpha = 0;
        _buttonsCanvas.blocksRaycasts = false;
        print("resolveOutcomes");
        foreach(var outcome in outcomes.Get()) outcome.Execute();
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
        ShowButton();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            print(SpinWheel(new uint[] { 100, 150, 110 }, out int val));
            VisualSpin(val);
        }
    }
}
