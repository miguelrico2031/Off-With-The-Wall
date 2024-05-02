using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EventManager : MonoBehaviour,IEventService
{
    [SerializeField] private CanvasGroup _buttonsCanvas;
private ITextDialogService _textService;
    private IPeopleService _peopleService;

    Event.EventReward _setReward;
    private void Start()
    {
        _textService = GameManager.Instance.Get<ITextDialogService>();
        _peopleService = GameManager.Instance.Get<IPeopleService>();

    }

    public void StartEvent(Event _event)
    {
        if(_event as ChoiceEvent)
        {
            print("es Choice");
            ChoiceEvent _chEvent = _event as ChoiceEvent;
            _buttonsCanvas.GetComponentsInChildren<Button>()[0].onClick.AddListener(delegate {_setReward = _chEvent._rewardA; _textService.SendDialog(_chEvent._endDialogueAKey, GiveRewardSet); });
            _buttonsCanvas.GetComponentsInChildren<Button>()[1].onClick.AddListener(delegate { _setReward = _chEvent._rewardB; _textService.SendDialog(_chEvent._endDialogueAKey, GiveRewardSet); });
            _textService.SendDialog(_chEvent._startDialogueKey,ShowButton);
        }
        if (_event as PassiveEvent)
        {
            print("es pasivo");
            PassiveEvent _pEvent = _event as PassiveEvent;
            _setReward = _pEvent._reward;
            _textService.SendDialog(_pEvent._startDialogueKey, GiveRewardSet) ;

        }

    }
    public void ShowButton()
    {
        _buttonsCanvas.alpha = 1;
        _buttonsCanvas.blocksRaycasts = true;
    }
    public void GiveReward(Event.EventReward _reward)
    {
        _buttonsCanvas.alpha = 0;
        _buttonsCanvas.blocksRaycasts = false;
        switch (_reward._type)
        {
            case Event.RewardType.AddPeople:
                _peopleService.AddPeople((uint)Mathf.RoundToInt(_reward.Valor));
                break;
        }
    }
    public void GiveRewardSet()
    {
        GiveReward(_setReward);
        
    }
}
