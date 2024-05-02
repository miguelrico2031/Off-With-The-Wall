using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Events/ChoiceEvent")]

public class ChoiceEvent : Event
{
    [field: SerializeField] public string _choiceTextA { get; private set; }
    [field: SerializeField] public string _choiceTextB {get; private set; }

[field: SerializeField] public string _endDialogueAKey {get;private set;}
    [field: SerializeField] public string _endDialogueBKey {get;private set;}

    [field: SerializeField] public EventReward _rewardA{get;private set;}
    [field: SerializeField] public EventReward _rewardB{get;private set;}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
